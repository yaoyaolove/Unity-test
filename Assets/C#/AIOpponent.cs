// =============================================================================
// 文件名称: AIOpponent.cs
// 作者: 刘垚
// 创建日期: 2024.11.24
// 更新日期：2024.11.24
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    public Shop shop;
    public MyMap map;
    public GameManager gameManager;

    //棋盘英雄数组，这里只存AI的英雄
    public GameObject[,] gridHerosArray;

    //当玩家输掉一轮后收到的伤害
    public int AIDamage = 2;

    //地图创建后调用
    public void OnMapReady()
    {
        gridHerosArray = new GameObject[MyMap.hexMapSizeX, MyMap.hexMapSizeZ / 2];

        AddRandomHero();
    }

    //游戏阶段结束后调用
    public void OnGameStageComplete(GameStage stage)
    {
        if (stage == GameStage.Preparation)
        {
            //start champion combat
            for (int x = 0; x < MyMap.hexMapSizeX; x++)
            {
                for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
                {
                    //there is a champion
                    if (gridHerosArray[x, z] != null)
                    {
                        //get character
                        HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

                        //start combat
                        heroController.OnCombatStart();
                    }

                }
            }
        }
    }

    private void GetEmptySlot(out int emptyIndexX, out int emptyIndexZ)
    {
        emptyIndexX = -1;
        emptyIndexZ = -1;

        //get first empty inventory slot
        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] == null)
                {
                    emptyIndexX = x;
                    emptyIndexZ = z;
                    break;
                }
            }
        }
    }

    public void AddRandomHero()
    {
        int indexX;
        int indexZ;
        GetEmptySlot(out indexX, out indexZ);

        //如果没有空位置就不再添加英雄
        if (indexX == -1 || indexZ == -1)
            return;

        Hero hero = shop.GetRandomHeroInfo();

        GameObject heroPrefab = Instantiate(hero.prefab);

        gridHerosArray[indexX, indexZ] = heroPrefab;

        HeroController heroController = heroPrefab.GetComponent<HeroController>();

        heroController.Init(hero, HeroController.TEAMID_AI);

        heroController.SetGridPosition(MyMap.GRIDTYPE_HEXA_MAP, indexX, indexZ + 4);

        heroController.SetWorldPosition();
        heroController.SetWorldRotation();
    }

    public void OnHeroDeath()
    {
        bool allDead = IsAllHeroDead();

        if (allDead)
            gameManager.EndRound();
    }

    private bool IsAllHeroDead()
    {
        //同GameManager
        int heroCount = 0;
        int heroDead = 0;

        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] != null)
                {
                    HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

                    heroCount++;

                    if (heroController.isDead)
                        heroDead++;
                }
            }
        }
        if (heroDead == heroCount)
            return true;

        return false;
    }
}
