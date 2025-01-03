// =============================================================================
// 文件名称: GameManager.cs
// 作者: 刘垚
// 创建日期: 2024.11.18
// 更新日期：2024.11.27
// 使用的设计模式：单例模式
// 备注：游戏的管理类，负责游戏进程等
// =============================================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

//游戏阶段

public enum GameStage { Preparation, Combat, Loss };
public class GameManager : MonoBehaviour
{
    //单例模式维护的静态变量
    private static GameManager Instance;
    public static GameManager GetInstance()=> Instance;
    // 在Awake中确保只存在一个实例
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  // 设置单例实例
            DontDestroyOnLoad(gameObject);  // 保证在场景切换时不销毁
        }
        else
        {
            Destroy(gameObject);  // 防止其他实例创建
        }
    }

    public readonly int HeroCounts = 9;
    public MyMap map;
    public UI uI;
    public InputController inputController;
    public GameHeroData gameHeroData;
    public Shop shop;
    public AIOpponent aIOpponent;

    public IGameStage gameStage;
    public GameStage currentGameStage;
    
    //用于系统计时的私有临时变量
    private float timer = 0;

    //放置英雄的时间
    public int preparationStageDuration = 16;
    //对战时间
    public int combatStageDuration = 60;
    //每回合后得到的基础金币数
    public int baseGoldIncome = 5;

    //我方备战席英雄数组
    [HideInInspector]
    public GameObject[] ownHeroInventoryArray;
    //对方备战席英雄数组
    [HideInInspector]
    public GameObject[] oponentHeroInventoryArray;
    //棋盘英雄数组,这里只存玩家的英雄
    [HideInInspector]
    public GameObject[,] gridHerosArray;

    [HideInInspector]
    public int currentLevel = 3;
    [HideInInspector]
    public int currentHeroCount = 0;
    [HideInInspector]
    public int currentGold = 5;
    [HideInInspector]
    public int currentHP = 100;
    [HideInInspector]
    public int timerDisplay = 0;

    //计算羁绊的两个变量
    public Dictionary<HeroType, int> heroTypeCount;
    public List<HeroBonus> activeBonusList;

    //两个在拖动英雄时维护的变量
    private GameObject draggedHero = null;
    private TriggerInfo dragStartTrigger = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //设定游戏开始阶段
        gameStage = new Preparation();
        currentGameStage =GameStage.Preparation;

        //初始化数组
        ownHeroInventoryArray = new GameObject[MyMap.inventorySize];
        oponentHeroInventoryArray = new GameObject[MyMap.inventorySize];
        gridHerosArray = new GameObject[MyMap.hexMapSizeX, MyMap.hexMapSizeZ / 2];

        uI.UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        gameStage.Update();
        //if (currentGameStage == GameStage.Preparation)
        //{
        //    timer += Time.deltaTime;

        //    timerDisplay = (int)(preparationStageDuration - timer);

        //    uI.UpdateTimerText();

        //    if (timer > preparationStageDuration)
        //    {
        //        timer = 0;

        //        OnGameStageComplete();
        //    }
        //}
        //else if (currentGameStage == GameStage.Combat)
        //{
        //    timer += Time.deltaTime;

        //    timerDisplay = (int)timer;

        //    if (timer > combatStageDuration)
        //    {
        //        timer = 0;

        //        OnGameStageComplete();
        //    }
        //}
    }

    public void ChangeStage(IGameStage gameStage)
    {
        currentGameStage = gameStage.GetName();
        this.gameStage = gameStage;
    }

    //当游戏阶段结束
    public void OnGameStageComplete()
    {
        //告知AI阶段完成
        aIOpponent.OnGameStageComplete(currentGameStage);
        //如果准备阶段完成
        gameStage.OnGameStageComplete();
        //if (currentGameStage == GameStage.Preparation)
        //{
        //    //进入战斗阶段
        //    currentGameStage = GameStage.Combat;

        //    //将指示器均隐藏，这是为了防止你在拖拽英雄的过程中准备阶段结束
        //    map.HideIndicators();

        //    //计时器隐藏
        //    uI.SetTimerTextActive(false);

        //    if (draggedHero != null)
        //    {
        //        //这里要记住将英雄拖拽信息更新    
        //        draggedHero.GetComponent<HeroController>().IsDragged = false;
        //        draggedHero = null;
        //    }

        //    //备战席
        //    for (int i = 0; i < ownHeroInventoryArray.Length; i++)
        //    {
        //        if (ownHeroInventoryArray[i] != null)
        //        {
        //            HeroController heroController = ownHeroInventoryArray[i].GetComponent<HeroController>();

        //            heroController.OnCombatStart();
        //        }
        //    }

        //    //六边形棋盘
        //    for (int x = 0; x < MyMap.hexMapSizeX; x++)
        //    {
        //        for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
        //        {
        //            if (gridHerosArray[x, z] != null)
        //            {
        //                HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

        //                heroController.OnCombatStart();
        //            }

        //        }
        //    }


        //    //检查是否没有英雄，会直接判负，回合结束
        //    if (IsAllHeroDead())
        //        EndRound();

        //}
        //else if (currentGameStage == GameStage.Combat)
        //{
        //    currentGameStage = GameStage.Preparation;

        //    uI.SetTimerTextActive(true);

        //    ResetHeros();

        //    //尝试对可能升级的英雄进行升级
        //    for (int i = 0; i < gameHeroData.herosArray.Length; i++)
        //    {
        //        TryUpgradeHero(gameHeroData.herosArray[i]);
        //    }

        //    //增加加金币
        //    currentGold += CalculateIncome();

        //    //更新UI
        //    uI.UpdateUI();

        //    //刷新商店
        //    shop.RefreshShop(true);

        //    //检查是否失败
        //    if (currentHP <= 0)
        //    {
        //        currentGameStage = GameStage.Loss;
        //        uI.ShowLossScreen();
        //    }

        //}
    }


    //从商店购买英雄的具体实现
    public bool BuyHeroFromShop(int heroIndex)
    {
        IHero hero=gameHeroData.herosArray[heroIndex];
        GameObject prefab = gameHeroData.prefabs[heroIndex];
        //得到第一个空的备战席位置
        int emptyIndex = -1;
        for (int i = 0; i < ownHeroInventoryArray.Length; i++)
        {
            if (ownHeroInventoryArray[i] == null)
            {
                emptyIndex = i;
                break;
            }
        }

        //如果没有空的备战席
        if (emptyIndex == -1)
            return false;

        //如果钱不够
        if (currentGold<hero.Cost)
            return false;
        
        //实例化一个预制件
        GameObject heroPrefab = Instantiate(prefab);

        //为该游戏对象添加一个英雄控制器的组件
        HeroController heroController = heroPrefab.GetComponent<HeroController>();

        //初始化一个英雄控制器
        heroController.CreateHero();
        heroController.Init(HeroController.TEAMID_PLAYER);

        //设置英雄的网格坐标
        heroController.SetGridPosition(MyMap.GRIDTYPE_OWN_INVENTORY, emptyIndex, -1);

        //设置该英雄的位置与旋转
        heroController.SetWorldPosition();
        heroController.SetWorldRotation();

        //将英雄存入备战席数组
        StoreHeroInArray(MyMap.GRIDTYPE_OWN_INVENTORY, map.ownTriggerArray[emptyIndex].gridX, -1, heroPrefab);

        //准备阶段尝试进行英雄的升级
        if (currentGameStage == GameStage.Preparation)
            TryUpgradeHero(hero); 

        //减少金币
        currentGold -=hero.Cost;

        //更新金币数
        uI.UpdateUI();

        return true;
    }

    //尝试对英雄进行升级
    public void TryUpgradeHero(IHero hero)
    {
        //用于统计该类型英雄的一星和二星英雄个数而设置的临时变量
        List<HeroController> heroList_lvl_1 = new List<HeroController>();
        List<HeroController> heroList_lvl_2 = new List<HeroController>();

        //计算备战席的英雄
        for (int i = 0; i < ownHeroInventoryArray.Length; i++)
        {
            if (ownHeroInventoryArray[i] != null)
            {
                HeroController heroController = ownHeroInventoryArray[i].GetComponent<HeroController>();

                if (heroController.hero == hero)
                {
                    if (heroController.lvl == 1)
                        heroList_lvl_1.Add(heroController);
                    else if (heroController.lvl == 2)
                        heroList_lvl_2.Add(heroController);
                }
            }

        }
        //计算棋盘上的英雄
        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] != null)
                {
                    HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

                    if (heroController.hero == hero)
                    {
                        if (heroController.lvl == 1)
                            heroList_lvl_1.Add(heroController);
                        else if (heroController.lvl == 2)
                            heroList_lvl_2.Add(heroController);
                    }
                }

            }
        }

        //如果有三个相同英雄，我们进行升级，删除其余两个
        if (heroList_lvl_1.Count > 2)
        {
            //升级
            heroList_lvl_1[2].UpgradeLevel();

            //移除其余两个
            RemoveHeroFromArray(heroList_lvl_1[0].gridType, heroList_lvl_1[0].gridPositionX, heroList_lvl_1[0].gridPositionZ);
            RemoveHeroFromArray(heroList_lvl_1[1].gridType, heroList_lvl_1[1].gridPositionX, heroList_lvl_1[1].gridPositionZ);

            //删除游戏对象
            Destroy(heroList_lvl_1[0].gameObject);
            Destroy(heroList_lvl_1[1].gameObject);

            //尝试升级到三级
            if (heroList_lvl_2.Count > 1)
            {
                //升级
                heroList_lvl_1[2].UpgradeLevel();

                //同理
                RemoveHeroFromArray(heroList_lvl_2[0].gridType, heroList_lvl_2[0].gridPositionX, heroList_lvl_2[0].gridPositionZ);
                RemoveHeroFromArray(heroList_lvl_2[1].gridType, heroList_lvl_2[1].gridPositionX, heroList_lvl_2[1].gridPositionZ);

                //同理
                Destroy(heroList_lvl_2[0].gameObject);
                Destroy(heroList_lvl_2[1].gameObject);
            }
        }

        //更新棋盘上人数
        currentHeroCount = GetHeroCountOnHexGrid();

        //更新UI
        uI.UpdateUI();
    }

    //从商店购买经验的具体实现
    public void BuyLevelFromShop()
    {
        if (currentGold < 4)
            return;

        if (currentLevel < 9)
        { 
            currentLevel++;
            currentGold -= 4;
            uI.UpdateUI();
        }
    }

    //在地图上拖拽英雄,该函数只在鼠标点击时调用
    public void StartDrag()
    {
        if (currentGameStage != GameStage.Preparation)
            return;

        //获取触发器信息
        TriggerInfo triggerinfo = inputController.triggerInfo;

        if (triggerinfo != null)
        {
            dragStartTrigger = triggerinfo;

            GameObject heroGO = GetHeroFromTriggerInfo(triggerinfo);

            if (heroGO != null)
            {
                //展示所有指示器
                map.ShowIndicators();

                draggedHero = heroGO;

                heroGO.GetComponent<HeroController>().IsDragged = true;
            }

        }

    }

    public void StopDrag()
    {
        //隐藏所有指示器
        map.HideIndicators();

        //六边形棋盘上的英雄个数
        int herosOnField = GetHeroCountOnHexGrid();

        //判断是取消拖拽英雄的操作
        if(draggedHero != null)
        {
            //将移动状态改回
            draggedHero.GetComponent<HeroController>().IsDragged = false;

            TriggerInfo triggerinfo = inputController.triggerInfo;

            //判断鼠标落在有效区域
            if(triggerinfo != null)
            {
                GameObject currentTriggerHero = GetHeroFromTriggerInfo(triggerinfo);
                //如果拖拽到的位置已经有英雄，就将两个英雄交换位置
                if (currentTriggerHero != null)
                {
                    StoreHeroInArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ, currentTriggerHero);

                    StoreHeroInArray(triggerinfo.gridType, triggerinfo.gridX, triggerinfo.gridZ, draggedHero);
                }
                //拖拽的地方没有英雄
                else
                {
                    //如果将英雄拖拽到六边形网格
                    if (triggerinfo.gridType == MyMap.GRIDTYPE_HEXA_MAP)
                    {
                        //两种情况，一种是从备战席往六边形网格放，另一种是在六边形网格上调整位置
                        if (herosOnField < currentLevel || dragStartTrigger.gridType == MyMap.GRIDTYPE_HEXA_MAP)
                        {
                            //将原来位置上的英雄删除
                            RemoveHeroFromArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ);

                            //将英雄添加到新位置
                            StoreHeroInArray(triggerinfo.gridType, triggerinfo.gridX, triggerinfo.gridZ, draggedHero);

                            if (dragStartTrigger.gridType != MyMap.GRIDTYPE_HEXA_MAP)
                                herosOnField++;
                        }
                    }
                    //如果将英雄拖到备战席
                    else if (triggerinfo.gridType == MyMap.GRIDTYPE_OWN_INVENTORY)
                    {
                        RemoveHeroFromArray(dragStartTrigger.gridType, dragStartTrigger.gridX, dragStartTrigger.gridZ);

                        StoreHeroInArray(triggerinfo.gridType, triggerinfo.gridX, triggerinfo.gridZ, draggedHero);

                        if (dragStartTrigger.gridType == MyMap.GRIDTYPE_HEXA_MAP)
                            herosOnField--;
                    }
                }
            }
            //每次拖动结束进行羁绊的重新计算
            CalculateBonuses();

            currentHeroCount = GetHeroCountOnHexGrid();

            uI.UpdateUI();

            //这是检测的要求
            draggedHero = null;
        }

    }

    //通过触发器信息获得英雄对象
    private GameObject GetHeroFromTriggerInfo(TriggerInfo triggerinfo)
    {
        //临时变量
        GameObject heroGO = null;

        if (triggerinfo.gridType == MyMap.GRIDTYPE_OWN_INVENTORY)
        {
            heroGO = ownHeroInventoryArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == MyMap.GRIDTYPE_OPONENT_INVENTORY)
        {
            heroGO = oponentHeroInventoryArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == MyMap.GRIDTYPE_HEXA_MAP)
        {
            heroGO = gridHerosArray[triggerinfo.gridX, triggerinfo.gridZ];
        }

        return heroGO;
    }

    //将英雄存入数组中并将英雄设置在新的位置上
    private void StoreHeroInArray(int gridType, int gridX, int gridZ, GameObject hero)
    {
        HeroController heroController = hero.GetComponent<HeroController>();
        heroController.SetGridPosition(gridType, gridX, gridZ);

        if (gridType == MyMap.GRIDTYPE_OWN_INVENTORY)
        {
            ownHeroInventoryArray[gridX] = hero;
        }
        else if (gridType == MyMap.GRIDTYPE_HEXA_MAP)
        {
            gridHerosArray[gridX, gridZ] = hero;
        }
    }

    //将英雄去除数组
    private void RemoveHeroFromArray(int gridType, int gridX, int gridZ)
    {
        if (gridType == MyMap.GRIDTYPE_OWN_INVENTORY)
        {
            ownHeroInventoryArray[gridX] = null;
        }
        else if (gridType == MyMap.GRIDTYPE_HEXA_MAP)
        {
            gridHerosArray[gridX, gridZ] = null;
        }
    }

    //得到六边形棋盘上的所有英雄对象的数量
    private int GetHeroCountOnHexGrid()
    {
        int count = 0;
        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                if (gridHerosArray[x, z] != null)
                {
                    count++;
                }
            }
        }
        return count;
    }

    //当一个英雄死亡时调用
    public void OnHeroDeath()
    {
        bool allDead=IsAllHeroDead();

        if(allDead)
            EndRound();
    }

    //判断英雄全部阵亡,英雄死亡但是游戏对象仍存在，所以可以遍历
    public bool IsAllHeroDead()
    {
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

    //减少战斗倒计时，加速游戏结束
    public void EndRound()
    {
        timer = combatStageDuration - 3; 
    }

    //重置英雄
    public void ResetHeros()
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

    //计算增加金币函数
    public int CalculateIncome()
    {
        int income = 0;

        //banked gold
        int bank = (int)(currentGold / 10);


        income += baseGoldIncome;
        income += bank;

        return income;
    }

    //玩家受到伤害
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        uI.UpdateUI();

    }

    //计算羁绊
    private void CalculateBonuses()
    {
        //初始化字典
        heroTypeCount = new Dictionary<HeroType, int>();

        //遍历六边形棋盘
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

            //have enough champions to get bonus
            if (m.Value >= heroBonus.heroCount)
            {
                activeBonusList.Add(heroBonus);
            }
        }

    }


    //重启游戏
    public void RestartGame()
    {
        for (int i = 0; i < ownHeroInventoryArray.Length; i++)
        {
            if (ownHeroInventoryArray[i] != null)
            {
                //get character
                HeroController heroController = ownHeroInventoryArray[i].GetComponent<HeroController>();

                Destroy(heroController.gameObject);
                ownHeroInventoryArray[i] = null;
            }

        }

        for (int x = 0; x < MyMap.hexMapSizeX; x++)
        {
            for (int z = 0; z < MyMap.hexMapSizeZ / 2; z++)
            {
                //there is a champion
                if (gridHerosArray[x, z] != null)
                {
                    //get character
                    HeroController heroController = gridHerosArray[x, z].GetComponent<HeroController>();

                    Destroy(heroController.gameObject);
                    gridHerosArray[x, z] = null;
                }

            }
        }

        //reset stats
        currentHP = 100;
        currentGold = 5;
        currentGameStage = GameStage.Preparation;
        currentLevel = 3;
        currentHeroCount = GetHeroCountOnHexGrid();

        uI.UpdateUI();

        //restart ai
        aIOpponent.Restart();

        //show hide ui
        uI.ShowGameScreen();


    }

    public void AddToTimer(float time)
    {
        timer += time;
    }

    public void ResetTimer()
    {
        timer = 0;
    }

    public void UpdateTimerDisplayToNow()
    {
        timerDisplay = (int)timer;
    }

    public void UpdateTimerDisplayToRestTime(float totalTime)
    {
        timerDisplay = (int)(totalTime - timer);
    }

    public bool JudgeTimeUp(float time)
    {
        return timer > time;
    }

    public GameObject GetDraggedHero()
    {
        return draggedHero;
    }
}
