using UnityEngine;

public class Killer : AttackDecorator
{
    public Killer(IAttackBehavior equipment) : base(equipment)
    {
    }

    public override bool Attack(HeroController target, HeroController source, float damage)
    {
        Debug.Log("Killer Attack");
        return base.Attack(target, source, damage+5);
        
    }
}