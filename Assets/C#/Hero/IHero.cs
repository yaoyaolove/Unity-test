// =============================================================================
// 文件名称: IHero.cs
// 作者: 刘垚
// 创建日期: 2024.11.18
// 更新日期：2024.11.18
// 使用的设计模式：工厂方法模式
// 备注：英雄基类接口
// =============================================================================
using UnityEngine;

public interface IHero
{
    Vector3 Position { get; set; }
    int MaxHealth {  get; set; }
    int Health { get; set; }
    void TakeDamage(int damage);
}
