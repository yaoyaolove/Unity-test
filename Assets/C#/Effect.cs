// =============================================================================
// 文件名称: Effect.cs
// 作者: 刘垚
// 创建日期: 2024.11.28
// 更新日期：2024.11.28
// 使用的设计模式：
// 备注：
// =============================================================================
using UnityEngine;

public class Effect : MonoBehaviour
{
    public GameObject effectPrefab;

    public float duration;
    private GameObject heroGO;
    private GameObject effectGO;
    

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;

        if (duration < 0)
            heroGO.GetComponent<HeroController>().RemoveEffect(this);
    }

    public void Init(GameObject _effectPrefab, GameObject _championGO, float _duration)
    {
        effectPrefab = _effectPrefab;
        duration = _duration;
        heroGO = _championGO;

        effectGO = Instantiate(effectPrefab);
        effectGO.transform.SetParent(heroGO.transform);
        effectGO.transform.localPosition = Vector3.zero;
    }

    public void Remove()
    {
        Destroy(effectGO);
        Destroy(this);
    }
}
