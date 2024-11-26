// =============================================================================
// 文件名称: TriggerInfo.cs
// 作者: 刘垚
// 创建日期: 2024.11.24
// 更新日期：2024.11.24
// 使用的设计模式：
// 备注：
// =============================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class to identify player interaction on the map
/// </summary>
public class TriggerInfo : MonoBehaviour
{
    ///The type of the grid, GRIDTYPE_OWN_INVENTORY = 0 GRIDTYPE_OPONENT_INVENTORY = 1 GRIDTYPE_HEXA_MAP = 2;
    public int gridType = -1;

    ///X position on the grid
    public int gridX = -1;

    ///Z position on the grid
    public int gridZ = -1;

}
