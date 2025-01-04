using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GridHeroIterator : HeroIterator
{
    private GridHerosArray heros;
    private int x = 0, z = 0;//图上的坐标

    public GridHeroIterator(GridHerosArray heros)
    {
        this.heros = heros;
    }

    public GameObject GetNext()
    {
        GameObject hero = null;
        while (hero == null && x < MyMap.hexMapSizeX && z < MyMap.hexMapSizeZ / 2)
        {
            if (heros.GetHero(x, z) != null)
            {
                hero = heros.GetHero(x, z);
            }
            z++;
            if (z == MyMap.hexMapSizeZ / 2)
            {
                z = 0;
                x++;
            }
        }
        return hero;
    }

    public void Reset()
    {
        x = 0;
        z = 0;
    }
}
