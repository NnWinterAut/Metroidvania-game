using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDiagram
{
    internal class Map
    {
        public float Difficulty;
        public object BGM;
        public object MapContent;
        public object Enemies;
        public object Player;
        public object Shop;
        public TimeSpan ClearanceTimer;
        public float ClearanceScore;
        public object Hint;
        public object Boss;
        public void LoadTutorial() { }
        public void LoadLevel()
        {

        }
        public void SpawnEnemy() { }
        public void SpawnPlayer() { }
        public void SpawnShop() { }
        public void SpawnMapContent() { }
        public void ReturnToMenu() { }
    }
}
