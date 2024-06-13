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
        public Camera mainCamera;
        private CinemachineVirtualCamera _camera;

        protected override void Awake()
        {
            base.Awake();
            mainCamera = Camera.main;
        }

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