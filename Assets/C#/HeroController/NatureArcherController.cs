using UnityEngine;

public class NatureArcherController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[6];
    }
}