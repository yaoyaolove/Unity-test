using UnityEngine;

[CreateAssetMenu(fileName = "Frost Wizard", menuName = "Scriptable Objects/Frost Wizard")]
public class FrostWizard : Hero
{
    public override void Skill()
    {
        Debug.Log("Frost Wizard Skill");
    }
}