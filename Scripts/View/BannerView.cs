using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BannerView : MonoBehaviour
{
    //배너 Panel
    [SerializeField]
    private GameObject _bannerPanel;

    //무기 버리는 Panel
    [SerializeField]
    private GameObject _changeWeaponPanel;

    //무기 버리는 panel에서 현재 내가 가진 무기 수 만큼 panel 표시
    [SerializeField]
    private GameObject[] _myWeaponPanel;

    //아이템 패널 눌렀을 때 색 표시하기 위함
    [SerializeField]
    private Image[] _itemPanel;

    //아이템 패널 눌렀을 때 체크박스 표시
    [SerializeField]
    private Image[] _itemPanelCheckBox;

    //아이템 패널의 아이콘 이미지
    [SerializeField]
    private Image[] _itemImage;

    //내가 획득하기로 한 무기의 아이콘
    [SerializeField]
    private Image _choiceWeaponIcon;

    //현재 내가 가진 무기의 아이콘
    [SerializeField]
    private Image[] _weaponIcon;
    //현재 내가 가진 무기의 데미지
    [SerializeField]
    private Text[] _weaponDamage;
    //현재 내가 가진 무기의 레벨
    [SerializeField]
    private Text[] _weaponLV;
    //현재 내가 가진 무기에 적용된 액세서리
    [SerializeField]
    private Text[] _weaponAcceessory;

    //버리는 무기 패널 눌렀을 때 체크박스 표시
    [SerializeField]
    private Image[] _ThrowWeaponCheckBox;

    //무기에 적용된 액세서리 담을 용도
    private List<Accessory> _accessories = new List<Accessory>();

    //각각의 배너의 결정하기 버튼
    [SerializeField]
    private Button _bannerBtn;
    [SerializeField]
    private Button _throwBannerBtn;

    //아이템 이름
    [SerializeField]
    Text[] _itemName;

    //아이템 설명
    [SerializeField]
    Text[] _itemDescription;

    //배너에서 누른 아이템 슬롯번호   
    private int _selectItem = -1;

    //레벨 업 시 BannerPanel ON
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
        _itemName[index].text = "이름 : " + item.ItemName;
        _itemDescription[index].text = "설명 : " + item.ItemDescription;
    }

    //1 ~ 3 (아이템, 악세서리 슬롯), 4 (회복 아이템 슬롯)
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

    //슬롯 누르고 결정버튼 누를 시
    public void OnClickDecisionBtn()
    {
        //슬롯을 누르지 않고 결정버튼 누르면 return
        if (_selectItem == -1)
            return;

        //누른 슬롯 아이템 획득
        Managers.Instance.Game.BannerPresenter.ClickDecision(_selectItem);

        for (int i = 0; i < _itemPanel.Length; i++)
        {
            _itemPanelCheckBox[i].gameObject.SetActive(false);
        }
        _bannerBtn.interactable = false;
        _selectItem = -1;
    }
    /*--------------------------무기 버리는 Banner ----------------------------------*/

    //무기 버리는 panel 열렸을 때 무기 데미지, 적용 액세서리 정보 표시
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
            _weaponDamage[i].text = "무기 데미지 : " + weapons[i].Damage;
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

    //버릴 무기의 슬롯을 클릭했을때
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

    //버리기 버튼 눌렀을 때
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
