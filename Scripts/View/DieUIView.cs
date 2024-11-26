using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieUIView : MonoBehaviour
{
    //죽었을 때 띄워지는 UI
    [SerializeField]
    private GameObject _diePanel;

    [SerializeField]
    private GameObject _victoryPanel;

    //메시아의 스완송 액세 UI - 조건부 부활
    [SerializeField]
    private GameObject _messiaPanel;

    //메시아 UI 남은 영혼 카운트
    [SerializeField]
    private Text _messiaText;

    private int _messiaCount = 35;

    //1회 부활만 가능하게 설정
    private int _checkReborn = 0;

    //Game Over UI가 띄워져있는 상태인지 확인
    private bool _isUIOn = false;

    //부활 버튼
    [SerializeField]
    private GameObject _rebornBtn;

    //부활 버튼 클릭
    public void OnClickRebornBtn()
    {
        Managers.Instance.Game.DieUIPresenter.ClickReborn();
    }

    public void OnClickContiuneBtn()
    {
        _victoryPanel.SetActive(false);
        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.None);
    }

    //타이틀 버튼 클릭
    public void OnClickTitleBtn()
    {
        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.None);
        _isUIOn = false;
        Managers.Instance.Game.DieUIPresenter.ClickTitle();
    }

    //Game over UI 띄우기

    public void OpenDiePanel()
    {
        //이미 UI가 열려져 있는 상태이면 return
        if (_isUIOn)
            return;

        _diePanel.SetActive(true);
        _isUIOn = true;
        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Dead);

        if (_checkReborn >= 1)
        {
            _rebornBtn.SetActive(false);
        }
        _checkReborn++;
    }

    //Game over UI 끄기
    public void CloseDiePanel()
    {
        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.None);
        _diePanel.SetActive(false);
        _isUIOn = false;
    }

    public void OpenMessiaPanel()
    {
        _messiaPanel.SetActive(true);
        SetMessiaText();
        _messiaPanel.GetComponent<Image>().DOFade(0.4f, 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void CloseMessiaPanel()
    {
        _messiaPanel.SetActive(false);
    }

    //메시아 Text 세팅
    public void SetMessiaText()
    {
        _messiaText.text = "부활까지 남은 영혼\n" + _messiaCount;
        _messiaText.lineSpacing = 1.5f;
    }

    //Count가 update되면 텍스트도 update
    public void SetMessiaCount(int count)
    {
        _messiaCount = count;
        SetMessiaText();
    }
}
