﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Singletons;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Utils
{
    public static float RandomRangeWithPrecision(float min, float max, int decimals)
    {
        var val = Random.Range(min, max);
        return (float)Math.Round((decimal)val, decimals);
    }

    public static float RandomRangeAndSign(float min, float max, int decimals)
    {
        var positiveVal = RandomRangeWithPrecision(min, max, decimals);
        return Random.value < 0.5f ? positiveVal : -positiveVal;
    }

    public static float RandomRangeAndSign(float min, float max)
    {
        var positiveVal = Random.Range(min, max);
        return Random.value < 0.5f ? positiveVal : -positiveVal;
    }

    public static float RandomMaxAndSign(float max)
    {
        return RandomRangeAndSign(0f, max);
    }

    public static Type[] GetSubclasses<T>() where T : class
    {
        return (from t in Assembly.GetExecutingAssembly().GetTypes()
            where t.IsSubclassOf(typeof(T))
            select t).ToArray();
    }

    public static List<T> InstantiateAllSubclasses<T>() where T : class
    {
        return GetSubclasses<T>().Select(cls => Activator.CreateInstance(cls) as T).ToList();
    }

    public static T InstantiateRandomSubclass<T>() where T : class
    {
        return InstantiateRandomSubclassXTimes<T>(1)[0];
    }

    public static List<T> InstantiateRandomSubclassXTimes<T>(int times) where T : class
    {
        var chosenClasses = RandomFromArray(GetSubclasses<T>(), times);
        return chosenClasses.Select(cls => Activator.CreateInstance(cls) as T).ToList();
    }

    public static T RandomFromArray<T>(T[] choices)
    {
        return RandomFromArray(choices, 1)[0];
    }

    public static T[] RandomFromArray<T>(T[] choices, int number)
    {
        var choicesLeft = new List<T>(choices);
        var numToRemove = choices.Length - number;
        for (var i = 0; i < numToRemove; i++) choicesLeft.RemoveAt(RandomIndex(choicesLeft));
        return choicesLeft.ToArray();
    }

    public static int RandomIndex<T>(IEnumerable<T> list)
    {
        return (int)Math.Floor((decimal)Random.Range(0, list.Count()));
    }

    public static void DestroyIfOffscreen(GameObject gameObject)
    {
        var viewportPoint = CameraUtil.Instance.mainCamera.WorldToViewportPoint(gameObject.transform.position);
        if (Math.Abs(viewportPoint.x) > 1.2 || Math.Abs(viewportPoint.y) > 1.2) Object.Destroy(gameObject);
    }

    public static void DestroyIfBelowScreen(GameObject gameObject)
    {
        var viewportPoint = CameraUtil.Instance.mainCamera.WorldToViewportPoint(gameObject.transform.position);
        if (viewportPoint.y < -1.2f) Object.Destroy(gameObject);
    }

    public static Rigidbody2D SpawnProjectile(GameObject projectilePrefab, GameObject startEntity, Vector2 target)
    {
        Vector2 startEntityPosition = startEntity.transform.position;
        var directionVector = (target - startEntityPosition).normalized;
        var startPosition = startEntityPosition + directionVector * 0.5f;
        var projectile = Object.Instantiate(projectilePrefab, startPosition, Quaternion.identity);
        return projectile.GetComponent<Rigidbody2D>();
    }

    public static void ShootAt(Rigidbody2D obj, Vector2 targetPosition, float hSpeed, float dampenDistance = 3f)
    {
        var gravity = Physics2D.gravity.y;
        var startPosition = obj.transform.position;
        var hVelocity = targetPosition.x < startPosition.x ? -hSpeed : hSpeed;
        var xDiff = targetPosition.x - startPosition.x;
        if (Math.Abs(xDiff) < dampenDistance)
            hVelocity *= Math.Abs(xDiff) / dampenDistance; // dampen horizontal velocity if target is close
        var yDiff = targetPosition.y - startPosition.y;
        var vVelocity = 0f;
        if (hVelocity != 0f)
        {
            var exp1 = xDiff / hVelocity;
            vVelocity = (yDiff - 0.5f * gravity * (float)Math.Pow(exp1, 2)) / exp1;
        }
        else if (yDiff > 0)
        {
            vVelocity = (float)Math.Sqrt(-2f * gravity * yDiff);
        }

        obj.AddForce(new Vector2(hVelocity, vVelocity) * obj.mass, ForceMode2D.Impulse);
    }

    public static List<float> GetIntervalsAroundZero(int numItems, float gap)
    {
        List<float> offsets = new();
        for (var i = 0; i < numItems; i++) offsets.Add((i - (numItems - 1) / 2f) * gap);
        return offsets;
    }

    public static bool IsBetween(GameObject target, GameObject obj1, GameObject obj2)
    {
        var boundaries = new List<float>
            { obj1.transform.position.y, obj2.transform.position.y };
        boundaries.Sort();
        var targetY = target.transform.position.y;
        return targetY > boundaries[0] && targetY < boundaries[1];
    }

    public static bool FlipXToFace(Transform transform, Vector2 target, bool leftIsForward = false)
    {
        var transform1 = transform;
        var targetToLeft = target.x < transform1.position.x;
        var scale = transform1.localScale;
        var oldXScale = scale.x;
        scale.x = targetToLeft ? -1f : 1f;
        if (leftIsForward) scale.x *= -1;
        transform.localScale = scale;
        return Math.Abs(oldXScale - scale.x) > 0.01f;
    }
}