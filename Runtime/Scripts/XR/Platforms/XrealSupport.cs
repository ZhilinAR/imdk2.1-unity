using System.Threading.Tasks;
using UnityEngine;

namespace Immersal.XR
{
    public class XrealSupport : MonoBehaviour, IPlatformSupport
    {
        public async Task<IPlatformConfigureResult> ConfigurePlatform()
        {
            PlatformConfiguration config = new PlatformConfiguration
            {
                CameraDataFormat = CameraDataFormat.RGB
            };
            return await ConfigurePlatform(config);
        }

        public async Task<IPlatformConfigureResult> ConfigurePlatform(IPlatformConfiguration configuration)
        {
            await Task.CompletedTask;
            return new SimplePlatformConfigureResult { Success = true };
        }

        public async Task<IPlatformUpdateResult> UpdatePlatform()
        {
            return await UpdatePlatform(null);
        }

        public async Task<IPlatformUpdateResult> UpdatePlatform(IPlatformConfiguration oneShotConfiguration)
        {
            await Task.CompletedTask;
            CameraData dummy = new CameraData(new SimpleImageData(new byte[0]))
            {
                Width = 0,
                Height = 0,
                Channels = 3,
                Format = CameraDataFormat.RGB,
                Intrinsics = Vector4.zero,
                CameraPositionOnCapture = Vector3.zero,
                CameraRotationOnCapture = Quaternion.identity,
                Orientation = Quaternion.identity,
                Distortion = new double[0]
            };

            return new SimplePlatformUpdateResult
            {
                Success = false,
                Status = new SimplePlatformStatus { TrackingQuality = 0 },
                CameraData = dummy
            };
        }

        public async Task StopAndCleanUp()
        {
            await Task.CompletedTask;
        }
    }
}
