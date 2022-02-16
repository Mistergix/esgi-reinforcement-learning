using System;
using System.Collections.Generic;
using UnityEngine;

namespace PGSauce.Games.IaEsgi.GridWorldConsole
{
    [CreateAssetMenu(menuName = Strings.GridWorldConsole + "Level")]
    public class GridWorldConsoleData : ScriptableObject
    {
        [Min(1)] public int width = 6;
        [Min(1)] public int height = 5;
        public Coords start = new Coords(0,4);
        public Coords end = new Coords(4, 0);
        public List<Coords> bombs;
        public List<Coords> energy;
    }
    
    [Serializable]
    public struct Coords
    {
        public int x;
        public int y;

        public Coords(int x, int y)
        {
            this.y = y;
            this.x = x;
        }
    }
}