using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieUIView : MonoBehaviour
{
    //�׾��� �� ������� UI
    [SerializeField]
    private GameObject _diePanel;

    [SerializeField]
    private GameObject _victoryPanel;

    //�޽þ��� ���ϼ� �׼� UI - ���Ǻ� ��Ȱ
    [SerializeField]
    private GameObject _messiaPanel;

    //�޽þ� UI ���� ��ȥ ī��Ʈ
    [SerializeField]
    private Text _messiaText;

    private int _messiaCount = 35;

    //1ȸ ��Ȱ�� �����ϰ� ����
    private int _checkReborn = 0;

    //Game Over UI�� ������ִ� �������� Ȯ��
    private bool _isUIOn = false;

    //��Ȱ ��ư
    [SerializeField]
    private GameObject _rebornBtn;

    //��Ȱ ��ư Ŭ��
    public void OnClickRebornBtn()
    {
        Managers.Instance.Game.DieUIPresenter.ClickReborn();
    }

    public void OnClickContiuneBtn()
    {
        _victoryPanel.SetActive(false);
        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.None);
    }

    //Ÿ��Ʋ ��ư Ŭ��
    public void OnClickTitleBtn()
    {
        Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.None);
        _isUIOn = false;
        Managers.Instance.Game.DieUIPresenter.ClickTitle();
    }

    //Game over UI ����

    public void OpenDiePanel()
    {
        //�̹� UI�� ������ �ִ� �����̸� return
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

    //Game over UI ����
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

    //�޽þ� Text ����
    public void SetMessiaText()
    {
        _messiaText.text = "��Ȱ���� ���� ��ȥ\n" + _messiaCount;
        _messiaText.lineSpacing = 1.5f;
    }

    //Count�� update�Ǹ� �ؽ�Ʈ�� update
    public void SetMessiaCount(int count)
    {
        _messiaCount = count;
        SetMessiaText();
    }
}
