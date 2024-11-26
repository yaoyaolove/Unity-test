// =============================================================================
// 文件名称: HeroFactoryManager.cs
// 作者: 刘垚
// 创建日期: 2024.11.18
// 更新日期：2024.11.18
// 使用的设计模式：无
// 备注：英雄工厂管理类，这样做是为了避免在商店类中管理各种英雄工厂
// =============================================================================
using System;
using System.Collections.Generic;
using UnityEngine;

public class HeroFactoryManager
{
    private Dictionary<string, HeroFactory> factories;

    public HeroFactoryManager()
    {
        factories = new Dictionary<string, HeroFactory>()
        {
            { "warrior",new WarriorFactory()}
        };
    }

    public IHero CreateHero(string type, Vector3 position)
    {
        if (factories.ContainsKey(type))
        {
            return factories[type].CreateHero(position);
        }
        else
        {
            throw new ArgumentException("Invalid hero type");
        }
    }
}
