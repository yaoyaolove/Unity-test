// =============================================================================
// 文件名称: HeroAnimation.cs
// 作者: 刘垚
// 创建日期: 2024.11.24
// 更新日期：2024.11.24
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;

public class HeroAnimation : MonoBehaviour
{
    //角色模型是英雄的一个子对象
    private GameObject characterModel;
    private Animator animator;
    private HeroController heroController;

    private Vector3 lastFramePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //获取角色模型
        characterModel = this.transform.Find("character").gameObject;

        //获取动作器
        animator = characterModel.GetComponent<Animator>();
        heroController = this.transform.GetComponent<HeroController>();
    }

    // Update is called once per frame
    void Update()
    {
        //计算移动速度
        float movementSpeed = (this.transform.position - lastFramePosition).magnitude / Time.deltaTime;

        //在动作器上设置移动速度
        animator.SetFloat("movementSpeed", movementSpeed);

        //存储上帧位置
        lastFramePosition = this.transform.position;
    }

    public void DoAttack(bool b)
    {
        animator.SetBool("isAttacking", b);

    }

    public void OnAttackAnimationFinished()
    {
        animator.SetBool("isAttacking", false);

        heroController.OnAttackAnimationFinished();
    }

    //设置动作器状态
    public void IsAnimated(bool b)
    {
        animator.enabled = b;
    }
}
