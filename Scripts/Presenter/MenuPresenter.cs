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


    //메뉴가 열렸는가 안열렸는가 설정
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
    
    //모바일에서 메뉴열렸을때 조이스틱 ON/OFF
    public void UpdateJoyStick(bool isUIOn)
    {
#if(UNITY_ANDROID)
        Managers.Instance.Game.UIController.JoyStick.gameObject.SetActive(isUIOn);
#endif
    }

    //메뉴 패널 닫기 -> 게임화면
    public void CloseMenu()
    {
        _menuView.ClickCloseMenuBtn();
    }

    //ESC가 아닌 버튼으로 눌렀을 때 Time 설정해주기 위함...
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

    //메뉴 닫고 퀵메뉴, 사이드메뉴 ON
    public void HideMenuPanel()
    {
        _menuView.CloseMenu();
    }

    //메뉴 열고 퀵메뉴, 사이드메뉴 OFF
    public void ShowMenuPanel()
    {
        _menuView.ClickMenuBtn();
    }

    public void SetMainCamera(bool cameraOn)
    {
        Managers.Instance.Game.CinemachineController.MainCamera(cameraOn);
    }

    // 점프할때 esc 누르면 메뉴 애니메이션이 무시되어 Invoke사용해서 아래 함수 실행 by. 최성훈
    public void PlayMenuAnimation()
    {
        int EmotionMenuHash = Animator.StringToHash("Aiming Gun");
        _animator.CrossFade(EmotionMenuHash, 0.05f);
        _leftPistol.SetActive(true);
    }
}
