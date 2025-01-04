// =============================================================================
// 文件名称: Hero.cs
// 作者: 刘垚、王骏禹
// 创建日期: 2024.11.18
// 更新日期：2024.12.11
// 使用的设计模式：
// 备注：12.11update 加入了IHero接口
// =============================================================================
using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Scriptable Objects/Hero")]
public abstract class Hero : ScriptableObject
{
    // 英雄发射的攻击投掷物
    public GameObject attackProjectile;

    // UI界面上显示的英雄名称
    public string uIName;

    // 英雄的价格，通常用于购买或解锁该英雄
    public int cost;

    // 英雄的第一种类型，用于分类
    public HeroType type1;

    // 英雄的第二种类型（可选），可以用于更细致的分类
    public HeroType type2;

    // 英雄的血量，决定其在游戏中的存活能力
    public float health;

    // 英雄的攻击力，决定其造成的伤害
    public float damage;

    // 英雄的攻击范围，决定其能够攻击的最大距离
    public float attackRange;

    public GameObject AttackProjectile
    {
        get => attackProjectile;
        set => attackProjectile = value;
    }
    // 获取英雄的UI名称
    public string UIName => uIName;

    // 获取英雄的价格
    public int Cost => cost;

    // 获取英雄的第一种类型
    public HeroType Type1 => type1;

    // 获取英雄的第二种类型
    public HeroType Type2 => type2;

    // 获取英雄的血量
    public float Health => health;

    // 获取英雄的攻击力
    public float Damage => damage;

    // 获取英雄的攻击范围
    public float AttackRange => attackRange;

    public abstract void Skill();
}

