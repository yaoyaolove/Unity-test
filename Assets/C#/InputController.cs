// =============================================================================
// 文件名称: InputController.cs
// 作者: 刘垚
// 创建日期: 2024.11.24
// 更新日期：2024.11.24
// 使用的设计模式：
// 备注：控制游戏中的输入
// =============================================================================
using UnityEngine;

public class InputController : MonoBehaviour
{

    public MyMap map;

    public LayerMask triggerLayer;

    private Vector3 rayCastStartPosition;

    //储存鼠标位置
    private Vector3 mousePosition;

    //触发器信息
    [HideInInspector]
    public TriggerInfo triggerInfo = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //初始化射线的起始位置
        rayCastStartPosition = new Vector3(0, 20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        triggerInfo = null;
        //重置所有指示器的颜色
        map.resetIndicators();

        //记录碰撞的详细信息
        RaycastHit hit;

        //将鼠标在屏幕上的位置转换为一条从摄像机出发的射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //如果射线击中了某个东西
        if (Physics.Raycast(ray, out hit, 100f, triggerLayer, QueryTriggerInteraction.Collide))
        {
            //得到被击中的游戏对象的TriggerInfo组件
            triggerInfo = hit.collider.gameObject.GetComponent<TriggerInfo>();

            if (triggerInfo != null)
            {
                //获取其指示器
                GameObject indicator = map.GetIndicatorFromTriggerInfo(triggerInfo);

                //将该指示器的颜色激活
                indicator.GetComponent<MeshRenderer>().material.color = map.indicatorActiveColor;
            }
            else
                map.resetIndicators(); 
        }


        if (Input.GetMouseButtonDown(0))
        {
            GameManager.GetInstance().StartDrag();
        }

        if (Input.GetMouseButtonUp(0))
        {
            GameManager.GetInstance().StopDrag();
        }

        mousePosition = Input.mousePosition;
    }
}
