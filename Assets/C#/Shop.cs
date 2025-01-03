// =============================================================================
// 文件名称: Shop.cs
// 作者: 刘垚
// 创建日期: 2024.11.18
// 更新日期：2024.11.24
// 使用的设计模式：外观模式
// 备注：商店UI类，目前还在调试阶段，鼠标右键生成warrior。Store为客户端提供了一个复杂子系统的简单接口，只需要调用该接口便可实现英雄的创建。
// =============================================================================
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Shop : MonoBehaviour
{
    public UI uI;

    //存储能购买的英雄列表
    private int[] availableHeroArray;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshShop(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //刷新商店
    public void RefreshShop(bool isFree)
    {
        if (GameManager.Instance.currentGold < 2 && isFree == false)
            return;

        //初始化数组
        availableHeroArray = new int[5];

        for (int i = 0; i < availableHeroArray.Length; i++)
        {
            int heroIndex = GetRandomHeroIndex();

            availableHeroArray[i] = heroIndex;

            uI.LoadShopItem(heroIndex, i);

            uI.ShowHeroFrames();
        }

        if (isFree == false)
            GameManager.Instance.currentGold -= 2;

        uI.UpdateUI();
    }

    //购买英雄
    public void BuyHero(int index)
    {
        int heroIndex = availableHeroArray[index];
        bool isSuccess = GameManager.Instance.BuyHeroFromShop(heroIndex);
        if (isSuccess)
        {
            uI.HideHeroFrame(index);
        }
    }

    //购买经验升级UI
    public void BuyLevel()
    {
        GameManager.Instance.BuyLevelFromShop();
    }

    public int GetRandomHeroIndex()
    {
        return Random.Range(0, GameManager.Instance.HeroCounts);
    }
}
