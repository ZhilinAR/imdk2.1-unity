/*===============================================================================
Copyright (C) 2024 Immersal - Part of Hexagon. All Rights Reserved.

This file is part of the Immersal SDK.

The Immersal SDK cannot be copied, distributed, or made available to
third-parties for commercial purposes without written permission of Immersal Ltd.

Contact sales@immersal.com for licensing requests.
===============================================================================*/

using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Immersal.XR
{
#if IMMERSAL_XREAL
    /// <summary>
    /// Platform support implementation for the XREAL XR Plugin.
    /// Uses reflection to access XREAL runtime types so that the
    /// SDK does not have a hard compile time dependency on the
    /// plugin assemblies or ARCore.
    /// </summary>
    public class XrealSupport : MonoBehaviour, IPlatformSupport
    {
        private IPlatformConfiguration m_Configuration = new PlatformConfiguration
        {
            CameraDataFormat = CameraDataFormat.SingleChannel
        };

        public Task<IPlatformConfigureResult> ConfigurePlatform()
        {
            return ConfigurePlatform(m_Configuration);
        }

        public Task<IPlatformConfigureResult> ConfigurePlatform(IPlatformConfiguration configuration)
        {
            m_Configuration = configuration;
            // XREAL plugin handles session start automatically so we just
            // store the configuration.
            return Task.FromResult<IPlatformConfigureResult>(new SimplePlatformConfigureResult
            {
                Success = true
            });
        }

        public Task<IPlatformUpdateResult> UpdatePlatform()
        {
            return UpdatePlatform(m_Configuration);
        }

        public async Task<IPlatformUpdateResult> UpdatePlatform(IPlatformConfiguration oneShotConfiguration)
        {
            bool success = false;
            CameraData cameraData = null;
            Vector3 position = Vector3.zero;
            Quaternion rotation = Quaternion.identity;
            Vector4 intrinsics = Vector4.zero;

            // All interaction with the plugin is done via reflection to keep
            // the dependency optional.
            Type frameType = Type.GetType("NRKernal.NRFrame, NRSDK", false);
            if (frameType != null)
            {
                try
                {
                    // Pose
                    var headPoseProp = frameType.GetProperty("HeadPose");
                    if (headPoseProp != null)
                    {
                        Pose pose = (Pose)headPoseProp.GetValue(null);
                        position = pose.position;
                        rotation = pose.rotation;
                    }

                    // Intrinsics
                    var intrinsicsMethod = frameType.GetMethod("GetRGBCameraIntrinsicMatrix") ??
                                           frameType.GetMethod("GetColorCameraIntrinsicMatrix");
                    if (intrinsicsMethod != null)
                    {
                        // Assume Matrix4x4 is returned from the intrinsic method
                        object matrixObj = intrinsicsMethod.Invoke(null, null);
                        if (matrixObj is Matrix4x4 m)
                        {
                            intrinsics = new Vector4(m[0, 2], m[1, 2], m[0, 0], m[1, 1]);
                        }
                    }

                    // Image data
                    var rawDataMethod = frameType.GetMethod("GetRGBCameraRawData") ??
                                        frameType.GetMethod("GetColorCameraRawData");
                    if (rawDataMethod != null)
                    {
                        // Expect byte[] return
                        byte[] bytes = rawDataMethod.Invoke(null, null) as byte[];
                        if (bytes != null)
                        {
                            SimpleImageData imageData = new SimpleImageData(bytes);
                            cameraData = new CameraData(imageData)
                            {
                                Width = 0,
                                Height = 0,
                                Intrinsics = intrinsics,
                                Format = oneShotConfiguration.CameraDataFormat,
                                Channels = oneShotConfiguration.CameraDataFormat == CameraDataFormat.SingleChannel ? 1 : 3,
                                CameraPositionOnCapture = position,
                                CameraRotationOnCapture = rotation,
                                Orientation = Quaternion.identity
                            };
                            success = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    ImmersalLogger.LogError($"XREAL plugin interaction failed: {e.Message}");
                    success = false;
                }
            }

            SimplePlatformStatus status = new SimplePlatformStatus
            {
                TrackingQuality = success ? 1 : 0
            };

            return await Task.FromResult<IPlatformUpdateResult>(new SimplePlatformUpdateResult
            {
                Success = success,
                Status = status,
                CameraData = cameraData
            });
        }

        public Task StopAndCleanUp()
        {
            // XREAL plugin has no explicit shutdown requirements for this support class.
            return Task.CompletedTask;
        }
    }
#else
    /// <summary>
    /// Stub implementation used when IMMERSAL_XREAL is not defined.
    /// </summary>
    public class XrealSupport : MonoBehaviour, IPlatformSupport
    {
        public Task<IPlatformConfigureResult> ConfigurePlatform()
        {
            return Task.FromResult<IPlatformConfigureResult>(new SimplePlatformConfigureResult { Success = false });
        }

        public Task<IPlatformConfigureResult> ConfigurePlatform(IPlatformConfiguration configuration)
        {
            return Task.FromResult<IPlatformConfigureResult>(new SimplePlatformConfigureResult { Success = false });
        }

        public Task<IPlatformUpdateResult> UpdatePlatform()
        {
            return Task.FromResult<IPlatformUpdateResult>(new SimplePlatformUpdateResult { Success = false });
        }

        public Task<IPlatformUpdateResult> UpdatePlatform(IPlatformConfiguration oneShotConfiguration)
        {
            return Task.FromResult<IPlatformUpdateResult>(new SimplePlatformUpdateResult { Success = false });
        }

        public Task StopAndCleanUp()
        {
            return Task.CompletedTask;
        }
    }
#endif
}

