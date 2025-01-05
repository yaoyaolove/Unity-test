public class DefenseBonusStrategy : IBonusEffectStrategy
{
    public float ApplyOnAttack(HeroController hero, HeroController targetHero, float bonusValue, float duration)
    {
        return 0; // 不增加伤害
    }

    public float ApplyOnGotHit(HeroController hero, float damage, float bonusValue)
    {
        return ((100 - bonusValue) / 100) * damage; // 减少受到的伤害
    }
}