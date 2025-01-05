using UnityEngine;
using static GameManager;

public class Preparation : IGameStage
{
    private GameManager gameManager = GameManager.GetInstance();
    private int duration = 15;

    public GameStage GetName()
    {
        return GameStage.Preparation;
    }

    public void Update()
    {
        gameManager.AddToTimer(Time.deltaTime);

        gameManager.UpdateTimerDisplayToRestTime(duration);

        EventManager.Publish("UpdateTimerText");

        if (gameManager.JudgeTimeUp(duration))
        {
            gameManager.ResetTimer();
            gameManager.OnGameStageComplete();
        }
    }

    public void OnGameStageComplete()
    {
        gameManager.ChangeStage(new Combat());

        //将指示器均隐藏，这是为了防止你在拖拽英雄的过程中准备阶段结束
        gameManager.map.HideIndicators();

        //计时器隐藏
        EventManager.Publish("HideTimerText");
        //gameManager.uI.SetTimerTextActive(false);

        GameObject draggedHero = gameManager.GetDraggedHero();
        if (draggedHero != null)
        {
            //这里要记住将英雄拖拽信息更新    
            draggedHero.GetComponent<HeroController>().IsDragged = false;
            draggedHero = null;
        }

        //备战席
        for (int i = 0; i < gameManager.ownHeroInventoryArray.Length; i++)
        {
            if (gameManager.ownHeroInventoryArray[i] != null)
            {
                HeroController heroController = gameManager.ownHeroInventoryArray[i].GetComponent<HeroController>();

                heroController.OnCombatStart();
            }
        }

        //六边形棋盘
        GridHeroIterator gridHeroIterator = gameManager.gridHeroIterator;
        gridHeroIterator.Reset();
        GameObject gridHero = null;
        while (gridHero = gridHeroIterator.GetNext())
        {
            HeroController heroController = gridHero.GetComponent<HeroController>();

            heroController.OnCombatStart();
        }


        //检查是否没有英雄，会直接判负，回合结束
        if (gameManager.IsAllHeroDead())
            gameManager.EndRound();
    }
}
