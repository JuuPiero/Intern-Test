using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalItem : Item
{
    public enum eNormalType
    {
        TYPE_ONE,
        TYPE_TWO,
        TYPE_THREE,
        TYPE_FOUR,
        TYPE_FIVE,
        TYPE_SIX,
        TYPE_SEVEN
    }

    public eNormalType ItemType;

    public void SetType(eNormalType type)
    {
        ItemType = type;
    }

    public override string GetPrefabName()
    {
        string prefabname = string.Empty;
        switch (ItemType)
        {
            case eNormalType.TYPE_ONE:
                prefabname = Constants.PREFAB_NORMAL_TYPE_ONE;
                break;
            case eNormalType.TYPE_TWO:
                prefabname = Constants.PREFAB_NORMAL_TYPE_TWO;
                break;
            case eNormalType.TYPE_THREE:
                prefabname = Constants.PREFAB_NORMAL_TYPE_THREE;
                break;
            case eNormalType.TYPE_FOUR:
                prefabname = Constants.PREFAB_NORMAL_TYPE_FOUR;
                break;
            case eNormalType.TYPE_FIVE:
                prefabname = Constants.PREFAB_NORMAL_TYPE_FIVE;
                break;
            case eNormalType.TYPE_SIX:
                prefabname = Constants.PREFAB_NORMAL_TYPE_SIX;
                break;
            case eNormalType.TYPE_SEVEN:
                prefabname = Constants.PREFAB_NORMAL_TYPE_SEVEN;
                break;
        }

        return prefabname;
    }

    public static readonly Dictionary<NormalItem.eNormalType, Color> Colors =
        new Dictionary<NormalItem.eNormalType, Color>()
        {
            { NormalItem.eNormalType.TYPE_ONE,   Color.red },
            { NormalItem.eNormalType.TYPE_TWO,   Color.green },
            { NormalItem.eNormalType.TYPE_THREE, Color.blue },
            { NormalItem.eNormalType.TYPE_FOUR,  Color.yellow },
            { NormalItem.eNormalType.TYPE_FIVE,  Color.magenta },
            { NormalItem.eNormalType.TYPE_SIX,   Color.cyan },
            { NormalItem.eNormalType.TYPE_SEVEN, new Color(1f, 0.5f, 0f) } // cam
        };

    internal override bool IsSameType(Item other)
    {
        NormalItem it = other as NormalItem;

        return it != null && it.ItemType == this.ItemType;
    }
}
