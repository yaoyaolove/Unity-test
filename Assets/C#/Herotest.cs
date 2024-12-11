// =============================================================================
// 文件名称: Herotest.cs
// 作者: 王骏禹
// 创建日期: 2024.12.11
// 更新日期：2024.12.11
// 使用的设计模式：
// 备注：12.11update 用来测试装备是否能够增加数值 使用方法 保存后点击unitytest最上面一栏的tools的 run hero test即可
// =============================================================================
using UnityEditor;
using UnityEngine;

public class HeroTestEditor : EditorWindow
{
    [MenuItem("Tools/Run Hero Test")]
    public static void RunHeroTest()
    {
        Hero hero = ScriptableObject.CreateInstance<Hero>();

        // 初始化 Hero 属性
        hero.prefab = new GameObject("HeroPrefab");  // 创建一个新的 GameObject 来作为 Prefab
        hero.uIName = "Warrior";
        hero.cost = 100;
        hero.health = 500f;
        hero.damage = 50f;
        hero.attackRange = 5f;

        // 输出初始化后的 Hero 属性
        Debug.Log($"Hero Name: {hero.UIName}, Damage: {hero.Damage}");

        // 假设你想增加的额外伤害值
        float additionalDamage = 0f;

        // 创建 DamageBoostDecorator 装饰器
        var damageBoostDecorator = new DamageBoostDecorator(hero, additionalDamage);

        // 打印原始和增强后的伤害值
        Debug.Log("Original Damage: " + hero.Damage); // 输出原始的英雄伤害
        Debug.Log("Boosted Damage: " + damageBoostDecorator.Damage); // 输出加成后的伤害
    }
}
