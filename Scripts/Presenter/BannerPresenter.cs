using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerPresenter : MonoBehaviour
{
    [SerializeField]
    private BannerView _bannerView;
    [SerializeField]
    private Draw _draw;

    //누적된 레벨업 횟수
    private int _accumulatedLevelUp = 0;

    //레벨에 따라 배너에 띄울 아이템이 다르게 나와야 해서 여기서 따로 한번 더 계산..
    private int _playerLevel = 0;

    private Item _item;

    //Banner에 띄울 아이템 리스트
    private List<Item> _showItem = new List<Item>();

    //Banner에서 골랐던 아이템 인덱스 저장
    private int _choiceIndex = -1;

    //버리기 Banner에서 선택한 무기 인덱스 저장
    private int _throwAwayIndex = -1;

    //무기에 맞는 액세서리 담을 용도
    private List<Accessory> _accessories = new List<Accessory>();

    //현재 버릴 후보군 무기 리스트
    private List<Weapon> _throwWeaponList = new List<Weapon>();

    //메시아 액세서리가 발동 중인가?
    [HideInInspector]
    public bool IsGoingDie = false;

    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }


    private void Init()
    {
        Managers.Instance.Game.BannerPresenter = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }

    //무기 or 액세서리에 맞게 class 생성요청
    public void ClickDecision(int index)
    {
        _choiceIndex = index;
        //획득한 아이템 획득 처리
        _draw.SelectItem(_showItem[_choiceIndex].ItemNumber);

        if (_showItem[_choiceIndex].ItemType == "무기")
        {
            //획득해있는 무기숫자가 최대치인지 판단
            bool check = Managers.Instance.Game.WeaponController.CheckMaximumWeapon(_showItem[_choiceIndex]);
            if (check)
            {
                Managers.Instance.Game.WeaponController.GenerateWeapon(_showItem[_choiceIndex]);
                CheckLeftLevelUp();
            }
            //아이템 버리는 Banner 띄우기
            else
            {
                ShowWeaponChangeBanner();
            }
        }
        else if (_showItem[_choiceIndex].ItemType == "액세서리")
        {
            Managers.Instance.Game.AccessoryController.GenerateAccessory(_showItem[_choiceIndex]);
            Managers.Instance.Game.AccessoryController.CheckHighTierAccessory(_showItem[_choiceIndex]);
            CheckLeftLevelUp();
        }
        //회복아이템
        else
        {
            Managers.Instance.Game.AccessoryController.GenerateAccessory(_showItem[_choiceIndex]);
            CheckLeftLevelUp();
        }
    }

    public void LevelUp()
    {
        _accumulatedLevelUp++;

        //여러번 레벨업 해도 한번에 배너 1번만 뜨게하기위함
        if (_accumulatedLevelUp == 1)
        {
            Managers.Instance.Game.TimeController.StopGame();
            ShowBanner();
        }
    }

    //view에서 아이템 선택 후 남은 레벨업 확인
    public void CheckLeftLevelUp()
    {
        _accumulatedLevelUp--;
        ClearItemList();

        //누적된 레벨업이 있을경우 한번 더 배너 띄우기
        if (_accumulatedLevelUp > 0)
        {
            ShowBanner();
        }
        else
        {
            _bannerView.CloseBannerPanel();
            Managers.Instance.Game.TimeController.ResumeGame();
            if (IsGoingDie)
            {
                Managers.Instance.Game.DieUIPresenter.OnMessiaPanel();
            }
        }
    }

    //레벨업 시 Banner Panel 여는 함수 호출
    public void ShowBanner()
    {
        _playerLevel++;
        _bannerView.ShowBannerPanel();
        DrawItem();
        DrawHealItem();
        if (IsGoingDie)
        {
            Managers.Instance.Game.DieUIPresenter.OffMessiaPanel();
        }
    }

    //무기 버리는 panel 열었을 때 현재 가진 무기 출력
    public void ShowWeaponChangeBanner()
    {
        _throwAwayIndex = -1;
        _throwWeaponList.Clear();

        for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
        {
            if (Managers.Instance.Game.WeaponController.Weapons[i].Name != "투척용 나이프" &&
                Managers.Instance.Game.WeaponController.Weapons[i].Name != "Javelin")
            {
                _throwWeaponList.Add(Managers.Instance.Game.WeaponController.Weapons[i]);
            }
        }
        _bannerView.ShowChangeWeaponPanel(_throwWeaponList, _showItem[_choiceIndex]);
    }

    //클릭한 버릴 무기 슬롯 저장
    public void ClickItemPanel(int index)
    {
        _throwAwayIndex = index;
    }

    //버리기 버튼 눌렸을 때 클릭한 무기 버리고 새로운 무기 획득, 남은 레벨 체크
    public void ClickThrowAway()
    {
        if (_throwAwayIndex == -1)
            return;

        //최대레벨 무기를 버렸을 경우 배너에 다시 뜨게해줘야해서 넣어둠 - 귀찮아서 버릴때마다 작동
        UnLockItem(_throwWeaponList[_throwAwayIndex].Number);
        Managers.Instance.Game.WeaponController.DeleteWeapon(_throwWeaponList[_throwAwayIndex]);
        Managers.Instance.Game.WeaponController.GenerateWeapon(_showItem[_choiceIndex]);
        CheckLeftLevelUp();
    }

    //무기에 적용된 액세서리를 찾아줌
    public List<Accessory> FindAccessory(Weapon weapon)
    {
        _accessories.Clear();
        _accessories = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon.Name);

        return _accessories;
    }

    //배너 선택하면 저장해놨던 리스트 비우기
    public void ClearItemList()
    {
        _showItem.Clear();
        _choiceIndex = -1;
    }

    //LV이 10 이하일 경우 일반 아이템만 3개
    //Lv이 10 이상일 경우 일반2개 + 희귀포함 아이템1개
    private void DrawItem()
    {
        //LV10 이하일 경우
        if (_playerLevel < 10)
        {
            while (true)
            {
                if (_showItem.Count == 3)
                    break;

                _item = _draw.DrawNormalItem();
                if (CheckItem(_item))
                    _showItem.Add(_item);
            }
        }

        //LV10 이상일 경우 3번째 칸에는 확률에 따라 고티어 아이템 등장
        else if (_playerLevel >= 10)
        {
            int increasePercent = DiceRareItem();
            int percent = Random.Range(1, 101);

            while (true)
            {
                if (_showItem.Count == 2)
                    break;

                _item = _draw.DrawNormalItem();
                if (CheckItem(_item))
                    _showItem.Add(_item);
            }
            //확률에 따라 고티어 아이템 등장
            while (true)
            {
                if (_showItem.Count == 3)
                    break;

                //고티어 뽑기
                if (percent > 100 - increasePercent)
                {
                    _item = _draw.DrawRareItem();
                    if (CheckItem(_item))
                        _showItem.Add(_item);
                }
                //일반 뽑기
                else if (percent <= 100 - increasePercent)
                {
                    _item = _draw.DrawNormalItem();
                    if (CheckItem(_item))
                        _showItem.Add(_item);
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            _bannerView.ShowItem(_showItem[i], i);
        }
    }

    private void DrawHealItem()
    {
        _showItem.Add(_draw.DrawHealingPack());
        _bannerView.ShowItem(_showItem[3], 3);
    }

    //배너에 중복되는 아이템 제외하기위함
    private bool CheckItem(Item item)
    {
        if (_showItem.Count > 0)
        {
            for (int i = 0; i < _showItem.Count; i++)
            {
                if (_showItem[i].ItemNumber == item.ItemNumber)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //특정 확률 이상이 나오면 고티어 아이템 등장 
    private int DiceRareItem()
    {
        int setPercent, rarePercent = 10;

        if (_playerLevel > 35)
        {
            rarePercent = 25;
        }
        else if (_playerLevel <= 35)
        {
            setPercent = (_playerLevel / 5) - 2;
            setPercent = 3 * setPercent;

            rarePercent += setPercent;
        }

        return rarePercent;
    }

    //무기 아이템 최대레벨 달성 시 안뜨게
    public void LockItem(int number)
    {
        _draw.LockMaxLevelItem(number);
    }

    //무기 아이템 최대레벨 달성 시 안뜨게
    public void UnLockItem(int number)
    {
        _draw.UnLockMaxLevelItem(number);
    }
}
