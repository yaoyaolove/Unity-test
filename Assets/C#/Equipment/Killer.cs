using UnityEngine;

public class Killer : AttackDecorator
{
    public Killer(IAttackBehavior equipment) : base(equipment)
    {
    }

    public override bool Attack(HeroController target, HeroController source, float damage)
    {
        return base.Attack(target, source, damage)|| base.Attack(target, source, 5);
        
    }
}