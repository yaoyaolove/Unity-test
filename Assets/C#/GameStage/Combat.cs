using UnityEngine;
using System.Collections.Generic;

public class Combat : IGameStage
{
    private GameManager gameManager=GameManager.GetInstance();
    private int duration=60;

    public GameStage GetName()
    {
        return GameStage.Combat;
    }

    public void Update()
    {
        //计时器
        gameManager.AddToTimer(Time.deltaTime);

        //显示时间
        gameManager.UpdateTimerDisplayToNow();

        //如果时间到了
        if (gameManager.JudgeTimeUp(duration))
        {
            gameManager.ResetTimer();

            //游戏阶段结束
            gameManager.OnGameStageComplete();
        }
    }

    public void OnGameStageComplete()
    {
        gameManager.ChangeStage(new Preparation());

        EventManager.Publish("ShowTimerText");
        //gameManager.uI.SetTimerTextActive(true);

        gameManager.ResetHeros();

        //尝试对可能升级的英雄进行升级
        for (int i = 0; i < gameManager.gameHeroData.herosArray.Length; i++)
        {
            gameManager.TryUpgradeHero(i);
        }

        //增加加金币
        gameManager.currentGold += gameManager.CalculateIncome();

        //更新UI
        EventManager.Publish("UpdateUI");

        //刷新商店
        gameManager.shop.RefreshShop(true);

        //检查是否失败
        if (gameManager.currentHP <= 0)
        {
            gameManager.ChangeStage(new Loss()); 
            EventManager.Publish("ShowLossScreen");
            //gameManager.uI.ShowLossScreen();
        }
    }
}
