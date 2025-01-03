using UnityEngine;

public class Loss : IGameStage
{
    private GameManager gameManager=GameManager.GetInstance();
    private int duration=99999;

    public GameStage GetName()
    {
        return GameStage.Combat;
    }

    public void Update()
    {
        
    }

    public void OnGameStageComplete()
    {
        
    }
}
