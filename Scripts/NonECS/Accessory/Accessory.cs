using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory
{
    //액세서리 이름
    [HideInInspector]
    public string ItemName;
    //액세서리 번호
    [HideInInspector]
    public int ItemNumber;
    //액세서리 설명
    [HideInInspector]
    public string ItemDescription;
    //액세서리 몇번 먹었는지 체크
    [HideInInspector]
    public int AcquireCount = 0;
    [HideInInspector]
    public string[] AccessoryWeaponType;

    //액세서리에 맞는 아이콘 출력예정?
    public void ShowIcon()
    {

    }

}
