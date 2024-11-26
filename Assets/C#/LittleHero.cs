// =============================================================================
// 文件名称: LittleHero.cs
// 作者: 刘垚
// 创建日期: 2024.11.17
// 更新日期：2024.11.17
// 使用的设计模式：
// 备注：还未添加设计模式，后续还需调整。
// =============================================================================
using UnityEngine;

public class LittleHero : MonoBehaviour
{
    //用于射线检测的平面
    public Plane m_Plane;

    //小小英雄的移动速度
    public float speed = 5.0f;

    //小小英雄的目标位置
    private Vector3 targetPosition;

    //是否有目标位置
    private bool hasTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Plane = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //处理鼠标点击事件
        if(Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
        //移动小小英雄
        MoveToTarget();*/
    }

    void HandleMouseClick()
    {
        // 发射射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 使用平面计算目标位置
        float enter;
        if (m_Plane.Raycast(ray, out enter))
        {
            Vector3 planeHitPoint = ray.GetPoint(enter);
            float currentY = transform.position.y;

            // 设置目标位置
            targetPosition = new Vector3(planeHitPoint.x, currentY, planeHitPoint.z);
            hasTarget = true;
        }
    }

    void MoveToTarget()
    {
        if (hasTarget)
        {
            // 计算移动方向
            Vector3 direction = (targetPosition - transform.position).normalized;

            // 移动英雄
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // 检查是否到达目标位置
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                hasTarget = false;
            }
        }
    }
}
