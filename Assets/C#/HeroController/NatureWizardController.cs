using UnityEngine;

public class NatureWizardController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[8];
    }
}