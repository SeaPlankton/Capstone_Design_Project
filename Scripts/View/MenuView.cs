using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class MenuView : MonoBehaviour
{
    /*-------------------------------------------�޴���ư---------------------------------*/
    [SerializeField]
    private GameObject _menuPanel;
    [SerializeField]
    private GameObject _quickPanel;

    /*-------------------------�޴� �г� ���� On/Off------------------------------------*/
    public void OnClickMenuBtn()
    {
        Managers.Instance.Game.MenuPresenter.OnMenu();
    }

    public void OnClickCloseMenuBtn()
    {
        Managers.Instance.Game.MenuPresenter.OffMenu();
    }

    //ESC�� ������ ��� �޴� Panel ON �۵�
    public void ClickMenuBtn()
    {
        Managers.Instance.Game.MenuPresenter.SetMenuState(true);
        Managers.Instance.Game.MenuPresenter.UpdateJoyStick(false);
        Managers.Instance.Game.MenuPresenter.SetMainCamera(false);
        EventSystem.current.SetSelectedGameObject(null);
        _menuPanel.SetActive(true);
        _menuPanel.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f);
        HideQuickPanel();
    }

    //ESC�� ������ ��� �޴� Panel OFF �۵�
    public async void ClickCloseMenuBtn()
    {
        Managers.Instance.Game.MenuPresenter.SetMenuState(false);
        Managers.Instance.Game.MenuPresenter.UpdateJoyStick(true);
        Managers.Instance.Game.MenuPresenter.SetMainCamera(true);
        _menuPanel.GetComponent<RectTransform>().DOAnchorPosX(960f, 0.3f);
        ShowQuickPanel();
        await UniTask.Delay(300);
        _menuPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        _menuPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(960f, 0);
        Managers.Instance.Game.MenuPresenter.SetMenuState(false);
        _menuPanel.SetActive(false);
    }

    public void ShowQuickPanel()
    {
        _quickPanel.SetActive(true);
        _quickPanel.GetComponent<RectTransform>().DOAnchorPosY(0f, 0.3f);
    }

    //��� Quick �г� On/Off
    public async void HideQuickPanel()
    {
        _quickPanel.GetComponent<RectTransform>().DOAnchorPosY(120f, 0.3f);
        await UniTask.Delay(300);
        _quickPanel.SetActive(false);
    }
}
