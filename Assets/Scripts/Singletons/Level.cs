namespace Singletons
{
    public class Level
    {
        public float EnemyPower;
        public int Size;

        public Level(float power, int size = 30)
        {
            Size = size;
            EnemyPower = power;
        }
    }
}