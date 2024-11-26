using UnityEngine;

[CreateAssetMenu(fileName = "Hero", menuName = "Scriptable Objects/Hero")]
public class Hero : ScriptableObject
{
    //在游戏中创建的预制体
    public GameObject prefab;

    //角色发射的投掷物
    public GameObject attackProjectile;

    //UI界面上显示的名字
    public string uIName;

    //价格
    public int cost;

    //英雄类型1
    public HeroType type1;

    //英雄类型2
    public HeroType type2;

    //英雄血量
    public float health;

    //英雄伤害
    public float damage;

    //英雄攻击范围
    public float attackRange;
}
