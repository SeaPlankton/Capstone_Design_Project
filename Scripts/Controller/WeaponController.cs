using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //내가 가지고있는 무기들
    [HideInInspector]
    public List<Weapon> Weapons;

    //무기 프리팹들
    //0 - 권총, 1 - 라이플, 2 - 샷건, 3 - 스나이퍼, 4 - 바주카포
    [SerializeField]
    private GameObject[] _weaponPrefab;

    //캐릭터 주위로 회전하는 궤도에 무기 넣기위함
    [SerializeField]
    private WeaponOrbit _weaponOrbit;

    //무기 프리팹 하이어라키창에 생성될 위치
    [SerializeField]
    private Transform _weaponPrefabTransform;

    //획득 가능한 무기 최대치
    private int _maximumWeapon = 2;

    //전술 교본을 먹었을 때를 대비해 무기를 버려도 LV을 저장하기 위함
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

    //처음 권총이랑 라이플만
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

        //무기 만들고 이미 먹어둔 액세서리 효과 적용하기위함
        List<Accessory> findAccessory = new List<Accessory>();
        findAccessory = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon1.Name);

        //무기에 액세서리 효과 적용
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

        //이미 있는 무기라면 프리팹 생성X, LV++
        if (isExist)
        {
            Weapons[index].WeaponLevelUp(weaponLV, false);
            Weapons[index].LV = weaponLV;
        }
        //없는 무기라면 프리팹 생성, LV1로 생성
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
                weapon.Name = "리볼버형 산탄총";

            //투척용 무기 먹었으면 배너에 안뜨기
            if (weaponPrefabCount == 6)
                Managers.Instance.Game.BannerPresenter.LockItem(weapon.Number);

            //Javelin 무기 먹었으면 배너에 안뜨기
            if (weaponPrefabCount == 7)
                Managers.Instance.Game.BannerPresenter.LockItem(weapon.Number);

            //전술 교본 있으면 전에 먹은 레벨 + 1 까지 능력치 조정
            if (Managers.Instance.Game.AccessoryController.CheckTextBook() && weaponPrefabCount != 5 && weaponPrefabCount != 6)
            {
                weapon.WeaponLevelUp(weaponLV, true);
            }

            //무기 만들고 이미 먹어둔 액세서리 효과 적용하기위함
            List<Accessory> findAccessory = new List<Accessory>();
            findAccessory = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon.Name);

            //무기에 액세서리 효과 적용
            for (int i = 0; i < findAccessory.Count; i++)
            {
                Managers.Instance.Game.AccessoryController.ApplyAccessoryEffect(findAccessory[i], false);
            }

            //투척무기의 경우 별도의 궤도에 담아두기
            if (weaponPrefabCount == 6 || weaponPrefabCount == 7)
            {
                _weaponOrbit.weapons2.Add(weaponPrefab);
            }
            else
            {
                //궤도에 무기 추가
                _weaponOrbit.weapons.Add(weaponPrefab);
            }
        }
    }

    //획득한 아이템 타입 확인
    //획득 여부에 따라 LV 처리후 리턴
    private (int, int) CheckWeaponType(Item item, bool isExist)
    {
        int weaponPrefab = -1;
        int weaponLV = -1;

        switch (item.ItemName)
        {
            case "권총":
                weaponPrefab = 0;

                //이미 획득한 무기였다면 Lv만 증가
                if (isExist)
                {
                    if (_pistolLV < 7)
                        _pistolLV++;

                    //만렙이 됐을 경우 리볼버형 산탄총으로 프리팹 생성
                    if (_pistolLV == 7)
                        weaponPrefab = 5;
                }

                //현재 획득하지 않았고 전술교본 액세서리가 있으면 LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_pistolLV < 7)
                        _pistolLV++;

                    //만렙이 됐을 경우 리볼버형 산탄총으로 프리팹 생성
                    if (_pistolLV == 7)
                        weaponPrefab = 5;
                }
                //현재 획득하지 않았고 전술교본 액세서리가 없으면 LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _pistolLV = 1;
                }
                weaponLV = _pistolLV;
                break;
            case "돌격 소총":
                weaponPrefab = 1;

                if (isExist)
                {
                    if (_rifleLV < 7)
                        _rifleLV++;
                }

                //현재 획득하지 않았고 전술교본 액세서리가 있으면 LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_rifleLV < 7)
                        _rifleLV++;
                }
                //현재 획득하지 않았고 전술교본 액세서리가 없으면 LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _rifleLV = 1;
                }
                weaponLV = _rifleLV;
                break;
            case "산탄총":
                weaponPrefab = 2;

                //이미 획득한 무기였다면 Lv만 증가
                if (isExist)
                {
                    if (_shotgunLV < 7)
                        _shotgunLV++;
                }

                //현재 획득하지 않았고 전술교본 액세서리가 있으면 LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_shotgunLV < 7)
                        _shotgunLV++;
                }
                //현재 획득하지 않았고 전술교본 액세서리가 없으면 LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _shotgunLV = 1;
                }
                weaponLV = _shotgunLV;
                break;
            case "저격 소총":
                weaponPrefab = 3;

                //이미 획득한 무기였다면 Lv만 증가
                if (isExist)
                {
                    if (_sniperLV < 7)
                        _sniperLV++;
                }

                //현재 획득하지 않았고 전술교본 액세서리가 있으면 LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_sniperLV < 7)
                        _sniperLV++;
                }
                //현재 획득하지 않았고 전술교본 액세서리가 없으면 LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _sniperLV = 1;
                }
                weaponLV = _sniperLV;
                break;
            case "바주카포":
                weaponPrefab = 4;

                //이미 획득한 무기였다면 Lv만 증가
                if (isExist)
                {
                    if (_rpgLV < 9)
                        _rpgLV++;
                }

                //현재 획득하지 않았고 전술교본 액세서리가 있으면 LV++
                else if (!isExist && Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    if (_rpgLV < 9)
                        _rpgLV++;
                }
                //현재 획득하지 않았고 전술교본 액세서리가 없으면 LV = 1
                else if (!isExist && !Managers.Instance.Game.AccessoryController.CheckTextBook())
                {
                    _rpgLV = 1;
                }
                weaponLV = _rpgLV;
                break;
            case "투척용 나이프":
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

    //획득한 무기가 이미 인벤토리에 존재하는지 확인
    private (bool, int) CheckWeapon(Item item)
    {
        bool isExist = false;
        int index = -1;

        if (Weapons == null) return (isExist, index);

        for (int i = 0; i < Weapons.Count; i++)
        {
            //만약 무기를 이미 먹었다면
            if (Weapons[i].Name == item.ItemName)
            {
                isExist = true;
                index = i;
                break;
            }
        }

        return (isExist, index);
    }

    //무기 인벤토리에서 각각의 무기를 찾아주는 함수들
    public int FindPistol()
    {
        int index = -1;

        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].Name == "권총" || Weapons[i].Name == "리볼버형 산탄총")
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
            if (Weapons[i].Name == "돌격 소총")
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
            if (Weapons[i].Name == "산탄총")
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
            if (Weapons[i].Name == "저격 소총")
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
            if (Weapons[i].Name == "바주카포")
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

    //무기 만렙 달성 시 배너에 안뜨게 설정
    public void CheckMaxLevel(int weaponNumber)
    {
        Managers.Instance.Game.BannerPresenter.LockItem(weaponNumber);
    }

    //무기 개수가 최대치인지 확인, 최대치라면 획득한 무기가 새로운 무기인지 확인
    public bool CheckMaximumWeapon(Item item)
    {
        bool isPossible = false;
        int count = Weapons.Count;

        if (Weapons.Count < _maximumWeapon)
            isPossible = true;

        if (item.ItemName == "투척용 나이프")
            isPossible = true;

        if (item.ItemName == "Javelin")
            isPossible = true;

        else
        {
            for (int i = 0; i < Weapons.Count; i++)
            {
                if (Weapons[i].Name == item.ItemName)
                    isPossible = true;
                if (Weapons[i].Name == "투척용 나이프")
                    count--;
                if (Weapons[i].Name == "Javelin")
                    count--;
            }
        }

        //투척 무기들 뺀 무기개수 체크
        if (count < _maximumWeapon)
            isPossible = true;

        return isPossible;
    }

    //최대 무기 획득 가능수치 +1
    public void IncreaseMaximumWeaponCount()
    {
        _maximumWeapon++;
    }

    //해당하는 무기 리스트, 궤도에서 버리기
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
        weapon.Name = "리볼버형 산탄총";
        weapon.Number = 0;
        weapon.Description = "권총의 최종 업그레이드 형태";

        //무기 만들고 이미 먹어둔 액세서리 효과 적용하기위함
        List<Accessory> findAccessory = new List<Accessory>();
        findAccessory = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon.Name);

        //무기에 액세서리 효과 적용
        for (int i = 0; i < findAccessory.Count; i++)
        {
            Managers.Instance.Game.AccessoryController.ApplyAccessoryEffect(findAccessory[i], false);
        }

        //궤도에 무기 추가
        _weaponOrbit.weapons.Add(weaponPrefab);
    }

    //처음 권총, 라이플 등 초기 무기 Init 해주기위해 정보 가져오기
    public Item GetItem(int index)
    {
        Item item = new Item();

        item = Managers.Instance.DataManager.ItemList.ItemList[index];

        return item;
    }
}
