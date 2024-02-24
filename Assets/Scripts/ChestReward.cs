using UnityEngine;

public interface IChestReward
{
    public string Name();
    public void Acquire();
    public Sprite GetSprite();
}