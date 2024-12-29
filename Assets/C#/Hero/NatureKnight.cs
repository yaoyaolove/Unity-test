using UnityEngine;

[CreateAssetMenu(fileName = "Nature Knight", menuName = "Scriptable Objects/Nature Knight")]
public class NatureKnight : Hero
{
    public override void Skill()
    {
        Debug.Log("Nature Knight Skill");
    }
}