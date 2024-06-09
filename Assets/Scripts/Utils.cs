using System;
using System.Linq;
using System.Reflection;
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

    public static T InstantiateRandomSubclass<T>() where T : class
    {
        var choices = (from t in Assembly.GetExecutingAssembly().GetTypes()
            where t.IsSubclassOf(typeof(T))
            select t).ToArray();
        var index = (int)Math.Floor((decimal)Random.Range(0, choices.Count()));
        return Activator.CreateInstance(choices[index]) as T;
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
}