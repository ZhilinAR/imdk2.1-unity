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
#if IMMERSAL_XREAL
using NRKernal;
#endif

namespace Immersal.XR
{
    /// <summary>
    /// Platform support for XREAL (NRSDK) devices.
    /// Provides RGB camera frames and pose information through the Immersal
    /// platform interfaces.
    /// </summary>
    public class XrealSupport : MonoBehaviour, IPlatformSupport
    {
#if IMMERSAL_XREAL
        private NRSessionManager m_SessionManager;
        private NRRGBCamera m_Camera;
        private bool m_Configured;

        public async Task<IPlatformConfigureResult> ConfigurePlatform()
        {
            return await ConfigurePlatform(new PlatformConfiguration
            {
                CameraDataFormat = CameraDataFormat.RGB
            });
        }

        public async Task<IPlatformConfigureResult> ConfigurePlatform(IPlatformConfiguration configuration)
        {
            m_SessionManager = NRSessionManager.Instance;
            m_Camera = NRSessionManager.Instance.RGBCamera;
            m_Camera?.Play();
            m_Configured = true;

            return await Task.FromResult(new SimplePlatformConfigureResult
            {
                Success = true
            });
        }

        public async Task<IPlatformUpdateResult> UpdatePlatform()
        {
            return await UpdatePlatform(new PlatformConfiguration
            {
                CameraDataFormat = CameraDataFormat.RGB
            });
        }

        public async Task<IPlatformUpdateResult> UpdatePlatform(IPlatformConfiguration configuration)
        {
            if (!m_Configured || m_Camera == null)
            {
                return await Task.FromResult(new SimplePlatformUpdateResult
                {
                    Success = false
                });
            }

            Pose pose = NRFrame.HeadPose;
            Texture2D tex = m_Camera.ActiveTexture;
            byte[] bytes = tex != null ? tex.GetRawTextureData() : Array.Empty<byte>();

            SimpleImageData imgData = new SimpleImageData(bytes);
            CameraData cam = new CameraData(imgData)
            {
                Width = tex ? tex.width : 0,
                Height = tex ? tex.height : 0,
                Channels = 3,
                Format = CameraDataFormat.RGB,
                Intrinsics = Vector4.zero,
                CameraPositionOnCapture = pose.position,
                CameraRotationOnCapture = pose.rotation,
                Orientation = pose.rotation
            };

            return await Task.FromResult(new SimplePlatformUpdateResult
            {
                Success = true,
                CameraData = cam,
                Status = new SimplePlatformStatus
                {
                    TrackingQuality = (int)NRFrame.GetTrackingState()
                }
            });
        }

        public async Task StopAndCleanUp()
        {
            if (m_Camera != null)
            {
                m_Camera.Stop();
                m_Camera = null;
            }

            if (m_SessionManager != null)
            {
                m_SessionManager.StopSession();
                m_SessionManager.DestroySession();
                m_SessionManager = null;
            }

            m_Configured = false;
            await Task.CompletedTask;
        }
#else
        // Stub implementation when NRSDK is not available.
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

        public Task<IPlatformUpdateResult> UpdatePlatform(IPlatformConfiguration configuration)
        {
            return Task.FromResult<IPlatformUpdateResult>(new SimplePlatformUpdateResult { Success = false });
        }

        public Task StopAndCleanUp()
        {
            return Task.CompletedTask;
        }
#endif
    }
}
