using UnityEngine;

public class NatureKnightController : HeroController
{
    public override void CreateHero()
    {  
        hero = GameManager.GetInstance().gameHeroData.herosArray[7];
    }
}