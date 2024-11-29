// =============================================================================
// 文件名称: HeroBonus.cs
// 作者: 刘垚
// 创建日期: 2024.11.28
// 更新日期：2024.11.28
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;

public enum HeroBonusType { Damage, Defense, Stun, Heal };
public enum BonusTarget { Self, Enemy };

[System.Serializable]
public class HeroBonus
{
    //激活羁绊需要的人数
    public int heroCount = 0;

    //羁绊效果的类型
    public HeroBonusType heroBonusType;

    //效果的作用对象
    public BonusTarget bonusTarget;

    //效果数值
    public float bonusValue = 0;

    //效果持续时间
    public float duration;

    public GameObject effectPrefab;

    //攻击时计算羁绊效果
    public float ApplyOnAttack(HeroController hero, HeroController targetHero)
    {

        float bonusDamage = 0;
        bool addEffect = false;
        switch (heroBonusType)
        {
            case HeroBonusType.Damage:
                bonusDamage += bonusValue;
                break;
            case HeroBonusType.Stun:
                int rand = Random.Range(0, 100);
                if (rand < bonusValue)
                {
                    targetHero.OnGotStun(duration);
                    addEffect = true;
                }
                break;
            case HeroBonusType.Heal:
                hero.OnGotHeal(bonusValue);
                addEffect = true;
                break;
            default:
                break;
        }

        if (addEffect)
        {
            if (bonusTarget == BonusTarget.Self)
                hero.AddEffect(effectPrefab, duration);
            else if (bonusTarget == BonusTarget.Enemy)
                targetHero.AddEffect(effectPrefab, duration);
        }


        return bonusDamage;
    }

    //受攻击时计算羁绊效果
    public float ApplyOnGotHit(HeroController hero, float damage)
    {
        switch (heroBonusType)
        {
            case HeroBonusType.Defense:
                damage = ((100 - bonusValue) / 100) * damage;
                break;
            default:
                break;
        }

        return damage;
    }
}
