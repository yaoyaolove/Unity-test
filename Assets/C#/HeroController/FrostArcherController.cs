using UnityEngine;

public class FrostArcherController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[3];
    }
}