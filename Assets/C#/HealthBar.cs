// =============================================================================
// 文件名称: HealthBar.cs
// 作者: 刘垚
// 创建日期: 2024.11.26
// 更新日期：2024.11.26
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameObject heroGO;
    private HeroController heroController;
    public Image fillImage;

    private CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (heroGO != null)
        {
            this.transform.position = heroGO.transform.position + new Vector3(0, 1.5f + 1.5f * heroGO.transform.localScale.x, 0);
            fillImage.fillAmount = heroController.currentHealth / heroController.maxHealth;

            if (heroController.currentHealth <= 0)
                canvasGroup.alpha = 0;
            else
                canvasGroup.alpha = 1;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(GameObject _heroGO)
    {
        heroGO = _heroGO;
        heroController = heroGO.GetComponent<HeroController>();
    }
}
