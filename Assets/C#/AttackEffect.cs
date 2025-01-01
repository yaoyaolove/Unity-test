using UnityEngine;

public class AttackEffect : IAttackBehavior
{
    public virtual bool Attack(HeroController target, HeroController source, float damage)
    {
        return target.OnGotHit(damage);
    }
}
