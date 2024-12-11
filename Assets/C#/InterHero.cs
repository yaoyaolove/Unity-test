// =============================================================================
// 文件名称: InterHero.cs
// 作者: 王骏禹
// 创建日期: 2024.12.11
// 更新日期：2024.12.11
// 使用的设计模式：装饰模式
// 备注：12.11update 创建InterHero接口
// =============================================================================
using UnityEngine;
public interface InterHero
{
    GameObject Prefab { get; }  // 预制体属性，供外部访问
    string UIName { get; }      // 英雄的 UI 名称
    int Cost { get; }           // 英雄的成本
    HeroType Type1 { get; }     // 英雄的第一个类型（如战士、法师等）
    HeroType Type2 { get; }     // 英雄的第二个类型（如攻击型、防御型等）
    float Health { get; }       // 英雄的生命值
    float Damage { get; }       // 英雄的伤害值
    float AttackRange { get; }  // 英雄的攻击范围
}
