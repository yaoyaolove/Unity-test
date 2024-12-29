using UnityEngine;

[CreateAssetMenu(fileName = "Fire Knight", menuName = "Scriptable Objects/Fire Knight")]
public class FireKnight : Hero
{
    public override void Skill()
    {
        Debug.Log("Fire Knight Skill");
    }
}