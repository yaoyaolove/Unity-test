public class DamageBonusStrategy : IBonusEffectStrategy
{
    public float ApplyOnAttack(HeroController hero, HeroController targetHero, float bonusValue, float duration)
    {
        return bonusValue; // 增加的伤害
    }

    public float ApplyOnGotHit(HeroController hero, float damage, float bonusValue)
    {
        return damage; // 不影响受到的伤害
    }
}