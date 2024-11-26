using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDataList
{
    public Item[] ItemList;
}

[System.Serializable]
public class Item
{
    public string ItemName;
    public string ItemType;         //아이템 종류
    public string ItemRarity;       //티어
    public bool Requirement;        //선행조건
    public bool Reduplicatable;     //중복가능
    public string[] AccessoryWeaponType;    //액세서리 효과 적용되는 무기
    public int ItemNumber;
    public string ItemDescription;
}
