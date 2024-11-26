// =============================================================================
// 文件名称: WarriorFactory.cs
// 作者: 刘垚
// 创建日期: 2024.11.18
// 更新日期：2024.11.18
// 使用的设计模式：工厂方法模式
// 备注：战士工厂类（具体工厂类）
// =============================================================================
using UnityEngine;
using UnityEngine.UIElements;

public class WarriorFactory:HeroFactory
{
    public override IHero CreateHero(Vector3 position)
    {
        GameObject heroObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        heroObject.transform.position = position;

        Warrior warrior=heroObject.AddComponent<Warrior>();

        return warrior;
    }
}
