using UnityEngine;

public class FrostKnightController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[4];
    }
}