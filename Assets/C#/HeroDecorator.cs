// =============================================================================
// 文件名称: IHeroDecorator.cs
// 作者: 王骏禹
// 创建日期: 2024.12.11
// 更新日期：2024.12.11
// 使用的设计模式：装饰模式
// 备注：12.11update 装备
// =============================================================================
using UnityEngine;

// Hero 装饰器基类
public abstract class HeroDecorator : InterHero
{
	protected InterHero _hero;

	// 构造函数，接收一个 InterHero 实例
	public HeroDecorator(InterHero hero)
	{
		_hero = hero;
	}

	// 重写 InterHero 接口的方法，这些方法实际上会调用 _hero 的相应方法
	public virtual GameObject Prefab => _hero.Prefab;
	public virtual string UIName => _hero.UIName;
	public virtual int Cost => _hero.Cost;
	public virtual HeroType Type1 => _hero.Type1;
	public virtual HeroType Type2 => _hero.Type2;
	public virtual float Health => _hero.Health;
	public virtual float Damage => _hero.Damage;
	public virtual float AttackRange => _hero.AttackRange;
}

// DamageBoostDecorator 类，增加英雄伤害的装饰器
public class DamageBoostDecorator : HeroDecorator
{
	private float _additionalDamage;  
	public DamageBoostDecorator(InterHero hero, float additionalDamage) : base(hero)
	{
		_additionalDamage = additionalDamage;
	}
	public override float Damage => base.Damage + _additionalDamage;
}

// AttackRangeBoostDecorator 类，增加英雄攻击范围的装饰器
public class AttackRangeBoostDecorator : HeroDecorator
{
	private float _additionalRange; 
	public AttackRangeBoostDecorator(InterHero hero, float additionalRange) : base(hero)
	{
		_additionalRange = additionalRange;
	}
	public override float AttackRange => base.AttackRange + _additionalRange;
}

// HealthBoostDecorator 类，增加英雄血量的装饰器
public class HealthBoostDecorator : HeroDecorator
{
	private float _additionalHealth; 
	public HealthBoostDecorator(InterHero hero, float additionalHealth) : base(hero)
	{
		_additionalHealth = additionalHealth;
	}
	public override float Health => base.Health + _additionalHealth;
}

