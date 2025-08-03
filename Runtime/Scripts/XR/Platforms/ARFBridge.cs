/*===============================================================================
Copyright (C) 2024 Immersal - Part of Hexagon. All Rights Reserved.

This file is part of the Immersal SDK.

The Immersal SDK cannot be copied, distributed, or made available to
third-parties for commercial purposes without written permission of Immersal Ltd.

Contact sales@immersal.com for licensing requests.
===============================================================================*/

using System;
using UnityEngine;
#if IMMERSAL_XREAL
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
#endif

namespace Immersal.XR
{
#if IMMERSAL_XREAL
    /// <summary>
    /// Simple AR Foundation bridge that forwards camera data and pose
    /// to the Immersal session.
    /// </summary>
    public class ARFBridge : MonoBehaviour
    {
        private ARCameraManager m_CameraManager;

        private void Awake()
        {
            m_CameraManager = FindObjectOfType<ARCameraManager>();
        }

        private void Update()
        {
            var session = ImmersalSDK.Instance?.Session;
            if (session == null || m_CameraManager == null)
                return;

            if (m_CameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                using (image)
                {
                    session.GetType().GetMethod("SubmitCpuImage")?.Invoke(session, new object[] { image });
                }
            }

            if (m_CameraManager.TryGetIntrinsics(out XRCameraIntrinsics intr))
            {
                Vector4 intrinsics = new Vector4(intr.focalLength.x, intr.focalLength.y, intr.principalPoint.x, intr.principalPoint.y);
                session.GetType().GetMethod("SetIntrinsics")?.Invoke(session, new object[] { intrinsics });
            }

            if (Camera.main != null)
            {
                Transform cam = Camera.main.transform;
                session.GetType().GetMethod("SetPose")?.Invoke(session, new object[] { cam.position, cam.rotation });
            }
        }
    }
#else
    /// <summary>
    /// Stub bridge when IMMERSAL_XREAL is not defined.
    /// </summary>
    public class ARFBridge : MonoBehaviour { }
#endif
}
