using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerPresenter : MonoBehaviour
{
    [SerializeField]
    private BannerView _bannerView;
    [SerializeField]
    private Draw _draw;

    //������ ������ Ƚ��
    private int _accumulatedLevelUp = 0;

    //������ ���� ��ʿ� ��� �������� �ٸ��� ���;� �ؼ� ���⼭ ���� �ѹ� �� ���..
    private int _playerLevel = 0;

    private Item _item;

    //Banner�� ��� ������ ����Ʈ
    private List<Item> _showItem = new List<Item>();

    //Banner���� ����� ������ �ε��� ����
    private int _choiceIndex = -1;

    //������ Banner���� ������ ���� �ε��� ����
    private int _throwAwayIndex = -1;

    //���⿡ �´� �׼����� ���� �뵵
    private List<Accessory> _accessories = new List<Accessory>();

    //���� ���� �ĺ��� ���� ����Ʈ
    private List<Weapon> _throwWeaponList = new List<Weapon>();

    //�޽þ� �׼������� �ߵ� ���ΰ�?
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

    //���� or �׼������� �°� class ������û
    public void ClickDecision(int index)
    {
        _choiceIndex = index;
        //ȹ���� ������ ȹ�� ó��
        _draw.SelectItem(_showItem[_choiceIndex].ItemNumber);

        if (_showItem[_choiceIndex].ItemType == "����")
        {
            //ȹ�����ִ� ������ڰ� �ִ�ġ���� �Ǵ�
            bool check = Managers.Instance.Game.WeaponController.CheckMaximumWeapon(_showItem[_choiceIndex]);
            if (check)
            {
                Managers.Instance.Game.WeaponController.GenerateWeapon(_showItem[_choiceIndex]);
                CheckLeftLevelUp();
            }
            //������ ������ Banner ����
            else
            {
                ShowWeaponChangeBanner();
            }
        }
        else if (_showItem[_choiceIndex].ItemType == "�׼�����")
        {
            Managers.Instance.Game.AccessoryController.GenerateAccessory(_showItem[_choiceIndex]);
            Managers.Instance.Game.AccessoryController.CheckHighTierAccessory(_showItem[_choiceIndex]);
            CheckLeftLevelUp();
        }
        //ȸ��������
        else
        {
            Managers.Instance.Game.AccessoryController.GenerateAccessory(_showItem[_choiceIndex]);
            CheckLeftLevelUp();
        }
    }

    public void LevelUp()
    {
        _accumulatedLevelUp++;

        //������ ������ �ص� �ѹ��� ��� 1���� �߰��ϱ�����
        if (_accumulatedLevelUp == 1)
        {
            Managers.Instance.Game.TimeController.StopGame();
            ShowBanner();
        }
    }

    //view���� ������ ���� �� ���� ������ Ȯ��
    public void CheckLeftLevelUp()
    {
        _accumulatedLevelUp--;
        ClearItemList();

        //������ �������� ������� �ѹ� �� ��� ����
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

    //������ �� Banner Panel ���� �Լ� ȣ��
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

    //���� ������ panel ������ �� ���� ���� ���� ���
    public void ShowWeaponChangeBanner()
    {
        _throwAwayIndex = -1;
        _throwWeaponList.Clear();

        for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
        {
            if (Managers.Instance.Game.WeaponController.Weapons[i].Name != "��ô�� ������" &&
                Managers.Instance.Game.WeaponController.Weapons[i].Name != "Javelin")
            {
                _throwWeaponList.Add(Managers.Instance.Game.WeaponController.Weapons[i]);
            }
        }
        _bannerView.ShowChangeWeaponPanel(_throwWeaponList, _showItem[_choiceIndex]);
    }

    //Ŭ���� ���� ���� ���� ����
    public void ClickItemPanel(int index)
    {
        _throwAwayIndex = index;
    }

    //������ ��ư ������ �� Ŭ���� ���� ������ ���ο� ���� ȹ��, ���� ���� üũ
    public void ClickThrowAway()
    {
        if (_throwAwayIndex == -1)
            return;

        //�ִ뷹�� ���⸦ ������ ��� ��ʿ� �ٽ� �߰�������ؼ� �־�� - �����Ƽ� ���������� �۵�
        UnLockItem(_throwWeaponList[_throwAwayIndex].Number);
        Managers.Instance.Game.WeaponController.DeleteWeapon(_throwWeaponList[_throwAwayIndex]);
        Managers.Instance.Game.WeaponController.GenerateWeapon(_showItem[_choiceIndex]);
        CheckLeftLevelUp();
    }

    //���⿡ ����� �׼������� ã����
    public List<Accessory> FindAccessory(Weapon weapon)
    {
        _accessories.Clear();
        _accessories = Managers.Instance.Game.AccessoryController.FindAccessoryForWeapon(weapon.Name);

        return _accessories;
    }

    //��� �����ϸ� �����س��� ����Ʈ ����
    public void ClearItemList()
    {
        _showItem.Clear();
        _choiceIndex = -1;
    }

    //LV�� 10 ������ ��� �Ϲ� �����۸� 3��
    //Lv�� 10 �̻��� ��� �Ϲ�2�� + ������� ������1��
    private void DrawItem()
    {
        //LV10 ������ ���
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

        //LV10 �̻��� ��� 3��° ĭ���� Ȯ���� ���� ��Ƽ�� ������ ����
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
            //Ȯ���� ���� ��Ƽ�� ������ ����
            while (true)
            {
                if (_showItem.Count == 3)
                    break;

                //��Ƽ�� �̱�
                if (percent > 100 - increasePercent)
                {
                    _item = _draw.DrawRareItem();
                    if (CheckItem(_item))
                        _showItem.Add(_item);
                }
                //�Ϲ� �̱�
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

    //��ʿ� �ߺ��Ǵ� ������ �����ϱ�����
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

    //Ư�� Ȯ�� �̻��� ������ ��Ƽ�� ������ ���� 
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

    //���� ������ �ִ뷹�� �޼� �� �ȶ߰�
    public void LockItem(int number)
    {
        _draw.LockMaxLevelItem(number);
    }

    //���� ������ �ִ뷹�� �޼� �� �ȶ߰�
    public void UnLockItem(int number)
    {
        _draw.UnLockMaxLevelItem(number);
    }
}
