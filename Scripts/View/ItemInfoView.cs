using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoView : MonoBehaviour
{
    public GameObject _itemPanel;

    //���� ���� ������ ���� ��ŭ ������ ����� �뵵
    [SerializeField]
    private GameObject[] _itemInfoSlot;

    //������ ������ ��� �뵵
    [SerializeField]
    private Image[] _itemIcon;

    //������ �̸� ��� �뵵
    [SerializeField]
    private Text[] _itemName;

    //������ ���� ��� �뵵
    [SerializeField]
    private Text[] _itemLV;

    //������ ���� ��� �뵵
    [SerializeField]
    private Text[] _itemDescription;

    //���� ���� ����, �׼����� ������ ȭ�鿡 �������� �κ��丮�� �����ͼ� ���� ����
    private List<Weapon> _weapons = new List<Weapon>();
    private List<Accessory> _accessories = new List<Accessory>();

    //���� or �׼����� panel�� �����ִ��� üũ
    private bool _weaponPanelOn = false;
    private bool _accessoryPanelOn = false;

    //�������г� ON
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

    //���� �г� ������ ���� ī�װ��� ������ ��
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

    //���� �г� ������ �׼����� ī�װ��� ������ ��
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

        Debug.Log(_accessories.Count + " �׼����� �� Ȯ��");

        for (int i = 0; i < _accessories.Count; i++)
        {
            _itemInfoSlot[i].SetActive(true);
            Managers.Instance.Game.UIController.SetIcon(_accessories[i].ItemNumber, _itemIcon[i]);
            _itemName[i].text = _accessories[i].ItemName;
            _itemLV[i].text = _accessories[i].AcquireCount.ToString();
            _itemDescription[i].text = _accessories[i].ItemDescription;
        }
    }

    //�������г� OFF
    public void OnClickCloseItemPanel()
    {
        _itemPanel.GetComponent<RectTransform>().DOAnchorPosX(2000f, 0.3f);
        Managers.Instance.Game.ItemInfoPresenter.CheckMenuPanel();
        Managers.Instance.Game.ItemInfoPresenter.SetItemCamera(false);
        _itemPanel.SetActive(false);
    }
}
