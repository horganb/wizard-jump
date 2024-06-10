namespace Singletons
{
    public class Level
    {
        public float BigSlimeChance;
        public int Size;
        public float SlimeChance;
        public float WaspChance;

        public Level(float slimeChance = 0f, float waspChance = 0f, float bigSlimeChance = 0f, int size = 30)
        {
            Size = size;
            SlimeChance = slimeChance;
            WaspChance = waspChance;
            BigSlimeChance = bigSlimeChance;
        }
    }
}