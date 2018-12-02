﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoskusCiv2.Enums;

namespace PoskusCiv2.Terrains
{
    internal class Fruit : BaseTerrain
    {
        public Fruit() : base(1, 0, 0, 0, 0, 0, 0, 0, 0, 15, 15)
        {
            Type = TerrainType.Fruit;
            Name = "Jungle";
            SpecialName = "Fruit";
        }
    }
}
