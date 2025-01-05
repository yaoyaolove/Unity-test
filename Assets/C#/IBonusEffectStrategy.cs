using UnityEngine;

public interface IBonusEffectStrategy
{
    float ApplyOnAttack(HeroController hero, HeroController targetHero, BonusTarget bonusTarget, GameObject effectPrefab, float bonusValue, float duration);
    float ApplyOnGotHit(HeroController hero, float damage, float bonusValue);
}