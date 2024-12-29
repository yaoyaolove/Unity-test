using UnityEngine;

[CreateAssetMenu(fileName = "Frost Knight", menuName = "Scriptable Objects/Nature Wizard")]
public class NatureWizard : Hero
{
    public override void Skill()
    {
        Debug.Log("Nature Wizard Skill");
    }
}