using UnityEngine;

[CreateAssetMenu(fileName = "Frost Archer", menuName = "Scriptable Objects/Frost Archer")]
public class FrostArcher : Hero
{
    public override void Skill()
    {
        Debug.Log("Frost Archer Skill");
    }
}