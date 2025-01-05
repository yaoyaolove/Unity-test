public class HealBonusStrategy : IBonusEffectStrategy
{
    public float ApplyOnAttack(HeroController hero, HeroController targetHero, float bonusValue, float duration)
    {
        hero.OnGotHeal(bonusValue);
        hero.AddEffect(hero.effectPrefab, duration);
        return 0; // 不增加伤害
    }

    public float ApplyOnGotHit(HeroController hero, float damage, float bonusValue)
    {
        return damage; // 不影响受到的伤害
    }
}