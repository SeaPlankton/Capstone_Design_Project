using UnityEngine;


public class DataManager : MonoBehaviour
{
    //��ü ������ �������� Load �ϱ�����
    public ItemDataList ItemList;

    private void Start()
    {
        SaveDB();
    }
    //DB ������ ���� ��� �Լ�
    public void SaveDB()
    {
        SaveFirstItemDB();
    }

    //�ʱ� ������ ���� ����
    public void SaveFirstItemDB()
    {
        ItemList = new ItemDataList();
        ItemList.ItemList = new Item[37];

        Item itemDB = new Item();
        itemDB.ItemName = "����";
        itemDB.ItemType = "����";
        itemDB.ItemRarity = "Normal";
        itemDB.Requirement = false;
        itemDB.Reduplicatable = true;
        itemDB.ItemNumber = 0;
        itemDB.ItemDescription = "���� ���� ���� �� ���׷��̵�";

        Item itemDB1 = new Item();
        itemDB1.ItemName = "���� ����";
        itemDB1.ItemType = "����";
        itemDB1.ItemRarity = "Normal";
        itemDB1.Requirement = false;
        itemDB1.Reduplicatable = true;
        itemDB1.ItemNumber = 1;
        itemDB1.ItemDescription = "���� ���� ���� ���� �� ���׷��̵�";

        Item itemDB2 = new Item();
        itemDB2.ItemName = "��ź��";
        itemDB2.ItemType = "����";
        itemDB2.ItemRarity = "Normal";
        itemDB2.Requirement = false;
        itemDB2.Reduplicatable = true;
        itemDB2.ItemNumber = 2;
        itemDB2.ItemDescription = "��ź�� ���� ���� �� ���׷��̵�";

        Item itemDB3 = new Item();
        itemDB3.ItemName = "���� ����";
        itemDB3.ItemType = "����";
        itemDB3.ItemRarity = "Normal";
        itemDB3.Requirement = false;
        itemDB3.Reduplicatable = true;
        itemDB3.ItemNumber = 3;
        itemDB3.ItemDescription = "���� ���� ���� ���� �� ���׷��̵�";

        Item itemDB4 = new Item();
        itemDB4.ItemName = "����ī��";
        itemDB4.ItemType = "����";
        itemDB4.ItemRarity = "Normal";
        itemDB4.Requirement = false;
        itemDB4.Reduplicatable = true;
        itemDB4.ItemNumber = 4;
        itemDB4.ItemDescription = "����ī�� ���� ���� �� ���׷��̵�";

        Item itemDB5 = new Item();
        itemDB5.AccessoryWeaponType = new string[6];
        itemDB5.ItemName = "FMJ";
        itemDB5.ItemType = "�׼�����";
        itemDB5.ItemRarity = "Normal";
        itemDB5.Requirement = false;
        itemDB5.Reduplicatable = true;
        itemDB5.ItemNumber = 5;
        itemDB5.ItemDescription = "ź�� ���׷��̵� - ������ 2 ����";
        itemDB5.AccessoryWeaponType[0] = "����";
        itemDB5.AccessoryWeaponType[1] = "���� ����";
        itemDB5.AccessoryWeaponType[2] = "��ź��";
        itemDB5.AccessoryWeaponType[3] = "���� ����";
        itemDB5.AccessoryWeaponType[4] = "����ī��";
        itemDB5.AccessoryWeaponType[5] = "�������� ��ź��";

        Item itemDB6 = new Item();
        itemDB6.AccessoryWeaponType = new string[3];
        itemDB6.ItemName = "HP";
        itemDB6.ItemType = "�׼�����";
        itemDB6.ItemRarity = "Normal";
        itemDB6.Requirement = false;
        itemDB6.Reduplicatable = true;
        itemDB6.ItemNumber = 6;
        itemDB6.ItemDescription = "ź�� ���׷��̵� - ������ 10 ���� (���� �� ���ݼ��� ����)";
        itemDB6.AccessoryWeaponType[0] = "����";
        itemDB6.AccessoryWeaponType[1] = "���� ����";
        itemDB6.AccessoryWeaponType[2] = "�������� ��ź��";

        Item itemDB7 = new Item();
        itemDB7.AccessoryWeaponType = new string[5];
        itemDB7.ItemName = "Magnum";
        itemDB7.ItemType = "�׼�����";
        itemDB7.ItemRarity = "Normal";
        itemDB7.Requirement = false;
        itemDB7.Reduplicatable = true;
        itemDB7.ItemNumber = 7;
        itemDB7.ItemDescription = "ź�� ���׷��̵� - ������ 10 ����";
        itemDB7.AccessoryWeaponType[0] = "����";
        itemDB7.AccessoryWeaponType[1] = "���� ����";
        itemDB7.AccessoryWeaponType[2] = "���� ����";
        itemDB7.AccessoryWeaponType[3] = "��ź��";
        itemDB7.AccessoryWeaponType[4] = "�������� ��ź��";

        Item itemDB8 = new Item();
        itemDB8.AccessoryWeaponType = new string[2];
        itemDB8.ItemName = "AP";
        itemDB8.ItemType = "�׼�����";
        itemDB8.ItemRarity = "Normal";
        itemDB8.Requirement = false;
        itemDB8.ItemNumber = 8;
        itemDB8.ItemDescription = "ź�� ���׷��̵� - ���ݼ��� ���� ���, ������ 20 ����";
        itemDB8.AccessoryWeaponType[0] = "���� ����";
        itemDB8.AccessoryWeaponType[1] = "���� ����";

        Item itemDB9 = new Item();
        itemDB9.AccessoryWeaponType = new string[3];
        itemDB9.ItemName = "Flechette";
        itemDB9.ItemType = "�׼�����";
        itemDB9.ItemRarity = "Normal";
        itemDB9.Requirement = false;
        itemDB9.Reduplicatable = false;
        itemDB9.ItemNumber = 9;
        itemDB9.ItemDescription = "ź�� ���׷��̵� - ���� �� ���ǿ� ���� ���, ������ 5 ����";
        itemDB9.AccessoryWeaponType[0] = "����";
        itemDB9.AccessoryWeaponType[1] = "��ź��";
        itemDB9.AccessoryWeaponType[2] = "�������� ��ź��";

        Item itemDB10 = new Item();
        itemDB10.AccessoryWeaponType = new string[3];
        itemDB10.ItemName = "000 Buckshot";
        itemDB10.ItemType = "�׼�����";
        itemDB10.ItemRarity = "Normal";
        itemDB10.Requirement = false;
        itemDB10.Reduplicatable = false;
        itemDB10.ItemNumber = 10;
        itemDB10.ItemDescription = "���� �� ���ǿ� ���� ����, ������ 5 ����";
        itemDB10.AccessoryWeaponType[0] = "����";
        itemDB10.AccessoryWeaponType[1] = "��ź��";
        itemDB10.AccessoryWeaponType[2] = "�������� ��ź��";

        Item itemDB11 = new Item();
        itemDB11.AccessoryWeaponType = new string[1];
        itemDB11.ItemName = "���� ����ź";
        itemDB11.ItemType = "�׼�����";
        itemDB11.ItemRarity = "Normal";
        itemDB11.Requirement = false;
        itemDB11.Reduplicatable = false;
        itemDB11.ItemNumber = 11;
        itemDB11.ItemDescription = "ź�� ���׷��̵� - ����ī�� ������ 30 ���� / ���� ���� 30% ����";
        itemDB11.AccessoryWeaponType[0] = "����ī��";

        Item itemDB12 = new Item();
        itemDB12.AccessoryWeaponType = new string[6];
        itemDB12.ItemName = "LCAA 6251";
        itemDB12.ItemType = "�׼�����";
        itemDB12.ItemRarity = "Normal";
        itemDB12.Requirement = false;
        itemDB12.Reduplicatable = true;
        itemDB12.ItemNumber = 12;
        itemDB12.ItemDescription = "HP 50 ��� �Һ�, ��� ������ 25 ����";
        itemDB12.AccessoryWeaponType[0] = "����";
        itemDB12.AccessoryWeaponType[1] = "���� ����";
        itemDB12.AccessoryWeaponType[2] = "��ź��";
        itemDB12.AccessoryWeaponType[3] = "���� ����";
        itemDB12.AccessoryWeaponType[4] = "����ī��";
        itemDB12.AccessoryWeaponType[5] = "�������� ��ź��";

        Item itemDB13 = new Item();
        itemDB13.AccessoryWeaponType = new string[1];
        itemDB13.ItemName = "��ũ 1";
        itemDB13.ItemType = "�׼�����";
        itemDB13.ItemRarity = "Normal";
        itemDB13.Requirement = false;
        itemDB13.Reduplicatable = false;
        itemDB13.ItemNumber = 13;
        itemDB13.ItemDescription = "��ź���� ź ������ �ٿ��ش�. (��ź�� ����)";
        itemDB13.AccessoryWeaponType[0] = "��ź��";

        Item itemDB14 = new Item();
        itemDB14.AccessoryWeaponType = new string[1];
        itemDB14.ItemName = "�������";
        itemDB14.ItemType = "�׼�����";
        itemDB14.ItemRarity = "Normal";
        itemDB14.Requirement = false;
        itemDB14.Reduplicatable = false;
        itemDB14.ItemNumber = 14;
        itemDB14.ItemDescription = "��ź���� ź ���� ������ �¿� 10�� �����ش�. (��ź�� ����)";
        itemDB14.AccessoryWeaponType[0] = "��ź��";

        Item itemDB15 = new Item();
        itemDB15.AccessoryWeaponType = new string[1];
        itemDB15.ItemName = "�׷�����1";
        itemDB15.ItemType = "�׼�����";
        itemDB15.ItemRarity = "Normal";
        itemDB15.Requirement = false;
        itemDB15.Reduplicatable = false;
        itemDB15.ItemNumber = 15;
        itemDB15.ItemDescription =
            "���� ������ ź���� ��ȭ���� �ֺ��� ���ظ� ��ġ�� �Ѵ�. (���� ���� ����)";
        itemDB15.AccessoryWeaponType[0] = "���� ����";

        Item itemDB16 = new Item();
        itemDB16.AccessoryWeaponType = new string[1];
        itemDB16.ItemName = "�׷�����2";
        itemDB16.ItemType = "�׼�����";
        itemDB16.ItemRarity = "Normal";
        itemDB16.Requirement = true;
        itemDB16.Reduplicatable = false;
        itemDB16.ItemNumber = 16;
        itemDB16.ItemDescription =
            "���� ������ ź���� ��ȭ���� �ֺ��� ������ ���ظ� ��ġ�� �Ѵ�. (���� ���� ����)";
        itemDB16.AccessoryWeaponType[0] = "���� ����";

        Item itemDB17 = new Item();
        itemDB17.AccessoryWeaponType = new string[1];
        itemDB17.ItemName = "����Ʈ ź";
        itemDB17.ItemType = "�׼�����";
        itemDB17.ItemRarity = "Normal";
        itemDB17.Requirement = false;
        itemDB17.Reduplicatable = false;
        itemDB17.ItemNumber = 17;
        itemDB17.ItemDescription =
            "������ źȯ�� �߻��Ͽ� ������ �̵��� �����մϴ�. (���� ���� ����)";
        itemDB17.AccessoryWeaponType[0] = "���� ����";

        Item itemDB18 = new Item();
        itemDB18.AccessoryWeaponType = new string[2];
        itemDB18.ItemName = "��Ŵ��";
        itemDB18.ItemType = "�׼�����";
        itemDB18.ItemRarity = "Normal";
        itemDB18.Requirement = false;
        itemDB18.Reduplicatable = false;
        itemDB18.ItemNumber = 18;
        itemDB18.ItemDescription =
            "������ �� �տ� ������ ��� �ٴϰ� �˴ϴ�. (���� �� ���� ���� ����)";
        itemDB18.AccessoryWeaponType[0] = "����";
        itemDB18.AccessoryWeaponType[1] = "���� ����";

        Item itemDB19 = new Item();
        itemDB19.AccessoryWeaponType = new string[3];
        itemDB19.ItemName = "������";
        itemDB19.ItemType = "�׼�����";
        itemDB19.ItemRarity = "Normal";
        itemDB19.Requirement = false;
        itemDB19.Reduplicatable = false;
        itemDB19.ItemNumber = 19;
        itemDB19.ItemDescription =
            "�ѱ⿡ �����⸦ �޾� ������ �������� �����ݴϴ�. (���� �� ���� ���� ����)";
        itemDB19.AccessoryWeaponType[0] = "����";
        itemDB19.AccessoryWeaponType[1] = "���� ����";
        itemDB19.AccessoryWeaponType[2] = "�������� ��ź��";

        Item itemDB20 = new Item();
        itemDB20.AccessoryWeaponType = new string[3];
        itemDB20.ItemName = "�ҹɸ����� ��õ";
        itemDB20.ItemType = "�׼�����";
        itemDB20.ItemRarity = "Normal";
        itemDB20.Requirement = false;
        itemDB20.Reduplicatable = false;
        itemDB20.ItemNumber = 20;
        itemDB20.ItemDescription =
            "����� ���⿡ �����Ⱑ �޷��ִٸ�, �����ϰ� �����... �������� ������ ���� �մϴ�. (���� �� ���� ���� ����)";
        itemDB20.AccessoryWeaponType[0] = "����";
        itemDB20.AccessoryWeaponType[1] = "���� ����";
        itemDB20.AccessoryWeaponType[2] = "�������� ��ź��";

        Item itemDB21 = new Item();
        itemDB21.ItemName = "��ô�� ������";
        itemDB21.ItemType = "����";
        itemDB21.ItemRarity = "Normal";
        itemDB21.Requirement = false;
        itemDB21.Reduplicatable = false;
        itemDB21.ItemNumber = 21;
        itemDB21.ItemDescription = "��ô ������ ���� ����";

        Item itemDB22 = new Item();
        itemDB22.ItemName = "Javelin";
        itemDB22.ItemType = "����";
        itemDB22.ItemRarity = "Normal";
        itemDB22.Requirement = false;
        itemDB22.Reduplicatable = false;
        itemDB22.ItemNumber = 22;
        itemDB22.ItemDescription = "��â ���� ����";

        Item itemDB23 = new Item();
        itemDB23.ItemName = "�żӽ� Lv.1";
        itemDB23.ItemType = "�׼�����";
        itemDB23.ItemRarity = "Normal";
        itemDB23.Requirement = false;
        itemDB23.Reduplicatable = false;
        itemDB23.ItemNumber = 23;
        itemDB23.ItemDescription = "�÷��̾��� �̵��ӵ��� ���������ش�.";

        Item itemDB24 = new Item();
        itemDB24.ItemName = "�żӽ� Lv.2";
        itemDB24.ItemType = "�׼�����";
        itemDB24.ItemRarity = "Normal";
        itemDB24.Requirement = true;
        itemDB24.Reduplicatable = false;
        itemDB24.ItemNumber = 24;
        itemDB24.ItemDescription = "�÷��̾��� �̵��ӵ��� ���������ش�.";

        Item itemDB25 = new Item();
        itemDB25.ItemName = "�żӽ� Lv.3";
        itemDB25.ItemType = "�׼�����";
        itemDB25.ItemRarity = "Normal";
        itemDB25.Requirement = true;
        itemDB25.Reduplicatable = false;
        itemDB25.ItemNumber = 25;
        itemDB25.ItemDescription = "�÷��̾��� �̵��ӵ��� ���������ش�.";

        Item itemDB26 = new Item();
        itemDB26.AccessoryWeaponType = new string[6];
        itemDB26.ItemName = "������";
        itemDB26.ItemType = "�׼�����";
        itemDB26.ItemRarity = "Unique";
        itemDB26.Requirement = false;
        itemDB26.Reduplicatable = false;
        itemDB26.ItemNumber = 26;
        itemDB26.ItemDescription = "�������� �ູ���� ������� ��ü�� ��ȭ �� ȸ�������ݴϴ�.";
        itemDB26.AccessoryWeaponType[0] = "����";
        itemDB26.AccessoryWeaponType[1] = "���� ����";
        itemDB26.AccessoryWeaponType[2] = "��ź��";
        itemDB26.AccessoryWeaponType[3] = "���� ����";
        itemDB26.AccessoryWeaponType[4] = "����ī��";
        itemDB26.AccessoryWeaponType[5] = "�������� ��ź��";

        Item itemDB27 = new Item();
        itemDB27.AccessoryWeaponType = new string[6];
        itemDB27.ItemName = "�ȱ���";
        itemDB27.ItemType = "�׼�����";
        itemDB27.ItemRarity = "Unique";
        itemDB27.Requirement = false;
        itemDB27.Reduplicatable = false;
        itemDB27.ItemNumber = 27;
        itemDB27.ItemDescription = "�������� �������� ������� �������� ���� ��ȭ�˴ϴ�.";
        itemDB27.AccessoryWeaponType[0] = "����";
        itemDB27.AccessoryWeaponType[1] = "���� ����";
        itemDB27.AccessoryWeaponType[2] = "��ź��";
        itemDB27.AccessoryWeaponType[3] = "���� ����";
        itemDB27.AccessoryWeaponType[4] = "����ī��";
        itemDB27.AccessoryWeaponType[5] = "�������� ��ź��";

        Item itemDB28 = new Item();
        itemDB28.ItemName = "ȸ�߽ð�";
        itemDB28.ItemType = "�׼�����";
        itemDB28.ItemRarity = "Unique";
        itemDB28.Requirement = false;
        itemDB28.Reduplicatable = false;
        itemDB28.ItemNumber = 28;
        itemDB28.ItemDescription = "�������� ������ ����ڿ��� Ư�� �ɷ��� �ο��˴ϴ�.";

        Item itemDB29 = new Item();
        itemDB29.ItemName = "����";
        itemDB29.ItemType = "�׼�����";
        itemDB29.ItemRarity = "Unique";
        itemDB29.Requirement = false;
        itemDB29.Reduplicatable = false;
        itemDB29.ItemNumber = 29;
        itemDB29.ItemDescription = "�������� ������ ����ڿ��� Ư�� �ɷ��� �ο��˴ϴ�.";

        Item itemDB30 = new Item();
        itemDB30.ItemName = "���ɰ�";
        itemDB30.ItemType = "�׼�����";
        itemDB30.ItemRarity = "Unique";
        itemDB30.Requirement = false;
        itemDB30.Reduplicatable = false;
        itemDB30.ItemNumber = 30;
        itemDB30.ItemDescription = "�������� �������� ������� Ȱ���� ���� �����˴ϴ�.";

        Item itemDB31 = new Item();
        itemDB31.ItemName = "���� ����";
        itemDB31.ItemType = "�׼�����";
        itemDB31.ItemRarity = "Unique";
        itemDB31.Requirement = false;
        itemDB31.Reduplicatable = false;
        itemDB31.ItemNumber = 31;
        itemDB31.ItemDescription = "���⸦ ������ ������ �׾Ƶ� �������� �ʱ�ȭ ���� �ʽ��ϴ�.";

        Item itemDB32 = new Item();
        itemDB32.ItemName = "���� ����";
        itemDB32.ItemType = "�׼�����";
        itemDB32.ItemRarity = "Unique";
        itemDB32.Requirement = false;
        itemDB32.Reduplicatable = false;
        itemDB32.ItemNumber = 32;
        itemDB32.ItemDescription = "���� ������ �ѱ��� ������ 1 �����մϴ�.";

        Item itemDB33 = new Item();
        itemDB33.ItemName = "�޽þ��� ���ϼ�";
        itemDB33.ItemType = "�׼�����";
        itemDB33.ItemRarity = "Unique";
        itemDB33.Requirement = false;
        itemDB33.Reduplicatable = false;
        itemDB33.ItemNumber = 33;
        itemDB33.ItemDescription = "��� ��, ��Ʈ �� 5�ʰ� �־����ϴ�. �̶� 35���� �浿���� ���� ��, �浿���� ����� �밡�� ����� ��Ȱ�մϴ�.";

        Item itemDB34 = new Item();
        itemDB34.ItemName = "���޻��� ��";
        itemDB34.ItemNumber = 34;
        itemDB34.ItemDescription = "HP�� 30 ȸ���ϴ� �������̴�.";

        Item itemDB35 = new Item();
        itemDB35.ItemName = "���޻��� ��";
        itemDB35.ItemNumber = 35;
        itemDB35.ItemDescription = "HP�� 70 ȸ���ϴ� �������̴�.";

        Item itemDB36 = new Item();
        itemDB36.ItemName = "���޻��� ��";
        itemDB36.ItemNumber = 36;
        itemDB36.ItemDescription = "HP�� 100 ȸ���ϴ� �������̴�.";

        ItemList.ItemList[0] = itemDB;
        ItemList.ItemList[1] = itemDB1;
        ItemList.ItemList[2] = itemDB2;
        ItemList.ItemList[3] = itemDB3;
        ItemList.ItemList[4] = itemDB4;
        ItemList.ItemList[5] = itemDB5;
        ItemList.ItemList[6] = itemDB6;
        ItemList.ItemList[7] = itemDB7;
        ItemList.ItemList[8] = itemDB8;
        ItemList.ItemList[9] = itemDB9;
        ItemList.ItemList[10] = itemDB10;
        ItemList.ItemList[11] = itemDB11;
        ItemList.ItemList[12] = itemDB12;
        ItemList.ItemList[13] = itemDB13;
        ItemList.ItemList[14] = itemDB14;
        ItemList.ItemList[15] = itemDB15;
        ItemList.ItemList[16] = itemDB16;
        ItemList.ItemList[17] = itemDB17;
        ItemList.ItemList[18] = itemDB18;
        ItemList.ItemList[19] = itemDB19;
        ItemList.ItemList[20] = itemDB20;
        ItemList.ItemList[21] = itemDB21;
        ItemList.ItemList[22] = itemDB22;
        ItemList.ItemList[23] = itemDB23;
        ItemList.ItemList[24] = itemDB24;
        ItemList.ItemList[25] = itemDB25;
        ItemList.ItemList[26] = itemDB26;
        ItemList.ItemList[27] = itemDB27;
        ItemList.ItemList[28] = itemDB28;
        ItemList.ItemList[29] = itemDB29;
        ItemList.ItemList[30] = itemDB30;
        ItemList.ItemList[31] = itemDB31;
        ItemList.ItemList[32] = itemDB32;
        ItemList.ItemList[33] = itemDB33;
        ItemList.ItemList[34] = itemDB34;
        ItemList.ItemList[35] = itemDB35;
        ItemList.ItemList[36] = itemDB36;
    }
}
