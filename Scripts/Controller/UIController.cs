using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //어떤 UI가 열려있는지 체크
    public enum UIStates
    {
        None,
        Banner,
        Menu,
        Item,
        Emotion,
        Option,
        Dead,
        Victory
    } 

    //누른 메뉴, 버튼에따라 각각의 감정표현 실행
    public enum UIEmotionStates
    {
        None,
        Menu1,
        Menu2,
        Button1,
        Button2
    }

    //현재 어떤 UI가 열려있는지 체크
    [SerializeField]
    private UIStates currentStates;

    //현재 어떤 메뉴, 버튼을 눌러서 어떤 감정표현인지
    public UIEmotionStates currentEmotionStates;

    [HideInInspector]
    //메뉴 창이 열렸는가? - MenuView
    public bool IsMenuPanelOn = false;

    [HideInInspector]
    //Panel 열렸을때 화면이동 안되게 하기위함
    public bool IsUIOn = false;

    //모바일용 조이스틱 ON/OFF 관리
    public GameObject JoyStick;

    //메시아 액세서리가 발동 중인가?
    [HideInInspector]
    public bool IsGoingDie = false;

    private void Awake()
    {
#if (UNITY_ANDROID) && !((UNITY_EDITOR) || (UNITY_STANDALONE_WIN))
        JoyStick.gameObject.SetActive(true);
#endif
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }

    public UIStates GetCurrentUIState()
    {
        return currentStates;
    }

    public void SetCurrentUIState(UIStates uIState)
    {
        if (currentStates != uIState)
        {
            currentStates = uIState;
            if (uIState == UIStates.None)
            {
                Managers.Instance.Game.InputManager.UnlockKeyBoard();
            } else
            {
                Managers.Instance.Game.InputManager.LockKeyBoard();
            }
            Managers.Instance.Game.InputManager.SetCursorState();
        }
    }

    private void Init()
    {
        Managers.Instance.Game.UIController = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }

    //Menu Panel 닫기
    public void HideMenuPanel()
    {     
        Managers.Instance.Game.MenuPresenter.HideMenuPanel();
    }

    //Menu Panel 열기
    public void ShowMenuPanel()
    {
        Managers.Instance.Game.MenuPresenter.ShowMenuPanel();       
    }

    //해상도 설정을 위해 모든 패널들은 한번이라도 켜졌어야 변경이된다.
    public void ShowAllPanel()
    {
        Managers.Instance.Game.EmotionPresenter.ShowEmotion();
        Managers.Instance.Game.ItemInfoPresenter.ShowItem();
    }

    public void HideAllPanel()
    {
        Managers.Instance.Game.EmotionPresenter.HideEmotion();
        Managers.Instance.Game.ItemInfoPresenter.HideItem();
    }

    //InputSystem으로 ESC 입력받을 시, 현재 열려있는 UI OFF
    public void UpdateCurrentUI()
    {
        switch(currentStates)
        {
            //메뉴 -> 게임화면으로 돌아가기
            case UIStates.Menu:
                Managers.Instance.Game.CinemachineController.MainCamera(true);
                Managers.Instance.Game.MenuPresenter.CloseMenu();
                Managers.Instance.Game.TimeController.ResumeGame();
                if (IsGoingDie)
                {
                    Managers.Instance.Game.DieUIPresenter.OnMessiaPanel();
                }
                break;
            case UIStates.Item:
                Managers.Instance.Game.ItemInfoPresenter.CloseItem();
                ShowMenuPanel();
                break;
            case UIStates.Emotion:
                Managers.Instance.Game.EmotionPresenter.CloseEmotion();
                Managers.Instance.Game.CinemachineController.EmotionCamera(false);
                ShowMenuPanel();
                break;
            case UIStates.Option:
                Managers.Instance.Game.OptionPresenter.CloseOption();
                ShowMenuPanel();
                break;
            //게임화면 -> 메뉴로 들어가기
            case UIStates.None:
                if (!Managers.Instance.Game.TimeController.TimeStop)
                {
                    Managers.Instance.Game.CinemachineController.MainCamera(false);
                    ShowMenuPanel();
                    Managers.Instance.Game.TimeController.StopGame();
                }
                if (IsGoingDie)
                {
                    Managers.Instance.Game.DieUIPresenter.OffMessiaPanel();
                }
                break;
        }
    }

    //아이템 number 값으로 아이콘 불러와서 이미지에 입히기
    public void SetIcon(int itemID, Image image)
    {
        image.sprite = Resources.Load("Item/item_" + itemID, typeof(Sprite)) as Sprite;
    }

    public void SetEmotionIcon(int emotionID, Image image)
    {
        image.sprite = Resources.Load("Emotion/emotion" + emotionID, typeof(Sprite)) as Sprite;
    }
}
