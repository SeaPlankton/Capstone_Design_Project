using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfoPresenter : MonoBehaviour
{
    [SerializeField]
    private ItemInfoView _itemInfoView;

    [SerializeField]
    private Animator _animator;

    protected static readonly int ItemMenuHash = Animator.StringToHash("Peek And Aim");

    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }

    private void Init()
    {
        Managers.Instance.Game.ItemInfoPresenter = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }

    //�ػ� ������ ���� �г� ON/OFF, Dotween�� �ʿ����
    public void ShowItem()
    {
        _itemInfoView._itemPanel.gameObject.SetActive(true);
    }

    public void HideItem()
    {
        _itemInfoView._itemPanel.gameObject.SetActive(false);
    }

    //ESC�� �г� �� �� ����
    public void CloseItem()
    {
        _itemInfoView.OnClickCloseItemPanel();
    }

    //���θ޴����� ������ �޴� ������ ��, ī�޶� ��ȯ
    public void SetItemCamera(bool cameraOn)
    {
        Managers.Instance.Game.CinemachineController.ItemCamera(cameraOn);
    }

    //���� �κ��丮���� ���� ������
    public void WeaponInformation(List<Weapon> weapon)
    {
        for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
        {
            weapon.Add(Managers.Instance.Game.WeaponController.Weapons[i]);
        }
    }

    //�׼����� �κ��丮���� �׼����� ������
    public void AccessoryInformation(List<Accessory> accessory)
    {
        for (int i = 0; i < Managers.Instance.Game.AccessoryController.Accessories.Count; i++)
        {
            accessory.Add(Managers.Instance.Game.AccessoryController.Accessories[i]);
        }
    }

    public void CheckMenuPanel()
    {
        //�޴� ���������� �޴� �ݱ�
        if (Managers.Instance.Game.UIController.IsMenuPanelOn)
        {
            Managers.Instance.Game.UIController.HideMenuPanel();
            Managers.Instance.Game.UIController.IsMenuPanelOn = false;
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Item);

            Managers.Instance.Game.UIController.currentEmotionStates = UIController.UIEmotionStates.Menu1;
            //_animator.Play("MenuEmotion1");
            _animator.CrossFade(ItemMenuHash, 0.05f);
        }
        else if (!Managers.Instance.Game.UIController.IsMenuPanelOn)
        {
            Managers.Instance.Game.UIController.ShowMenuPanel();
            Managers.Instance.Game.UIController.IsMenuPanelOn = true;
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Menu);
            Managers.Instance.Game.UIController.currentEmotionStates = UIController.UIEmotionStates.None;
            _animator.Play("Aiming Gun");
        }
    }
}
