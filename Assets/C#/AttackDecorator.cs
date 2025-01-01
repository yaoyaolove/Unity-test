using UnityEngine;

public abstract class AttackDecorator : IAttackBehavior
{
    protected IAttackBehavior equipment;

    public AttackDecorator(IAttackBehavior equipment)
    {
        this.equipment = equipment;
    }
    public virtual bool Attack(HeroController target, HeroController source, float damage)
    {
        return equipment.Attack(target, source, damage);
    }
}
