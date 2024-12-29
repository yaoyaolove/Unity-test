using UnityEngine;

[CreateAssetMenu(fileName = "Nature Archer", menuName = "Scriptable Objects/Nature Archer")]
public class NatureArcher : Hero
{
    public override void Skill()
    {
        Debug.Log("Nature Archer Skill");
    }
}