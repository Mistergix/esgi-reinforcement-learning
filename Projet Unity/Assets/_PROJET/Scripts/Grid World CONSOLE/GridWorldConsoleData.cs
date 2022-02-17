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
        public float bombValue = -100000f;
        public float energyValue = 1000f;
        public float endValue = 10000f;
        public float blankValue = -10f ;
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

        public static Coords operator +(Coords a, Coords b)
        {
            return new Coords(a.x + b.x, a.y + b.y);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Coords other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }
    }
}