// =============================================================================
// 文件名称: Warrior.cs
// 作者: 刘垚
// 创建日期: 2024.11.18
// 更新日期：2024.11.18
// 使用的设计模式：工厂方法模式
// 备注：英雄类（实体类）
// =============================================================================
using UnityEngine;

public class Warrior : MonoBehaviour,IHero
{
    //英雄类的一些常量
    private Vector3 position;
    private int maxHealth = 150;
    private int health;

    public Vector3 Position { get => position; set => position = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Health { get => health; set => health = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
