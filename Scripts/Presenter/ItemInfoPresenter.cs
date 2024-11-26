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

    //해상도 설정을 위해 패널 ON/OFF, Dotween이 필요없음
    public void ShowItem()
    {
        _itemInfoView._itemPanel.gameObject.SetActive(true);
    }

    public void HideItem()
    {
        _itemInfoView._itemPanel.gameObject.SetActive(false);
    }

    //ESC로 패널 끌 때 실행
    public void CloseItem()
    {
        _itemInfoView.OnClickCloseItemPanel();
    }

    //메인메뉴에서 아이템 메뉴 눌렀을 때, 카메라 전환
    public void SetItemCamera(bool cameraOn)
    {
        Managers.Instance.Game.CinemachineController.ItemCamera(cameraOn);
    }

    //무기 인벤토리에서 무기 가져옴
    public void WeaponInformation(List<Weapon> weapon)
    {
        for (int i = 0; i < Managers.Instance.Game.WeaponController.Weapons.Count; i++)
        {
            weapon.Add(Managers.Instance.Game.WeaponController.Weapons[i]);
        }
    }

    //액세서리 인벤토리에서 액세서리 가져옴
    public void AccessoryInformation(List<Accessory> accessory)
    {
        for (int i = 0; i < Managers.Instance.Game.AccessoryController.Accessories.Count; i++)
        {
            accessory.Add(Managers.Instance.Game.AccessoryController.Accessories[i]);
        }
    }

    public void CheckMenuPanel()
    {
        //메뉴 열려있으면 메뉴 닫기
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
