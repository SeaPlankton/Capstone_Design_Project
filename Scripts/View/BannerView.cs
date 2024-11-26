using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerView : MonoBehaviour
{
    //��� Panel
    [SerializeField]
    private GameObject _bannerPanel;

    //���� ������ Panel
    [SerializeField]
    private GameObject _changeWeaponPanel;

    //���� ������ panel���� ���� ���� ���� ���� �� ��ŭ panel ǥ��
    [SerializeField]
    private GameObject[] _myWeaponPanel;

    //������ �г� ������ �� �� ǥ���ϱ� ����
    [SerializeField]
    private Image[] _itemPanel;

    //������ �г� ������ �� üũ�ڽ� ǥ��
    [SerializeField]
    private Image[] _itemPanelCheckBox;

    //������ �г��� ������ �̹���
    [SerializeField]
    private Image[] _itemImage;

    //���� ȹ���ϱ�� �� ������ ������
    [SerializeField]
    private Image _choiceWeaponIcon;

    //���� ���� ���� ������ ������
    [SerializeField]
    private Image[] _weaponIcon;
    //���� ���� ���� ������ ������
    [SerializeField]
    private Text[] _weaponDamage;
    //���� ���� ���� ������ ����
    [SerializeField]
    private Text[] _weaponLV;
    //���� ���� ���� ���⿡ ����� �׼�����
    [SerializeField]
    private Text[] _weaponAcceessory;

    //������ ���� �г� ������ �� üũ�ڽ� ǥ��
    [SerializeField]
    private Image[] _ThrowWeaponCheckBox;

    //���⿡ ����� �׼����� ���� �뵵
    private List<Accessory> _accessories = new List<Accessory>();

    //������ ����� �����ϱ� ��ư
    [SerializeField]
    private Button _bannerBtn;
    [SerializeField]
    private Button _throwBannerBtn;

    //������ �̸�
    [SerializeField]
    Text[] _itemName;

    //������ ����
    [SerializeField]
    Text[] _itemDescription;

    //��ʿ��� ���� ������ ���Թ�ȣ   
    private int _selectItem = -1;

    //���� �� �� BannerPanel ON
    public void ShowBannerPanel()
    {
        _bannerPanel.SetActive(true);
        _bannerBtn.interactable = false;
        _bannerPanel.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.6f);

        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Banner);
        Managers.Instance.Game.InputManager.SetCursorState();
        Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.LevelUp);
        Managers.Instance.Game.GameAudio.EffectBgm(true);

    }

    //Banner Panel OFF
    public void CloseBannerPanel()
    {
        _bannerBtn.interactable = false;
        _bannerPanel.SetActive(false);
        _bannerPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 1200f, 0f);

        for (int i = 0; i < _itemPanel.Length; i++)
        {
            _itemPanelCheckBox[i].gameObject.SetActive(false);
        }

        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.None);
        Managers.Instance.Game.InputManager.SetCursorState();
        Managers.Instance.Game.GameAudio.EffectBgm(false);
    }

    public void ShowItem(Item item, int index)
    {
        Managers.Instance.Game.UIController.SetIcon(item.ItemNumber, _itemImage[index]);
        _itemName[index].text = "�̸� : " + item.ItemName;
        _itemDescription[index].text = "���� : " + item.ItemDescription;
    }

    //1 ~ 3 (������, �Ǽ����� ����), 4 (ȸ�� ������ ����)
    public void OnClickSlotBtn(int index)
    {
        for (int i = 0; i < _itemPanel.Length; i++)
        {
            _itemPanelCheckBox[i].gameObject.SetActive(false);
        }

        _itemPanelCheckBox[index].gameObject.SetActive(true);
        _bannerBtn.interactable = true;

        _selectItem = index;
    }

    //���� ������ ������ư ���� ��
    public void OnClickDecisionBtn()
    {
        //������ ������ �ʰ� ������ư ������ return
        if (_selectItem == -1)
            return;

        //���� ���� ������ ȹ��
        Managers.Instance.Game.BannerPresenter.ClickDecision(_selectItem);

        for (int i = 0; i < _itemPanel.Length; i++)
        {
            _itemPanelCheckBox[i].gameObject.SetActive(false);
        }
        _bannerBtn.interactable = false;
        _selectItem = -1;
    }
    /*--------------------------���� ������ Banner ----------------------------------*/

    //���� ������ panel ������ �� ���� ������, ���� �׼����� ���� ǥ��
    public void ShowChangeWeaponPanel(List<Weapon> weapons, Item item)
    {
        _changeWeaponPanel.SetActive(true);
        _changeWeaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        _throwBannerBtn.interactable = false;

        for (int i = 0; i < _myWeaponPanel.Length; i++)
        {
            _ThrowWeaponCheckBox[i].gameObject.SetActive(false);
            _myWeaponPanel[i].SetActive(false);
        }

        Managers.Instance.Game.UIController.SetIcon(item.ItemNumber, _choiceWeaponIcon);
        for (int i = 0; i < weapons.Count; i++)
        {
            _myWeaponPanel[i].SetActive(true);
            Managers.Instance.Game.UIController.SetIcon(weapons[i].Number, _weaponIcon[i]);
            _weaponDamage[i].text = "���� ������ : " + weapons[i].Damage;
            _weaponLV[i].text = "LV." + weapons[i].LV.ToString();
            _accessories = Managers.Instance.Game.BannerPresenter.FindAccessory(weapons[i]);

            for (int j = 0; j < _accessories.Count; j++)
            {
                if (j == 0)
                    _weaponAcceessory[i].text = _accessories[j].ItemName;
                else
                    _weaponAcceessory[i].text = _weaponAcceessory[i].text + ", " + _accessories[j].ItemName;
            }
        }
    }

    //���� ������ ������ Ŭ��������
    public void OnClickItemPanel(int index)
    {
        for (int i = 0; i < _myWeaponPanel.Length; i++)
        {
            _myWeaponPanel[i].GetComponent<Image>().color = new Color(255, 255, 255, 100);
            _ThrowWeaponCheckBox[i].gameObject.SetActive(false);
        }
        Managers.Instance.Game.BannerPresenter.ClickItemPanel(index);
        _myWeaponPanel[index].GetComponent<Image>().color = new Color(255, 255, 0, 100);
        _ThrowWeaponCheckBox[index].gameObject.SetActive(true);
        _throwBannerBtn.interactable = true;
    }

    //������ ��ư ������ ��
    public void OnClickThrowAwayBtn()
    {
        Managers.Instance.Game.BannerPresenter.ClickThrowAway();
        _throwBannerBtn.interactable = false;
        _changeWeaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1300);
        _changeWeaponPanel.SetActive(false);
    }

    public void OnClickBackBtn()
    {
        Managers.Instance.Game.BannerPresenter.ClickItemPanel(-1);
        _throwBannerBtn.interactable = false;
        _changeWeaponPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1300);
        _changeWeaponPanel.SetActive(false);
    }

    /*-------------------------------------------------------------------------------*/
}
