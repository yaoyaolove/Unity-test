using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GridHeroIterator : HeroIterator
{
    private GridHerosArray heros;
    private int prex = 0, prez = 0;//图上的坐标

    public GridHeroIterator(GridHerosArray heros)
    {
        this.heros = heros;
    }

    public GameObject GetNext()
    {
        for (int x = prex; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = prez; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (heros.GetHero(x, z) != null)
                {
                    prex = x;
                    prez = z + 1;
                    return heros.GetHero(x, z);
                }
            }
        }
        return null;
    }

    public void Reset()
    {
        prex = 0;
        prez = 0;
    }
}
