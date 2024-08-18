namespace grid.entities.monsters
{
    public class Monster: Entity
    {
        private int health;
        private bool isAlive = true;

        public void Damage(int value)
        {
            health -= value;
            if (health < 0)
            {
                health = 0;
                isAlive = false;
            }
        }

        public static void Initialize()
        {
            
        }
    }
}