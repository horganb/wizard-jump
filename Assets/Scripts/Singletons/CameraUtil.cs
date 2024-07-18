using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Singletons
{
    public class CameraUtil : SingletonMonoBehaviour<CameraUtil>
    {
        public Camera mainCamera;
        private PixelPerfectCamera _pixelPerfectCamera;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
            _pixelPerfectCamera = mainCamera.GetComponent<PixelPerfectCamera>();
        }

        public float GetWorldWidth()
        {
            return (float)_pixelPerfectCamera.refResolutionX / _pixelPerfectCamera.assetsPPU;
        }
    }
}