using System;
using Cinemachine;
using UnityEngine;

namespace Singletons
{
    public class CameraUtil : SingletonMonoBehaviour<CameraUtil>
    {
        public float worldWidth = 20f;
        public float maxWidthHeightRatio = 2f;
        public RectTransform cameraCanvas;
        private CinemachineVirtualCamera _camera;

        private void Start()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            cameraCanvas.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldWidth);
            var widthHeightRatio = Screen.width / (float)Screen.height;
            _camera.m_Lens.OrthographicSize = worldWidth / Math.Min(widthHeightRatio, maxWidthHeightRatio) / 2;
        }
    }
}