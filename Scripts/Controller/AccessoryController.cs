using Miku.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessoryController : MonoBehaviour
{
    //현재 내가 획득한 모든 액세서리들
    [HideInInspector]
    public List<Accessory> Accessories = new List<Accessory>();

    //전술 교본 획득여부
    private bool _isTextBook = false;

    //액세서리 생성 함수
    public void GenerateAccessory(Item item)
    {
        Accessory accessory = new Accessory();

        accessory.ItemName = item.ItemName;
        accessory.ItemNumber = item.ItemNumber;
        accessory.ItemDescription = item.ItemDescription;
        accessory.AcquireCount = 1;

        if (item.ItemType == "액세서리")
        {
            //만약 무기에 관한 액세서리일 경우 해당 무기에 관한 정보 채워넣기
            if (item.AccessoryWeaponType != null)
            {
                accessory.AccessoryWeaponType = new string[item.AccessoryWeaponType.Length];
                for (int i = 0; i < item.AccessoryWeaponType.Length; i++)
                {
                    accessory.AccessoryWeaponType[i] = item.AccessoryWeaponType[i];
                }
            }

            //이미 획득한 액세서리는 인벤토리에 추가x
            bool isAcquire;
            int index;
            (isAcquire, index) = FindAccessory(item);

            if (isAcquire)
            {
                Accessories[index].AcquireCount++;
            }
            else if (!isAcquire)
            {
                //만약 벅샷이나 플레체 액세서리면 인벤토리에서 둘 중 한개 삭제
                CheckShotgunAccessory(item);

                //액세서리 인벤토리에 추가
                Accessories.Add(accessory);
            }
        }

        ApplyAccessoryEffect(accessory, true);
    }

    //생성된 액세서리 효과 적용
    //true면 현재 획득해져 있는 모든무기 데미지 증가 등 효과
    //false면 마지막으로 획득한 무기만 효과 - 액세서리를 미리 먹어두고 나중에 무기 획득했을 시
    public void ApplyAccessoryEffect(Accessory accessory, bool applyAll)
    {
        switch (accessory.ItemNumber)
        {
            //FMJ - 모든 무기 데미지 2 증가
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
                        Name == "돌격 소총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetRiflePenetration(true);
                    }
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "저격 소총")
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
                        Name == "권총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetPistolPenetration(true);
                    }
                    else if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "산탄총")
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
                        Name == "권총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetPistolPenetration(false);
                    }
                    else if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "산탄총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetShotgunPenetration(false);
                    }
                }
                break;
            //저압 고폭탄
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
            //쵸크1
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
            //도날드 덕
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
            //그레이즈1
            case 15:
                if (applyAll)
                {
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();

                    //스나이퍼 총알 피격범위 증가
                    if (sniper != -1)
                        Managers.Instance.Game.WeaponController.Weapons[sniper].SetSniperRangeUpgrade1();

                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetSniperRangeUpgrade1();
                }
                break;
            //그레이즈2
            case 16:
                if (applyAll)
                {
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();

                    //스나이퍼 총알 피격범위 증가
                    if (sniper != -1)
                        Managers.Instance.Game.WeaponController.Weapons[sniper].SetSniperRangeUpgrade2();

                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetSniperRangeUpgrade2();
                }
                break;
            //페인트 탄
            case 17:
                if (applyAll)
                {
                    int sniper = Managers.Instance.Game.WeaponController.FindSniper();

                    //스나이퍼 총알에 슬로우 부여
                    if (sniper != -1)
                        Managers.Instance.Game.WeaponController.Weapons[sniper].SetSlowSniperBullet();

                }
                else
                {
                    Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetSlowSniperBullet();
                }
                break;
            //아킴보
            case 18:
                if (applyAll)
                {
                    int pistol = Managers.Instance.Game.WeaponController.FindPistol();
                    int rifle = Managers.Instance.Game.WeaponController.FindRifle();

                    //권총, 돌격 소총 총알 2줄기로 설정
                    if (pistol != -1)
                        Managers.Instance.Game.WeaponController.Weapons[pistol].SetTwoBullets();
                    //권총, 돌격 소총 총알 2줄기로 설정
                    if (rifle != -1)
                        Managers.Instance.Game.WeaponController.Weapons[rifle].SetTwoBullets();

                }
                else
                {
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "권총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetTwoBullets();
                    }
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "돌격 소총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        SetTwoBullets();
                    }
                }
                break;
            //소음기
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
                        Name == "권총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetFireInterval(1.5f);
                    }
                    if (Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                        Name == "돌격 소총")
                    {
                        Managers.Instance.Game.WeaponController.Weapons[Managers.Instance.Game.WeaponController.Weapons.Count - 1].
                            SetFireInterval(1.2f);
                    }
                }
                break;
            //소믈리에의 추천
            case 20:
                bool isExist = false;

                //소음기가 있으면
                for (int i = 0; i < Accessories.Count; i++)
                {
                    if (Accessories[i].ItemName == "소음기")
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
            //신속신 LV1
            case 23:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(15f);
                break;
            //신속신 LV2
            case 24:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(15f);
                break;
            //신속신 LV3
            case 25:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(10f);
                break;
            //불제봉
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
            //팔괘로
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
            //회중시계
            case 28:
                Managers.Instance.Game.WeaponController.SetOnSwanKnife();
                Managers.Instance.Game.ZombieSpawnManager.ExecutionFunction();
                Managers.Instance.Game.ZombieController.SetZombieSpeedForAccessory();
                break;
            //부적
            case 29:
                Managers.Instance.Game.Player.PlayerCombat.ContinuouslyRecoverHP();
                break;
            //유령검
            case 30:
                Managers.Instance.Game.Player.PlayerController.PlayerSpeedUp(50f);
                Managers.Instance.Game.Player.PlayerCombat.SetMissDamage();
                break;
            //전술교본
            case 31:
                _isTextBook = true;
                break;
            //전술가방
            case 32:
                Managers.Instance.Game.WeaponController.IncreaseMaximumWeaponCount();
                break;
            //메시아의 스완송
            case 33:
                break;
            //구급상자 소
            case 34:
                Managers.Instance.Game.Player.PlayerCombat.RecoverHP(30);
                break;
            //구급상자 중
            case 35:
                Managers.Instance.Game.Player.PlayerCombat.RecoverHP(70);
                break;
            //구급상자 대
            case 36:
                Managers.Instance.Game.Player.PlayerCombat.RecoverHP(100);
                break;
        }
    }

    //중복되는 액세서리를 리스트에 넣지 않고, LV만 올려주기 위함
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

    //액세서리에서 Flechette와 000 Buckshot은 공존할 수 없기에 삭제
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

    //부활기 액세 있는지 확인
    public bool FindRebornAccessory()
    {
        bool isExist = false;

        for (int i = 0; i < Accessories.Count; i++)
        {
            if (Accessories[i].ItemName == "메시아의 스완송")
            {
                isExist = true;
            }
        }

        return isExist;
    }

    //부활기 액세서리 사용 후 삭제
    public void RemoveRebornAccessory()
    {
        for (int i = 0; i < Accessories.Count; i++)
        {
            if (Accessories[i].ItemName == "메시아의 스완송")
            {
                Accessories.RemoveAt(i);
            }
        }
    }

    //상위 티어 아이템 먹었을 시 하위 티어 아이템 표시 X
    public void CheckHighTierAccessory(Item item)
    {
        //그레이즈 2 먹으면 1 삭제
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
        //신속신 2 먹으면 1 삭제
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
        //신속신 3 먹으면 2 삭제
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

    //무기에 맞는 현재 보유한 액세서리를 찾기위한 코드
    //무기 획득했을 시, 무기를 버릴때 효과를 알아야 할때 2가지 경우에 사용할것으로 보임
    public List<Accessory> FindAccessoryForWeapon(string weaponName)
    {
        List<Accessory> matchingAccessories = new List<Accessory>();
        if (Accessories == null) return matchingAccessories;
        matchingAccessories.Clear();

        //액세서리를 loop 돌면서 해당하는 무기 찾기
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

    //전술 교본 획득했는지 여부 확인
    public bool CheckTextBook()
    {
        return _isTextBook;
    }
}
