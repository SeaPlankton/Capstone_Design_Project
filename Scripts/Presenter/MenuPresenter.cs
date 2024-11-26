using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPresenter : MonoBehaviour
{
    [SerializeField]
    private MenuView _menuView;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private GameObject _leftPistol;

    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }

    private void Start()
    {
        _menuView = _menuView.GetComponent<MenuView>();
    }

    private void Init()
    {
        Managers.Instance.Game.MenuPresenter = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }


    //�޴��� ���ȴ°� �ȿ��ȴ°� ����
    public void SetMenuState(bool isMenuOn)
    {
        Managers.Instance.Game.UIController.IsMenuPanelOn = isMenuOn;
        if (isMenuOn)
        {
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Menu);
            Managers.Instance.Game.InputManager.CursorLocked = false;

            if (Managers.Instance.Game.Player.PlayerController.Grounded == false)
            {
                Invoke("PlayMenuAnimation", 0.5f);
            }
            else
            {
                _animator.Play("Aiming Gun");
                _leftPistol.SetActive(true);
            }
        }
        else if (!isMenuOn)
        {
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.None);
            Managers.Instance.Game.InputManager.CursorLocked = true;
            _animator.Play("Locomotion");
            _leftPistol.SetActive(false);
        }
    }
    
    //����Ͽ��� �޴��������� ���̽�ƽ ON/OFF
    public void UpdateJoyStick(bool isUIOn)
    {
#if(UNITY_ANDROID)
        Managers.Instance.Game.UIController.JoyStick.gameObject.SetActive(isUIOn);
#endif
    }

    //�޴� �г� �ݱ� -> ����ȭ��
    public void CloseMenu()
    {
        _menuView.ClickCloseMenuBtn();
    }

    //ESC�� �ƴ� ��ư���� ������ �� Time �������ֱ� ����...
    public void OnMenu()
    {
        Managers.Instance.Game.TimeController.StopGame();
        _menuView.ClickMenuBtn();
    }

    public void OffMenu()
    {
        Managers.Instance.Game.TimeController.ResumeGame();
        _menuView.ClickCloseMenuBtn();
        if (Managers.Instance.Game.UIController.IsGoingDie)
        {
            Managers.Instance.Game.DieUIPresenter.OnMessiaPanel();
        }
    }

    //�޴� �ݰ� ���޴�, ���̵�޴� ON
    public void HideMenuPanel()
    {
        _menuView.CloseMenu();
    }

    //�޴� ���� ���޴�, ���̵�޴� OFF
    public void ShowMenuPanel()
    {
        _menuView.ClickMenuBtn();
    }

    public void SetMainCamera(bool cameraOn)
    {
        Managers.Instance.Game.CinemachineController.MainCamera(cameraOn);
    }

    // �����Ҷ� esc ������ �޴� �ִϸ��̼��� ���õǾ� Invoke����ؼ� �Ʒ� �Լ� ���� by. �ּ���
    public void PlayMenuAnimation()
    {
        int EmotionMenuHash = Animator.StringToHash("Aiming Gun");
        _animator.CrossFade(EmotionMenuHash, 0.05f);
        _leftPistol.SetActive(true);
    }
}
