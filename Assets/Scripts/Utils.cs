using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interactable;
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

    public static T InstantiateRandomSubclass<T>(List<Type> exclude = null) where T : class
    {
        var choices = (from t in Assembly.GetExecutingAssembly().GetTypes()
            where t.IsSubclassOf(typeof(T)) && (exclude == null || !exclude.Contains(t))
            select t).ToArray();
        var index = (int)Math.Floor((decimal)Random.Range(0, choices.Length));
        return Activator.CreateInstance(choices[index]) as T;
    }

    public static List<T> InstantiateRandomSubclassXTimes<T>(int times) where T : class
    {
        List<T> instances = new();
        List<Type> usedTypes = new();
        for (var i = 0; i < times; i++)
        {
            var newEntity = InstantiateRandomSubclass<T>(usedTypes);
            instances.Add(newEntity);
            usedTypes.Add(newEntity.GetType());
        }

        return instances;
    }

    public static IChestReward InstantiateRandomChestReward()
    {
        var choices = (from t in Assembly.GetExecutingAssembly().GetTypes()
            where !t.IsAbstract && typeof(IChestReward).IsAssignableFrom(t)
            select t).ToArray();
        var rewardType = RandomFromArray(choices);
        return Activator.CreateInstance(rewardType) as IChestReward;
    }

    public static T RandomFromArray<T>(T[] choices)
    {
        var index = (int)Math.Floor((decimal)Random.Range(0, choices.Length));
        return choices[index];
    }

    public static void DestroyIfOffscreen(GameObject gameObject)
    {
        var viewportPoint = CameraUtil.Instance.mainCamera.WorldToViewportPoint(gameObject.transform.position);
        if (Math.Abs(viewportPoint.x) > 1.2 || Math.Abs(viewportPoint.y) > 1.2) Object.Destroy(gameObject);
    }
}