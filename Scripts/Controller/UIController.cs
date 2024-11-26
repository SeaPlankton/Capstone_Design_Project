using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //� UI�� �����ִ��� üũ
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

    //���� �޴�, ��ư������ ������ ����ǥ�� ����
    public enum UIEmotionStates
    {
        None,
        Menu1,
        Menu2,
        Button1,
        Button2
    }

    //���� � UI�� �����ִ��� üũ
    [SerializeField]
    private UIStates currentStates;

    //���� � �޴�, ��ư�� ������ � ����ǥ������
    public UIEmotionStates currentEmotionStates;

    [HideInInspector]
    //�޴� â�� ���ȴ°�? - MenuView
    public bool IsMenuPanelOn = false;

    [HideInInspector]
    //Panel �������� ȭ���̵� �ȵǰ� �ϱ�����
    public bool IsUIOn = false;

    //����Ͽ� ���̽�ƽ ON/OFF ����
    public GameObject JoyStick;

    //�޽þ� �׼������� �ߵ� ���ΰ�?
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

    //Menu Panel �ݱ�
    public void HideMenuPanel()
    {     
        Managers.Instance.Game.MenuPresenter.HideMenuPanel();
    }

    //Menu Panel ����
    public void ShowMenuPanel()
    {
        Managers.Instance.Game.MenuPresenter.ShowMenuPanel();       
    }

    //�ػ� ������ ���� ��� �гε��� �ѹ��̶� ������� �����̵ȴ�.
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

    //InputSystem���� ESC �Է¹��� ��, ���� �����ִ� UI OFF
    public void UpdateCurrentUI()
    {
        switch(currentStates)
        {
            //�޴� -> ����ȭ������ ���ư���
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
            //����ȭ�� -> �޴��� ����
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

    //������ number ������ ������ �ҷ��ͼ� �̹����� ������
    public void SetIcon(int itemID, Image image)
    {
        image.sprite = Resources.Load("Item/item_" + itemID, typeof(Sprite)) as Sprite;
    }

    public void SetEmotionIcon(int emotionID, Image image)
    {
        image.sprite = Resources.Load("Emotion/emotion" + emotionID, typeof(Sprite)) as Sprite;
    }
}
