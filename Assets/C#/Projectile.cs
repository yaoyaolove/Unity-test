// =============================================================================
// 文件名称: HeroController.cs
// 作者: 刘垚
// 创建日期: 2024.11.28
// 更新日期：2024.11.28
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //投射物的移动速度
    public float speed;

    //投射物的消失时间
    public float duration;

    private GameObject target;

    private bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init(GameObject _target)
    {
        target = _target;

        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isMoving)
        {
            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            //计算相对位置
            Vector3 relativePos = target.transform.position - transform.position;

            //创建一个旋转，使得z轴指向relativePos方向，并且y轴尽可能地与Vector3.up对齐
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            this.transform.rotation = rotation;
            //加上一个向上的偏移量
            Vector3 targetPosition = target.transform.position + new Vector3(0, 1, 0);

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);

            float distance = Vector3.Distance(this.transform.position, targetPosition);

            if (distance < 0.2f)
            {
                //将当前对象的父级设置为目标对象
                this.transform.parent = target.transform;

                Destroy(this.gameObject, duration);
            }
        }
    }
}
