using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GridHerosArray : IHerosArray
{
    private GameObject[,] heros;

    public GridHerosArray()
    {
        heros = new GameObject[MyMap.hexMapSizeX, MyMap.hexMapSizeZ];
    }

    public HeroIterator CreateIterator()
    {
        return new GridHeroIterator(this);
    }

    public GameObject GetHero(int x, int z)
    {
        return heros[x, z];
    }

    public void AddHero(GameObject hero, int x, int z)
    {
        heros[x, z] = hero;
    }

    public void RemoveHero(int x, int z)
    {
        heros[x, z] = null;
    }

    public void Clear()
    {
        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ; z++)
            {
                heros[x, z] = null;
            }
        }
    }
}
