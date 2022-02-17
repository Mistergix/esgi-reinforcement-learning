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
