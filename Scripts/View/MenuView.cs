using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;

public class MenuView : MonoBehaviour
{
    /*-------------------------------------------메뉴버튼---------------------------------*/
    [SerializeField]
    private GameObject _menuPanel;
    [SerializeField]
    private GameObject _quickPanel;

    /*-------------------------메뉴 패널 관리 On/Off------------------------------------*/
    public void OnClickMenuBtn()
    {
        Managers.Instance.Game.MenuPresenter.OnMenu();
    }

    public void OnClickCloseMenuBtn()
    {
        Managers.Instance.Game.MenuPresenter.OffMenu();
    }

    //ESC로 눌렀을 경우 메뉴 Panel ON 작동
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

    //ESC로 눌렀을 경우 메뉴 Panel OFF 작동
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

    //상단 Quick 패널 On/Off
    public async void HideQuickPanel()
    {
        _quickPanel.GetComponent<RectTransform>().DOAnchorPosY(120f, 0.3f);
        await UniTask.Delay(300);
        _quickPanel.SetActive(false);
    }
}
