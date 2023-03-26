using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram
{
    internal class Enemey
    {
        public float Health_Point;
        public float Magic_Point;
        public float Armor_Point;
        public object Loot;
        public Point Position;
        public float Speed;
        public float Damage;
        public TimeSpan Cooldown;
        public TimeSpan StunnedState;
        public object Status;
        public void Attack()
        {
        }
        public void Defense()
        {

        }
        public void DamageTaken()
        {

        }
        public void Spawn()
        {

        }
        private void IsAlive()
        {

        }
    }
}
