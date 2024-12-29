using UnityEngine;

[CreateAssetMenu(fileName = "Fire Wizard", menuName = "Scriptable Objects/Fire Wizard")]
public class FireWizard : Hero
{
    public override void Skill()
    {
        Debug.Log("Fire Wizard Skill");
    }
}