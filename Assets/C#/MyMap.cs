// =============================================================================
// 文件名称: MyMap.cs
// 作者: 刘垚
// 创建日期: 2024.11.19
// 更新日期：2024.11.19
// 使用的设计模式：
// 备注：还未添加设计模式，后续还需调整。该类主要是处理网格。有一些冗余，测试成功后边进行删减工作。
// =============================================================================
using UnityEngine;

public class MyMap : MonoBehaviour
{
    //声明网格类型，分别为玩家库存网格、对手库存网格、六边形地图网格
    public static int GRIDTYPE_OWN_INVENTORY = 0;
    public static int GRIDTYPE_OPONENT_INVENTORY = 1;
    public static int GRIDTYPE_HEXA_MAP = 2;

    //六边形网格x轴方向有7个，z轴方向有8个，库存网格有9个
    public static int hexMapSizeX = 7;
    public static int hexMapSizeZ = 8;
    public static int inventorySize = 9;

    //用于射线检测的平面
    public Plane m_Plane;

    //各网格的起始位置，额，这个应该都没用到，后面测试可以直接删掉
    public Transform ownInventoryStartPosition;
    public Transform oponentInventoryStartPosition;
    public Transform mapStartPosition;

    //方格指示器和六边形指示器的预制体
    public GameObject squareIndicator;
    public GameObject hexaIndicator;

    //定义了两种指示器的颜色
    public Color indicatorDefaultColor;
    public Color indicatorActiveColor;

    //声明存储网格位置的数组
    [HideInInspector]
    public Vector3[] ownInventoryGridPositions;
    [HideInInspector]
    public Vector3[] oponentInventoryGridPositions;
    [HideInInspector]
    public Vector3[,] mapGridPositions;

    //声明存储网格指示器对象的数组
    [HideInInspector]
    public GameObject[] ownIndicatorArray;
    [HideInInspector]
    public GameObject[] oponentIndicatorArray;
    [HideInInspector]
    public GameObject[,] mapIndicatorArray;

    //声明存储触发器信息的数组
    [HideInInspector]
    public TriggerInfo[] ownTriggerArray;
    [HideInInspector]
    public TriggerInfo[,] mapGridTriggerArray;

    //声明存储所有指示器对象的容器对象
    private GameObject indicatorContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateGridPosition();
        CreateIndicators();
        HideIndicators();

        m_Plane = new Plane(Vector3.up, Vector3.zero);

        this.SendMessage("OnMapReady", SendMessageOptions.DontRequireReceiver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化网格位置
    private void CreateGridPosition()
    {
        //分配内存
        ownInventoryGridPositions = new Vector3[inventorySize];
        oponentInventoryGridPositions = new Vector3[inventorySize];
        mapGridPositions = new Vector3[hexMapSizeX, hexMapSizeZ];


        //创建玩家库存网格的位置
        for (int i = 0; i < inventorySize; i++)
        {
            //calculate position x offset for this slot
            float offsetX = i * -2.5f;

            //calculate and store the position
            Vector3 position = GetMapHitPoint(ownInventoryStartPosition.position + new Vector3(offsetX, 0, 0));

            //add position variable to array
            ownInventoryGridPositions[i] = position;
        }

        //创建对手库存网格的位置
        for (int i = 0; i < inventorySize; i++)
        {
            //calculate position x offset for this slot
            float offsetX = i * -2.5f;

            //calculate and store the position
            Vector3 position = GetMapHitPoint(oponentInventoryStartPosition.position + new Vector3(offsetX, 0, 0));

            //add position variable to array
            oponentInventoryGridPositions[i] = position;
        }

        //创建六边形网格的位置
        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ; z++)
            {
                //calculate even or add row
                int rowOffset = z % 2;

                //calculate position x and z
                float offsetX = x * -3f + rowOffset * 1.5f;
                float offsetZ = z * -2.5f;

                //calculate and store the position
                Vector3 position = GetMapHitPoint(mapStartPosition.position + new Vector3(offsetX, 0, offsetZ));

                //add position variable to array
                mapGridPositions[x, z] = position;
            }

        }

    }

    //初始化网格指示器
    private void CreateIndicators()
    {
        //分配内存
        indicatorContainer = new GameObject();
        indicatorContainer.name = "IndicatorContainer";

        //创建了一个存储触发器的容器
        GameObject triggerContainer = new GameObject();
        triggerContainer.name = "TriggerContainer";


        //分配内存
        ownIndicatorArray = new GameObject[inventorySize];
        oponentIndicatorArray = new GameObject[inventorySize];
        mapIndicatorArray = new GameObject[hexMapSizeX, hexMapSizeZ / 2];

        ownTriggerArray = new TriggerInfo[inventorySize];
        mapGridTriggerArray = new TriggerInfo[hexMapSizeX, hexMapSizeZ / 2];


        //迭代玩家库存网格的位置
        for (int i = 0; i < inventorySize; i++)
        {
            //实例化一个游戏对象
            GameObject indicatorGO = Instantiate(squareIndicator);

            //设置位置
            indicatorGO.transform.position = ownInventoryGridPositions[i];

            //设置指示器的父对象
            indicatorGO.transform.parent = indicatorContainer.transform;

            //将指示器对象存入数组
            ownIndicatorArray[i] = indicatorGO;

            //创建触发器对象
            GameObject trigger = CreateBoxTrigger(GRIDTYPE_OWN_INVENTORY, i);

            //设置触发器的父对象
            trigger.transform.parent = triggerContainer.transform;

            //设置触发器对象的位置
            trigger.transform.position = ownInventoryGridPositions[i];

            //存储触发器的信息
            ownTriggerArray[i] = trigger.GetComponent<TriggerInfo>();
        }

        //iterate map grid position
        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ / 2; z++)
            {
                //实例化
                GameObject indicatorGO = Instantiate(hexaIndicator);

                //设置位置
                indicatorGO.transform.position = mapGridPositions[x, z];

                //设置指示器的父对象
                indicatorGO.transform.parent = indicatorContainer.transform;

                //将指示器对象存入数组
                mapIndicatorArray[x, z] = indicatorGO;

                //创建触发器对象
                GameObject trigger = CreateSphereTrigger(GRIDTYPE_HEXA_MAP, x, z);

                //设定父对象
                trigger.transform.parent = triggerContainer.transform;

                //设置触发器对象的位置
                trigger.transform.position = mapGridPositions[x, z];

                //存储触发器信息
                mapGridTriggerArray[x, z] = trigger.GetComponent<TriggerInfo>();

            }
        }

    }

    //用于得到地面的实际坐标
    public Vector3 GetMapHitPoint(Vector3 p)
    {
        Vector3 newPos = p;

        RaycastHit hit;

        if (Physics.Raycast(newPos + new Vector3(0, 10, 0), Vector3.down, out hit, 15))
        {
            newPos = hit.point;
        }

        return newPos;
    }

    //创建盒形触发器，用于库存网格
    private GameObject CreateBoxTrigger(int type, int x)
    {
        //create primitive gameobject
        GameObject trigger = new GameObject();

        //add collider component
        BoxCollider collider = trigger.AddComponent<BoxCollider>();

        //set collider size
        collider.size = new Vector3(2, 0.5f, 2);

        //set collider to trigger 
        collider.isTrigger = true;

        //add and store trigger info
        TriggerInfo trigerInfo = trigger.AddComponent<TriggerInfo>();
        trigerInfo.gridType = type;
        trigerInfo.gridX = x;

        trigger.layer = LayerMask.NameToLayer("Triggers");

        return trigger;
    }

    //创建球形触发器，用于六边形地图网格
    private GameObject CreateSphereTrigger(int type, int x, int z)
    {
        //create primitive gameobject
        GameObject trigger = new GameObject();

        //add collider component
        SphereCollider collider = trigger.AddComponent<SphereCollider>();

        //set collider size
        collider.radius = 1.4f;

        //set collider to trigger 
        collider.isTrigger = true;

        //add and store trigger info
        TriggerInfo trigerInfo = trigger.AddComponent<TriggerInfo>();
        trigerInfo.gridType = type;
        trigerInfo.gridX = x;
        trigerInfo.gridZ = z;

        trigger.layer = LayerMask.NameToLayer("Triggers");

        return trigger;
    }

    //这个还不知道在哪用呢
    public GameObject GetIndicatorFromTriggerInfo(TriggerInfo triggerinfo)
    {
        GameObject triggerGo = null;

        if (triggerinfo.gridType == GRIDTYPE_OWN_INVENTORY)
        {
            triggerGo = ownIndicatorArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == GRIDTYPE_OPONENT_INVENTORY)
        {
            triggerGo = oponentIndicatorArray[triggerinfo.gridX];
        }
        else if (triggerinfo.gridType == GRIDTYPE_HEXA_MAP)
        {
            triggerGo = mapIndicatorArray[triggerinfo.gridX, triggerinfo.gridZ];
        }


        return triggerGo;
    }

    //重置指示器颜色
    public void resetIndicators()
    {
        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ / 2; z++)
            {
                mapIndicatorArray[x, z].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
            }
        }

        for (int x = 0; x < 9; x++)
        {
            ownIndicatorArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
        }

    }

    //展示指示器
    public void ShowIndicators()
    {
        indicatorContainer.SetActive(true);
    }

    //隐藏指示器
    public void HideIndicators()
    {
        indicatorContainer.SetActive(false);
    }
}
