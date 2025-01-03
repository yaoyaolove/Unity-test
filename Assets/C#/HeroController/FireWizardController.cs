using UnityEngine;

public class FireWizardController : HeroController
{
    public override void CreateHero()
    {
        hero = GameManager.GetInstance().gameHeroData.herosArray[2];
    }
}