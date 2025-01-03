using UnityEngine;
using System.Collections.Generic;

public interface IGameStage
{
    public GameStage GetName();

    public void Update();

    public void OnGameStageComplete();

    

}
