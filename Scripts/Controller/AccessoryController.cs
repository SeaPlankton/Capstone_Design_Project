using Miku.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryController : MonoBehaviour
{
    //���� ���� ȹ���� ��� �׼�������
    [HideInInspector]
    public List<Accessory> Accessories = new List<Accessory>();

    //���� ���� ȹ�濩��
    private bool _isTextBook = false;

    //�׼����� ���� �Լ�
    public void GenerateAccessory(Item item)
    {
        Accessory accessory = new Accessory();

        accessory.ItemName = item.ItemName;
        accessory.ItemNumber = item.ItemNumber;
        accessory.ItemDescription = item.ItemDescription;
        accessory.AcquireCount = 1;

        if (item.ItemType == "�׼�����")
        {
            //���� ���⿡ ���� �׼������� ��� �ش� ���⿡ ���� ���� ä���ֱ�
            if (item.AccessoryWeaponType != null)
            {
                accessory.AccessoryWeaponType = new string[item.AccessoryWeaponType.Length];
                for (int i = 0; i < item.AccessoryWeaponType.Length; i++)
                {
                    accessory.AccessoryWeaponType[i] = item.AccessoryWeaponType[i];
                }
            }

            //�̹� ȹ���� �׼������� �κ��丮�� �߰�x
            bool isAcquire;
            int index;
            (isAcquire, index) = FindAccessory(item);

            if (isAcquire)
            {
                Accessories[index].AcquireCount++;
            }
            else if (!isAcquire)
            {
                //���� �����̳� �÷�ü �׼������� �κ��丮���� �� �� �Ѱ� ����
                CheckShotgunAccessory(item);

                //�׼����� �κ��丮�� �߰�
                Accessories.Add(accessory);
            }
        }

        ApplyAccessoryEffect(accessory, true);
    }

    //������ �׼����� ȿ�� ����
    //true�� ���� ȹ������ �ִ� ��繫�� ������ ���� �� ȿ��
    //false�� ���������� ȹ���� ���⸸ ȿ�� - �׼������� �̸� �Ծ�ΰ� ���߿� ���� ȹ������ ��
    public void ApplyAccessoryEffect(Accessory accessory, bool applyAll)
    {
        switch (accessory.ItemNumber)
        {
            //FMJ - ��� ���� ������ 2 ����
            case 5:
                if (applyAll)
                {
                    for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[i].WeaponDamageUp(2);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(2);
                }
                break;
            //HP
            case 6:
                if (applyAll)
                {
                    int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                    int rifle = Managers.Instance.Game.WeaponController.FindRifle();

                    if (pistol != -1)
                        Managers.Instance.Game.WeaponController.Weapons[pistol].WeaponDamageUp(10);
                    if (rifle != -1)
                        Managers.Instance.Game.WeaponController.Weapons[rifle].WeaponDamageUp(10);
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(10);
                }
                break;
            //Magnum
            case 7:
                if (applyAll)
                {
                    int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                    int rifle = Managers.Instance.Game.WeaponController.FindRifle();
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();
                    int shotgun = Managers.Instance.Game.WeaponController.FindShotgun();

                    if (pistol != -1)
                        Managers.Instance.Game.WeaponController.Weapons[pistol].WeaponDamageUp(10);
                    if (rifle != -1)
                        Managers.Instance.Game.WeaponController.Weapons[rifle].WeaponDamageUp(10);
                    if (sniper != -1)
                        Managers.Instance.Game.WeaponController.Weapons[sniper].WeaponDamageUp(10);
                    if (shotgun != -1)
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].WeaponDamageUp(10);
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(10);
                }
                break;
            //AP
            case 8:
                if (applyAll)
                {
                    int rifle = Managers.Instance.Game.WeaponController.FindRifle();
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();

                    if (rifle != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[rifle].WeaponDamageUp(20);
                        Managers.Instance.Game.WeaponController.Weapons[rifle].SetRiflePenetration(true);
                    }
                    if (sniper != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[sniper].WeaponDamageUp(20);
                        Managers.Instance.Game.WeaponController.Weapons[sniper].SetSniperPenetration(true);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(20);

                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "���� ����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetRiflePenetration(true);
                    }
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "���� ����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetSniperPenetration(true);
                    }
                }
                break;
            //Flechette
            case 9:
                if (applyAll)
                {
                    int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                    int shotgun = Managers.Instance.Game.WeaponController.FindShotgun();

                    if (pistol != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[pistol].WeaponDamageUp(-5);
                        Managers.Instance.Game.WeaponController.Weapons[pistol].SetPistolPenetration(true);
                    }
                    if (shotgun != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].WeaponDamageUp(-5);
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].SetShotgunPenetration(true);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(-5);

                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetPistolPenetration(true);
                    }
                    else if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "��ź��")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetShotgunPenetration(true);
                    }
                }
                break;
            //000 Buckshot
            case 10:
                if (applyAll)
                {
                    int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                    int shotgun = Managers.Instance.Game.WeaponController.FindShotgun();

                    if (pistol != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[pistol].WeaponDamageUp(5);
                        Managers.Instance.Game.WeaponController.Weapons[pistol].SetPistolPenetration(false);
                    }
                    if (shotgun != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].WeaponDamageUp(5);
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].SetShotgunPenetration(false);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(5);

                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetPistolPenetration(false);
                    }
                    else if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "��ź��")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetShotgunPenetration(false);
                    }
                }
                break;
            //���� ����ź
            case 11:
                if (applyAll)
                {
                    int rpg = Managers.Instance.Game.WeaponController.FindRPG();

                    if (rpg != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[rpg].WeaponDamageUp(30);
                        Managers.Instance.Game.WeaponController.Weapons[rpg].SetRpgRangeUpgrade();
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(30);
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetRpgRangeUpgrade();
                }
                break;
            //LCAA 6251
            case 12:
                if (applyAll)
                {
                    Managers.Instance.Game.Player.PlayerCombat.DropHP(50);
                    for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[i].WeaponDamageUp(25);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(25);
                }
                break;
            //��ũ1
            case 13:
                if (applyAll)
                {
                    int shotgun = Managers.Instance.Game.WeaponController.FindShotgun();

                    if (shotgun != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].WeaponDamageDouble();
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].SetShotgunChoke();
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageDouble();
                }
                break;
            //������ ��
            case 14:
                if (applyAll)
                {
                    int shotgun = Managers.Instance.Game.WeaponController.FindShotgun();

                    if (shotgun != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[shotgun].SetShotgunDonald();
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetShotgunDonald();
                }
                break;
            //�׷�����1
            case 15:
                if (applyAll)
                {
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();

                    //�������� �Ѿ� �ǰݹ��� ����
                    if (sniper != -1)
                        Managers.Instance.Game.WeaponController.Weapons[sniper].SetSniperRangeUpgrade1();

                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetSniperRangeUpgrade1();
                }
                break;
            //�׷�����2
            case 16:
                if (applyAll)
                {
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();

                    //�������� �Ѿ� �ǰݹ��� ����
                    if (sniper != -1)
                        Managers.Instance.Game.WeaponController.Weapons[sniper].SetSniperRangeUpgrade2();

                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetSniperRangeUpgrade2();
                }
                break;
            //����Ʈ ź
            case 17:
                if (applyAll)
                {
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();

                    //�������� �Ѿ˿� ���ο� �ο�
                    if (sniper != -1)
                        Managers.Instance.Game.WeaponController.Weapons[sniper].SetSlowSniperBullet();

                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetSlowSniperBullet();
                }
                break;
            //��Ŵ��
            case 18:
                if (applyAll)
                {
                    int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                    int rifle = Managers.Instance.Game.WeaponController.FindRifle();

                    //����, ���� ���� �Ѿ� 2�ٱ�� ����
                    if (pistol != -1)
                        Managers.Instance.Game.WeaponController.Weapons[pistol].SetTwoBullets();
                    //����, ���� ���� �Ѿ� 2�ٱ�� ����
                    if (rifle != -1)
                        Managers.Instance.Game.WeaponController.Weapons[rifle].SetTwoBullets();

                }
                else
                {
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetTwoBullets();
                    }
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "���� ����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetTwoBullets();
                    }
                }
                break;
            //������
            case 19:
                if (applyAll)
                {
                    int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                    int rifle = Managers.Instance.Game.WeaponController.FindRifle();

                    if (pistol != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[pistol].WeaponDamageUp(-3);
                        Managers.Instance.Game.WeaponController.Weapons[pistol].SetFireInterval(1.2f);
                    }
                    if (rifle != -1)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[rifle].WeaponDamageUp(-3);
                        Managers.Instance.Game.WeaponController.Weapons[rifle].SetFireInterval(1.2f);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(-3);
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetFireInterval(1.5f);
                    }
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "���� ����")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetFireInterval(1.2f);
                    }
                }
                break;
            //�ҹɸ����� ��õ
            case 20:
                bool isExist = false;

                //�����Ⱑ ������
                for (int i = 0; i < Accessories.Count; i++)
                {
                    if (Accessories[i].ItemName == "������")
                    {
                        isExist = true;
                        break;
                    }
                }

                if (applyAll)
                {
                    if (isExist)
                    {
                        int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                        int rifle = Managers.Instance.Game.WeaponController.FindRifle();

                        if (pistol != -1)
                        {
                            Managers.Instance.Game.WeaponController.Weapons[pistol].WeaponDamagePercent(15f);
                        }
                        if (rifle != -1)
                        {
                            Managers.Instance.Game.WeaponController.Weapons[rifle].WeaponDamagePercent(15f);
                        }
                    }

                }
                else
                {
                    if (isExist)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamagePercent(15f);
                    }
                }
                break;
            //�żӽ� LV1
            case 23:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(15f);
                break;
            //�żӽ� LV2
            case 24:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(15f);
                break;
            //�żӽ� LV3
            case 25:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(10f);
                break;
            //������
            case 26:
                if (applyAll)
                {
                    Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(25f);
                    Managers.Instance.Game.Player.PlayerCombat.IncreaseMaxHP(25);
                    Managers.Instance.Game.Player.PlayerCombat.RecoverFullHP();
                    for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[i].WeaponDamageUp(25);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(25);
                }
                break;
            //�ȱ���
            case 27:
                if (applyAll)
                {
                    for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
                    {
                        Managers.Instance.Game.WeaponController.Weapons[i].WeaponDamageUp(100);
                    }
                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        WeaponDamageUp(100);
                }
                break;
            //ȸ�߽ð�
            case 28:
                Managers.Instance.Game.WeaponController.SetOnSwanKnife();
                Managers.Instance.Game.ZombieSpawnManager.ExecutionFunction();
                Managers.Instance.Game.ZombieController.SetZombieSpeedForAccessory();
                break;
            //����
            case 29:
                Managers.Instance.Game.Player.PlayerCombat.ContinuouslyRecoverHP();
                break;
            //���ɰ�
            case 30:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(50f);
                Managers.Instance.Game.Player.PlayerCombat.SetMissDamage();
                break;
            //��������
            case 31:
                _isTextBook = true;
                break;
            //��������
            case 32:
                Managers.Instance.Game.WeaponController.IncreaseMaximumWeaponCount();
                break;
            //�޽þ��� ���ϼ�
            case 33:
                break;
            //���޻��� ��
            case 34:
                Managers.Instance.Game.Player.PlayerCombat.RecoverHP(30);
                break;
            //���޻��� ��
            case 35:
                Managers.Instance.Game.Player.PlayerCombat.RecoverHP(70);
                break;
            //���޻��� ��
            case 36:
                Managers.Instance.Game.Player.PlayerCombat.RecoverHP(100);
                break;
        }
    }

    //�ߺ��Ǵ� �׼������� ����Ʈ�� ���� �ʰ�, LV�� �÷��ֱ� ����
    private (bool, int) FindAccessory(Item item)
    {
        bool isAcquired = false;
        int index = -1;

        for (int i = 0; i < Accessories.Count; i++)
        {
            if (Accessories[i].ItemName == item.ItemName)
            {
                isAcquired = true;
                index = i;
                break;
            }
        }

        return (isAcquired, index);
    }

    //�׼��������� Flechette�� 000 Buckshot�� ������ �� ���⿡ ����
    private void CheckShotgunAccessory(Item item)
    {
        if (item.ItemName == "000 Buckshot")
        {
            for (int i = 0; i < Accessories.Count; i++)
            {
                if (Accessories[i].ItemName == "Flechette")
                {
                    Accessories.RemoveAt(i);
                }
            }
        }
        else if (item.ItemName == "Flechette")
        {
            for (int i = 0; i < Accessories.Count; i++)
            {
                if (Accessories[i].ItemName == "000 Buckshot")
                {
                    Accessories.RemoveAt(i);
                }
            }
        }
    }

    //��Ȱ�� �׼� �ִ��� Ȯ��
    public bool FindRebornAccessory()
    {
        bool isExist = false;

        for (int i = 0; i < Accessories.Count; i++)
        {
            if (Accessories[i].ItemName == "�޽þ��� ���ϼ�")
            {
                isExist = true;
            }
        }

        return isExist;
    }

    //��Ȱ�� �׼����� ��� �� ����
    public void RemoveRebornAccessory()
    {
        for (int i = 0; i < Accessories.Count; i++)
        {
            if (Accessories[i].ItemName == "�޽þ��� ���ϼ�")
            {
                Accessories.RemoveAt(i);
            }
        }
    }

    //���� Ƽ�� ������ �Ծ��� �� ���� Ƽ�� ������ ǥ�� X
    public void CheckHighTierAccessory(Item item)
    {
        //�׷����� 2 ������ 1 ����
        if(item.ItemNumber == 16)
        {
            for(int i = 0; i < Accessories.Count; i++)
            {
                if (Accessories[i].ItemNumber == 15)
                {
                    Accessories.RemoveAt(i);
                }
            }
        }
        //�żӽ� 2 ������ 1 ����
        else if(item.ItemNumber == 24)
        {
            for (int i = 0; i < Accessories.Count; i++)
            {
                if (Accessories[i].ItemNumber == 23)
                {
                    Accessories.RemoveAt(i);
                }
            }
        }
        //�żӽ� 3 ������ 2 ����
        else if (item.ItemNumber == 25)
        {
            for (int i = 0; i < Accessories.Count; i++)
            {
                if (Accessories[i].ItemNumber == 24)
                {
                    Accessories.RemoveAt(i);
                }
            }
        }
    }

    //���⿡ �´� ���� ������ �׼������� ã������ �ڵ�
    //���� ȹ������ ��, ���⸦ ������ ȿ���� �˾ƾ� �Ҷ� 2���� ��쿡 ����Ұ����� ����
    public List<Accessory> FindAccessoryForWeapon(string weaponName)
    {
        List<Accessory> matchingAccessories = new List<Accessory>();
        if (Accessories == null) return matchingAccessories;
        matchingAccessories.Clear();

        //�׼������� loop ���鼭 �ش��ϴ� ���� ã��
        for (int i = 0; i < Accessories.Count; i++)
        {
            if (Accessories[i].AccessoryWeaponType != null)
            {
                for (int j = 0; j < Accessories[i].AccessoryWeaponType.Length; j++)
                {
                    if (weaponName == Accessories[i].AccessoryWeaponType[j])
                    {
                        matchingAccessories.Add(Accessories[i]);
                    }
                }
            }
        }

        return matchingAccessories;
    }

    //���� ���� ȹ���ߴ��� ���� Ȯ��
    public bool CheckTextBook()
    {
        return _isTextBook;
    }
}
