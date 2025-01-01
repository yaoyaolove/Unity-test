// =============================================================================
// 文件名称: FloatingText.cs
// 作者: 刘垚
// 创建日期: 2024.11.26
// 更新日期：2024.11.26
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FloatingText : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    //移动方向
    private Vector3 moveDirection;
    //一个计时器
    private float timer = 0;

    //字的移动速度
    public float speed = 3;

    //字消失时间
    public float fadeOutTime = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //模拟字移动的效果
        this.transform.position = this.transform.position + moveDirection * speed * Time.deltaTime;

        timer += Time.deltaTime;
        float fade = (fadeOutTime - timer) / fadeOutTime;

        canvasGroup.alpha = fade;

        if (fade <= 0)
            Destroy(this.gameObject);
    }

    public void Init(Vector3 startPosition, float v, Color color)
    {
        this.transform.position = startPosition;

        canvasGroup = this.GetComponent<CanvasGroup>();

        this.GetComponent<Text>().text = Mathf.Round(v).ToString();
        this.GetComponent<Text>().color = color;

        moveDirection = new Vector3(Random.Range(-0.5f, 0.5f), 1, Random.Range(-0.5f, 0.5f)).normalized;
    }
}
