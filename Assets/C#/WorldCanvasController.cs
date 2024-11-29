// =============================================================================
// 文件名称: WorldCanvasController.cs
// 作者: 刘垚
// 创建日期: 2024.11.26
// 更新日期：2024.11.26
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;

public class WorldCanvasController : MonoBehaviour
{
    public GameObject worldCanvas;
    public GameObject floatingTextPrefab;
    public GameObject healthBarPrefab;

    //添加伤害文本
    public void AddDamageText(Vector3 position, float v)
    {
        GameObject go = Instantiate(floatingTextPrefab);
        go.transform.SetParent(worldCanvas.transform);

        go.GetComponent<FloatingText>().Init(position, v);
    }

    //添加血条
    public void AddHealthBar(GameObject championGO)
    {
        GameObject go = Instantiate(healthBarPrefab);
        go.transform.SetParent(worldCanvas.transform);

        go.GetComponent<HealthBar>().Init(championGO);
    }
}
