using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory
{
    //�׼����� �̸�
    [HideInInspector]
    public string ItemName;
    //�׼����� ��ȣ
    [HideInInspector]
    public int ItemNumber;
    //�׼����� ����
    [HideInInspector]
    public string ItemDescription;
    //�׼����� ��� �Ծ����� üũ
    [HideInInspector]
    public int AcquireCount = 0;
    [HideInInspector]
    public string[] AccessoryWeaponType;

    //�׼������� �´� ������ ��¿���?
    public void ShowIcon()
    {

    }

}
