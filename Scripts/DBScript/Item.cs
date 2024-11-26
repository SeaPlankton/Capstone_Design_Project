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
    public string ItemType;         //������ ����
    public string ItemRarity;       //Ƽ��
    public bool Requirement;        //��������
    public bool Reduplicatable;     //�ߺ�����
    public string[] AccessoryWeaponType;    //�׼����� ȿ�� ����Ǵ� ����
    public int ItemNumber;
    public string ItemDescription;
}
