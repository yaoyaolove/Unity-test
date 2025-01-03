using UnityEngine;

public class FrostWizardController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[5];
    }
}