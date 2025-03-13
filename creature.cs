using System;

namespace DungeonExplorer
{
    public class Creature
    {
        private string v1;
        private int v2;
        private int v3;

        public Creature(string v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public int Health { get; internal set; }
        public object Name { get; internal set; }

        internal int Attack()
        {
            throw new NotImplementedException();
        }

        internal void TakeDamage(int playerDamage)
        {
            throw new NotImplementedException();
        }
    }
}