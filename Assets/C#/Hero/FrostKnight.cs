using UnityEngine;

[CreateAssetMenu(fileName = "Frost Knight", menuName = "Scriptable Objects/Frost Knight")]
public class FrostKnight: Hero
{
    public override void Skill()
    {
        Debug.Log("Frost Knight Skill");
    }
}