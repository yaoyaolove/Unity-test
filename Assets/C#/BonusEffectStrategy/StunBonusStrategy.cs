using UnityEngine;

public class StunBonusStrategy : IBonusEffectStrategy
{
    public float ApplyOnAttack(HeroController hero, HeroController targetHero, BonusTarget bonusTarget, GameObject effectPrefab, float bonusValue, float duration)
    {
        int rand = Random.Range(0, 100);
        if (rand < bonusValue)
        {
            targetHero.OnGotStun(duration);
            if (bonusTarget == BonusTarget.Self)
                hero.AddEffect(effectPrefab, duration);
            else if (bonusTarget == BonusTarget.Enemy)
                targetHero.AddEffect(effectPrefab, duration);
        }
        return 0; // 不增加伤害
    }

    public float ApplyOnGotHit(HeroController hero, float damage, float bonusValue)
    {
        return damage; // 不影响受到的伤害
    }
}