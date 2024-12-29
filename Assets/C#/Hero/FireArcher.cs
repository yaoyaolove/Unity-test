using UnityEngine;

[CreateAssetMenu(fileName = "Fire Archer", menuName = "Scriptable Objects/Fire Archer")]
public class FireArcher : Hero
{
    public override void Skill()
    {
        Debug.Log("Fire Archer Skill");
    }
}