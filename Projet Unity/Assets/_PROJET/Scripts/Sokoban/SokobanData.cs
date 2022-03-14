using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PGSauce.Games.IaEsgi.Sokoban
{
    [CreateAssetMenu(menuName = Strings.Sokoban + "Level")]
    public class SokobanData : ScriptableObject
    {
        [Min(1)] public int width = 6;
        [Min(1)] public int height = 5;
        public Coords start = new Coords(0, 4);
        public List<Coords> Caisses;
        public List<Coords> Objectifs;
        public List<Coords> Murs;
        public float blankValue = -10f;

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
