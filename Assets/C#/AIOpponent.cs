// =============================================================================
// 文件名称: AIOpponent.cs
// 作者: 刘垚
// 创建日期: 2024.11.24
// 更新日期：2024.11.24
// 使用的设计模式：
// 备注：
// =============================================================================
using System.Collections.Generic;
using UnityEngine;

public class AIOpponent : MonoBehaviour
{
    public Shop shop;
    public MyMap map;

    //棋盘英雄数组，这里只存AI的英雄
    public GameObject[,] gridHerosArray;

    //计算AI方的羁绊
    public Dictionary<HeroType, int> heroTypeCount;
    public List<HeroBonus> activeBonusList;

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

        if (stage == GameStage.Combat)
        {
            //totall damage player takes
            int damage = 0;

            //iterate champions
            //start champion combat
            for (int x = 0; x < MyMap.hexMapSizeX; x++)
            {
                for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
                {
                    if (gridHerosArray[x, z] != null)
                    {
                        HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

                        if (heroController.currentHealth > 0)
                            damage += AIDamage;
                    }
                }
            }

            //玩家收到伤害
            GameManager.Instance.TakeDamage(damage);

            ResetHeros();

            AddRandomHero();
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
        GetEmptySlot(out int indexX, out int indexZ);

        //如果没有空位置就不再添加英雄
        if (indexX == -1 || indexZ == -1)
            return;

        int heroIndex = shop.GetRandomHeroIndex();
        IHero hero=GameManager.Instance.gameHeroData.herosArray[heroIndex];

        GameObject heroPrefab = Instantiate(GameManager.Instance.gameHeroData.prefabs[heroIndex]);

        gridHerosArray[indexX, indexZ] = heroPrefab;

        HeroController heroController = heroPrefab.GetComponent<HeroController>();

        heroController.Init(heroIndex, HeroController.TEAMID_AI);

        heroController.SetGridPosition(MyMap.GRIDTYPE_HEXA_MAP, indexX, indexZ + 4);

        heroController.SetWorldPosition();
        heroController.SetWorldRotation();

        //检查英雄升级
        List<HeroController> championList_lvl_1 = new List<HeroController>();
        List<HeroController> championList_lvl_2 = new List<HeroController>();

        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] != null)
                {
                    HeroController cc = gridHerosArray[x, z].GetComponent<HeroController>();

                    if (cc.hero == hero)
                    {
                        if (cc.lvl == 1)
                            championList_lvl_1.Add(cc);
                        else if (cc.lvl == 2)
                            championList_lvl_2.Add(cc);
                    }
                }
            }
        }

        if (championList_lvl_1.Count == 3)
        {
            championList_lvl_1[2].UpgradeLevel();

            Destroy(championList_lvl_1[0].gameObject);
            Destroy(championList_lvl_1[1].gameObject);

            if (championList_lvl_2.Count == 2)
            {
                championList_lvl_1[2].UpgradeLevel();

                Destroy(championList_lvl_2[0].gameObject);
                Destroy(championList_lvl_2[1].gameObject);
            }
        }

        CalculateBonuses();
    }

    public void OnHeroDeath()
    {
        bool allDead = IsAllHeroDead();

        if (allDead)
            GameManager.Instance.EndRound();
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

    private void ResetHeros()
    {
        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] != null)
                {
                    HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

                    heroController.Reset();
                }

            }
        }
    }

    private void CalculateBonuses()
    {
        heroTypeCount = new Dictionary<HeroType, int>();

        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] != null)
                {
                    IHero c = gridHerosArray[x, z].GetComponent<HeroController>().hero;

                    if (heroTypeCount.ContainsKey(c.Type1))
                    {
                        int cCount = 0;
                        heroTypeCount.TryGetValue(c.Type1, out cCount);

                        cCount++;

                        heroTypeCount[c.Type1] = cCount;
                    }
                    else
                    {
                        heroTypeCount.Add(c.Type1, 1);
                    }

                    if (heroTypeCount.ContainsKey(c.Type2))
                    {
                        int cCount = 0;
                        heroTypeCount.TryGetValue(c.Type2, out cCount);

                        cCount++;

                        heroTypeCount[c.Type2] = cCount;
                    }
                    else
                    {
                        heroTypeCount.Add(c.Type2, 1);
                    }

                }
            }
        }

        activeBonusList = new List<HeroBonus>();

        foreach (KeyValuePair<HeroType, int> m in heroTypeCount)
        {
            HeroBonus heroBonus = m.Key.heroBonus;

            if (m.Value >= heroBonus.heroCount)
            {
                activeBonusList.Add(heroBonus);
            }
        }

    }

    public void Restart()
    {
        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] != null)
                {
                    //get character
                    HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

                    Destroy(heroController.gameObject);
                    gridHerosArray[x, z] = null;

                }

            }
        }
        AddRandomHero();
    }
}
