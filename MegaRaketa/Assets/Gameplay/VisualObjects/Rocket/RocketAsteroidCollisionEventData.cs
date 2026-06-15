namespace MegaRaketa.Gameplay.VisualObjects.Rocket
{
    public class RocketAsteroidCollisionEventData
    {
        public RocketAsteroidCollisionEventData(float asteroidSize)
        {
            AsteroidSize = asteroidSize;
        }

        public float AsteroidSize { get; }
    }
}
