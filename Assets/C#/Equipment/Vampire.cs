using UnityEngine;

public class Vampire : AttackDecorator
{
    public Vampire(IAttackBehavior equipment) : base(equipment)
    {
    }

    public override bool Attack(HeroController target, HeroController source, float damage)
    {
        Debug.Log("Vampire Attack");
        source.OnGotHeal(damage/4);
        return base.Attack(target, source, damage);
        
    }
}