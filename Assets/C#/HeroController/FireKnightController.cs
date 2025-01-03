using UnityEngine;

public class FireKnightController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[1];
    }
}