using UnityEngine;

[CreateAssetMenu(fileName = "HeroType", menuName = "Scriptable Objects/HeroType")]
public class HeroType : ScriptableObject
{
    //在UI上显示的名字
    public string displayName = "name";

    //在UI上显示的图标
    public Sprite icon;

    //英雄类拥有的羁绊效果
    public HeroBonus heroBonus;
}
