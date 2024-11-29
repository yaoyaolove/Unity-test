using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.transform.parent.GetComponent<HeroAnimation>().OnAttackAnimationFinished();
    }
}

