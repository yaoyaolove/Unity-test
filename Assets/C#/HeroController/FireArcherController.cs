using UnityEngine;

public class FireArcherController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[0];
    }
}