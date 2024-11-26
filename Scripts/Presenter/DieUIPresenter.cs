using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DieUIPresenter : MonoBehaviour
{
    [SerializeField]
    private DieUIView _dieUIView;

    private void Start()
    {
        Managers.Instance.Game.TimeController.PlayerDie += OnDiePanel;
        Managers.Instance.Game.TimeController.PlayerRetry += OffDiePanel;
        Managers.Instance.Game.TimeController.RebornTimeCheck += OnMessiaPanel;
        Managers.Instance.Game.TimeController.SuccessReborn += OffMessiaPanel;
    }

    //��Ȱ ��ư
    public void ClickReborn()
    {
        Managers.Instance.Game.TimeController.RetryPlayer();
    }
    

    //Ÿ��Ʋ ��ư
    public void ClickTitle()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnDiePanel()
    {
        _dieUIView.OpenDiePanel();
    }

    public void OffDiePanel()
    {
        _dieUIView.CloseDiePanel();
    }

    public void OnMessiaPanel()
    {
        _dieUIView.OpenMessiaPanel();
    }

    public void OffMessiaPanel()
    {
        _dieUIView.CloseMessiaPanel();
    }

    //View�� ��� Messia Count ����
    public void GetLeftMessiaCount(int count)
    {
        _dieUIView.SetMessiaCount(count);
    }
}
