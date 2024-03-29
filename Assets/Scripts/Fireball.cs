﻿using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 2f;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var fireballViewportPoint = _mainCamera.WorldToViewportPoint(gameObject.transform.position);
        if (fireballViewportPoint.x > 1.2 || Math.Abs(fireballViewportPoint.y) > 1.2) Destroy(gameObject);
        var o = gameObject;
        var pos = o.transform.position;
        pos += o.transform.right * (Time.deltaTime * speed);
        o.transform.position = pos;
    }
}