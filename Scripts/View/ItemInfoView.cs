using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoView : MonoBehaviour
{
    public GameObject _itemPanel;

    //내가 가진 아이템 개수 만큼 슬롯을 띄워줄 용도
    [SerializeField]
    private GameObject[] _itemInfoSlot;

    //아이템 아이콘 띄울 용도
    [SerializeField]
    private Image[] _itemIcon;

    //아이템 이름 띄울 용도
    [SerializeField]
    private Text[] _itemName;

    //아이템 레벨 띄울 용도
    [SerializeField]
    private Text[] _itemLV;

    //아이템 설명 띄울 용도
    [SerializeField]
    private Text[] _itemDescription;

    //내가 가진 무기, 액세서리 정보를 화면에 띄우기위해 인벤토리에 가져와서 담을 변수
    private List<Weapon> _weapons = new List<Weapon>();
    private List<Accessory> _accessories = new List<Accessory>();

    //무기 or 액세서리 panel이 열려있는지 체크
    private bool _weaponPanelOn = false;
    private bool _accessoryPanelOn = false;

    //아이템패널 ON
    public void OnClickItemPanel()
    {
        _itemPanel.SetActive(true);
        _itemPanel.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f);
        Managers.Instance.Game.ItemInfoPresenter.CheckMenuPanel();
        Managers.Instance.Game.ItemInfoPresenter.SetItemCamera(true);
        _weaponPanelOn = false;
        _accessoryPanelOn = false;
        OnClickWeaponMenuBtn();
    }

    //무기 패널 왼쪽의 무기 카테고리를 눌렀을 때
    public void OnClickWeaponMenuBtn()
    {
        if (_weaponPanelOn)
            return;

        _weaponPanelOn = true;
        _accessoryPanelOn = false;

        _weapons.Clear();

        for (int i = 0; i < _itemInfoSlot.Length; i++)
        {
            _itemInfoSlot[i].gameObject.SetActive(false);
        }

        Managers.Instance.Game.ItemInfoPresenter.WeaponInformation(_weapons);

        for (int i = 0; i < _weapons.Count; i++)
        {
            _itemInfoSlot[i].SetActive(true);
            Managers.Instance.Game.UIController.SetIcon(_weapons[i].Number, _itemIcon[i]);
            _itemName[i].text = _weapons[i].Name;
            _itemLV[i].text = _weapons[i].LV.ToString();
            _itemDescription[i].text = _weapons[i].Description;
        }
    }

    //무기 패널 왼쪽의 액세서리 카테고리를 눌렀을 때
    public void OnClickAccessoryMenuBtn()
    {
        if (_accessoryPanelOn)
            return;

        _weaponPanelOn = false;
        _accessoryPanelOn = true;

        _accessories.Clear();

        for (int i = 0; i < _itemInfoSlot.Length; i++)
        {
            _itemInfoSlot[i].gameObject.SetActive(false);
        }

        Managers.Instance.Game.ItemInfoPresenter.AccessoryInformation(_accessories);

        Debug.Log(_accessories.Count + " 액세서리 수 확인");

        for (int i = 0; i < _accessories.Count; i++)
        {
            _itemInfoSlot[i].SetActive(true);
            Managers.Instance.Game.UIController.SetIcon(_accessories[i].ItemNumber, _itemIcon[i]);
            _itemName[i].text = _accessories[i].ItemName;
            _itemLV[i].text = _accessories[i].AcquireCount.ToString();
            _itemDescription[i].text = _accessories[i].ItemDescription;
        }
    }

    //아이템패널 OFF
    public void OnClickCloseItemPanel()
    {
        _itemPanel.GetComponent<RectTransform>().DOAnchorPosX(2000f, 0.3f);
        Managers.Instance.Game.ItemInfoPresenter.CheckMenuPanel();
        Managers.Instance.Game.ItemInfoPresenter.SetItemCamera(false);
        _itemPanel.SetActive(false);
    }
}
