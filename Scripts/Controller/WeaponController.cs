using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //���� �������ִ� �����
    [HideInInspector]
    public List<Weapon> Weapons;

    //���� �����յ�
    //0 - ����, 1 - ������, 2 - ����, 3 - ��������, 4 - ����ī��
    [SerializeField]
    private GameObject[] _weaponPrefab;

    //ĳ���� ������ ȸ���ϴ� �˵��� ���� �ֱ�����
    [SerializeField]
    private WeaponOrbit _weaponOrbit;

    //���� ������ ���̾��Űâ�� ������ ��ġ
    [SerializeField]
    private Transform _weaponPrefabTransform;

    //ȹ�� ������ ���� �ִ�ġ
    private int _maximumWeapon = 2;

    //���� ������ �Ծ��� ���� ����� ���⸦ ������ LV�� �����ϱ� ����
    private int _pistolLV = 0;
    private int _rifleLV = 0;
    private int _shotgunLV = 0;
    private int _sniperLV = 0;
    private int _rpgLV = 0;


    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }

    private void Start()
    {
        _weaponOrbit = _weaponOrbit.GetComponent<WeaponOrbit>();
        InitWeapon();
    }

    private void Init()
    {
        Managers.Instance.Game.WeaponController = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }

    //ó�� �����̶� �����ø�
    public void InitWeapon()
    {
        Item item = new Item();
        GameObject weaponPrefab;

        item = GetItem(0);

        weaponPrefab = Instantiate(_weaponPrefab[0], _weaponPrefabTransform);
        Weapon weapon1 = weaponPrefab.GetComponent<Weapon>();
        Weapons.Add(weapon1);
        weapon1.Name = item.ItemName;
        weapon1.Number = item.ItemNumber;
        weapon1.Description = item.ItemDescription;

        //���� ����� �̹� �Ծ�� �׼����� ȿ�� �����ϱ�����
        List<Accessory> findAccessory = new List<Accessory>();
        findAccessory = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon1.Name);

        //���⿡ �׼����� ȿ�� ����
        for (int i = 0; i < findAccessory.Count; i++)
        {
            Managers.Instance.Game.AccessoryController.ApplyAccessoryEffect(findAccessory[i], false);
        }

        _weaponOrbit.weapons.Add(weaponPrefab);

        _pistolLV = 1;
        //_rifleLV = 1;
        //_shotgunLV = 1;
        //_sniperLV = 1;
        //_rpgLV = 1;
    }

    public void GenerateWeapon(Item item)
    {
        GameObject weaponPrefab;
        int weaponPrefabCount;
        int weaponLV;
        bool isExist;
        int index;

        (isExist, index) = CheckWeapon(item);
        (weaponPrefabCount, weaponLV) = CheckWeaponType(item, isExist);

        //�̹� �ִ� ������ ������ ����X, LV++
        if (isExist)
        {
            Weapons[index].WeaponLevelUp(weaponLV, false);
            Weapons[index].LV = weaponLV;
        }
        //���� ������ ������ ����, LV1�� ����
        else
        {
            weaponPrefab = Instantiate(_weaponPrefab[weaponPrefabCount], _weaponPrefabTransform);
            Weapon weapon = weaponPrefab.GetComponent<Weapon>();
            Weapons.Add(weapon);
            weapon.Name = item.ItemName;
            weapon.Number = item.ItemNumber;
            weapon.Description = item.ItemDescription;
            weapon.LV = weaponLV;

            if (weaponPrefabCount == 5)
                weapon.Name = "�������� ��ź��";

            //��ô�� ���� �Ծ����� ��ʿ� �ȶ߱�
            if (weaponPrefabCount == 6)
                Managers.Instance.Game.BannerPresenter.LockItem(weapon.Number);

            //Javelin ���� �Ծ����� ��ʿ� �ȶ߱�
            if (weaponPrefabCount == 7)
                Managers.Instance.Game.BannerPresenter.LockItem(weapon.Number);

            //���� ���� ������ ���� ���� ���� + 1 ���� �ɷ�ġ ����
            if (Managers.Instance.Game.AccessoryController.CheckTextBook() && weaponPrefabCount != 5 && weaponPrefabCount != 6)
            {
                weapon.WeaponLevelUp(weaponLV, true);
            }

            //���� ����� �̹� �Ծ�� �׼����� ȿ�� �����ϱ�����
            List<Accessory> findAccessory = new List<Accessory>();
            findAccessory = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon.Name);

            //���⿡ �׼����� ȿ�� ����
            for (int i = 0; i < findAccessory.Count; i++)
            {
                Managers.Instance.Game.AccessoryController.ApplyAccessoryEffect(findAccessory[i], false);
            }

            //��ô������ ��� ������ �˵��� ��Ƶα�
            if (weaponPrefabCount == 6 || weaponPrefabCount == 7)
            {
                _weaponOrbit.weapons2.Add(weaponPrefab);
            }
            else
            {
                //�˵��� ���� �߰�
                _weaponOrbit.weapons.Add(weaponPrefab);
            }
        }
    }

    //ȹ���� ������ Ÿ�� Ȯ��
    //ȹ�� ���ο� ���� LV ó���� ����
    private (int, int) CheckWeaponType(Item item, bool isExist)
    {
        int weaponPrefab = -1;
        int weaponLV = -1;

        switch (item.ItemName)
        {
            case "����":
                weaponPrefab = 0;

                //�̹� ȹ���� ���⿴�ٸ� Lv�� ����
                if (isExist)
                {
                    if (_pistolLV < 7)
                        _pistolLV++;

                    //������ ���� ��� �������� ��ź������ ������ ����
                    if (_pistolLV == 7)
                        weaponPrefab = 5;
                }

                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_pistolLV < 7)
                        _pistolLV++;

                    //������ ���� ��� �������� ��ź������ ������ ����
                    if (_pistolLV == 7)
                        weaponPrefab = 5;
                }
                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _pistolLV = 1;
                }
                weaponLV = _pistolLV;
                break;
            case "���� ����":
                weaponPrefab = 1;

                if (isExist)
                {
                    if (_rifleLV < 7)
                        _rifleLV++;
                }

                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_rifleLV < 7)
                        _rifleLV++;
                }
                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _rifleLV = 1;
                }
                weaponLV = _rifleLV;
                break;
            case "��ź��":
                weaponPrefab = 2;

                //�̹� ȹ���� ���⿴�ٸ� Lv�� ����
                if (isExist)
                {
                    if (_shotgunLV < 7)
                        _shotgunLV++;
                }

                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_shotgunLV < 7)
                        _shotgunLV++;
                }
                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _shotgunLV = 1;
                }
                weaponLV = _shotgunLV;
                break;
            case "���� ����":
                weaponPrefab = 3;

                //�̹� ȹ���� ���⿴�ٸ� Lv�� ����
                if (isExist)
                {
                    if (_sniperLV < 7)
                        _sniperLV++;
                }

                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_sniperLV < 7)
                        _sniperLV++;
                }
                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _sniperLV = 1;
                }
                weaponLV = _sniperLV;
                break;
            case "����ī��":
                weaponPrefab = 4;

                //�̹� ȹ���� ���⿴�ٸ� Lv�� ����
                if (isExist)
                {
                    if (_rpgLV < 9)
                        _rpgLV++;
                }

                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_rpgLV < 9)
                        _rpgLV++;
                }
                //���� ȹ������ �ʾҰ� �������� �׼������� ������ LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _rpgLV = 1;
                }
                weaponLV = _rpgLV;
                break;
            case "��ô�� ������":
                weaponPrefab = 6;
                weaponLV = 1;
                break;
            case "Javelin":
                weaponPrefab = 7;
                weaponLV = 1;
                break;
        }

        return (weaponPrefab, weaponLV);
    }

    //ȹ���� ���Ⱑ �̹� �κ��丮�� �����ϴ��� Ȯ��
    private (bool, int) CheckWeapon(Item item)
    {
        bool isExist = false;
        int index = -1;

        if (Weapons == null) return (isExist, index);

        for (int i = 0; i < Weapons.Count; i++)
        {
            //���� ���⸦ �̹� �Ծ��ٸ�
            if (Weapons[i].Name == item.ItemName)
            {
                isExist = true;
                index = i;
                break;
            }
        }

        return (isExist, index);
    }

    //���� �κ��丮���� ������ ���⸦ ã���ִ� �Լ���
    public int FindPistol()
    {
        int index = -1;

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].Name == "����" || Weapons[i].Name == "�������� ��ź��")
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public int FindRifle()
    {
        int index = -1;

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].Name == "���� ����")
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public int FindShotgun()
    {
        int index = -1;

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].Name == "��ź��")
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public int FindSniper()
    {
        int index = -1;

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].Name == "���� ����")
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public int FindRPG()
    {
        int index = -1;

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].Name == "����ī��")
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public void SetOnSwanKnife()
    {
        GameObject weaponPrefab;
        float angle = 90;

        for (int i = 0; i < 30; i++)
        {
            weaponPrefab = Instantiate(_weaponPrefab[8], _weaponPrefabTransform);
            weaponPrefab.transform.rotation = Quaternion.Euler(0, angle, 0);
            _weaponOrbit.weapons3.Add(weaponPrefab);
            angle -= 13f;
        }
    }

    //���� ���� �޼� �� ��ʿ� �ȶ߰� ����
    public void CheckMaxLevel(int weaponNumber)
    {
        Managers.Instance.Game.BannerPresenter.LockItem(weaponNumber);
    }

    //���� ������ �ִ�ġ���� Ȯ��, �ִ�ġ��� ȹ���� ���Ⱑ ���ο� �������� Ȯ��
    public bool CheckMaximumWeapon(Item item)
    {
        bool isPossible = false;
        int count = Weapons.Count;

        if (Weapons.Count < _maximumWeapon)
            isPossible = true;

        if (item.ItemName == "��ô�� ������")
            isPossible = true;

        if (item.ItemName == "Javelin")
            isPossible = true;

        else
        {
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (Weapons[i].Name == item.ItemName)
                    isPossible = true;
                if (Weapons[i].Name == "��ô�� ������")
                    count--;
                if (Weapons[i].Name == "Javelin")
                    count--;
            }
        }

        //��ô ����� �� ���ⰳ�� üũ
        if (count < _maximumWeapon)
            isPossible = true;

        return isPossible;
    }

    //�ִ� ���� ȹ�� ���ɼ�ġ +1
    public void IncreaseMaximumWeaponCount()
    {
        _maximumWeapon++;
    }

    //�ش��ϴ� ���� ����Ʈ, �˵����� ������
    public void DeleteWeapon(Weapon weapon)
    {
        int id, orbitID;
        (id, orbitID) = FindWeapon(weapon);
        Weapons.RemoveAt(id);
        Destroy(_weaponOrbit.weapons[orbitID]);
        _weaponOrbit.weapons.RemoveAt(orbitID);
    }

    private (int,int) FindWeapon(Weapon weapon)
    {
        int id = -1;
        int orbitID = -1;

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].Name == weapon.Name)
            {
                id = i;
                break;
            }
        }

        for(int i = 0; i < _weaponOrbit.weapons.Count; i++)
        {
            if(weapon.Type == _weaponOrbit.weapons[i].GetComponent<Weapon>().Type)
            {
                orbitID = i;
                break;
            }
        }
        return (id, orbitID);
    }

    public void CreateReloverShotgun()
    {
        int index = FindPistol();
        DeleteWeapon(Weapons[index]);

        GameObject weaponPrefab;
        weaponPrefab = Instantiate(_weaponPrefab[5], _weaponPrefabTransform);
        Weapon weapon = weaponPrefab.GetComponent<Weapon>();
        Weapons.Add(weapon);
        weapon.Name = "�������� ��ź��";
        weapon.Number = 0;
        weapon.Description = "������ ���� ���׷��̵� ����";

        //���� ����� �̹� �Ծ�� �׼����� ȿ�� �����ϱ�����
        List<Accessory> findAccessory = new List<Accessory>();
        findAccessory = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon.Name);

        //���⿡ �׼����� ȿ�� ����
        for (int i = 0; i < findAccessory.Count; i++)
        {
            Managers.Instance.Game.AccessoryController.ApplyAccessoryEffect(findAccessory[i], false);
        }

        //�˵��� ���� �߰�
        _weaponOrbit.weapons.Add(weaponPrefab);
    }

    //ó�� ����, ������ �� �ʱ� ���� Init ���ֱ����� ���� ��������
    public Item GetItem(int index)
    {
        Item item = new Item();

        item = Managers.Instance.DataManager.ItemList.ItemList[index];

        return item;
    }
}
