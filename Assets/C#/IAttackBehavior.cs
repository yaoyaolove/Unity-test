using UnityEngine;

public interface IAttackBehavior
{
    public bool Attack(HeroController target, HeroController source, float damage);
}