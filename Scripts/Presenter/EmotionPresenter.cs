using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionPresenter : MonoBehaviour
{
    public int Emotion01 = 0;
    public int Emotion02 = 1;


    [SerializeField]
    private EmotionView _emotionView;

    [SerializeField]
    private Animator _animator;



    [Header("버튼 선택관련 Value")]
    [SerializeField]
    private int _highlightEmotionNum = 0;
    [SerializeField]
    private int _selectEmotionNum = -1;

    protected static readonly int EmotionMenuHash = Animator.StringToHash("Action Idle To Fight Idle");


    ExpressEmotionDB expressEmotionDB;

    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }

    private void Init()
    {
        Managers.Instance.Game.EmotionPresenter = this;
        expressEmotionDB = new ExpressEmotionDB();
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }

    //해상도 설정을 위해 단순 ON/OFF, Dotween이 필요없음
    public void ShowEmotion()
    {
        _emotionView._emotionPanel.gameObject.SetActive(true);
    }

    public void HideEmotion()
    {
        _emotionView._emotionPanel.gameObject.SetActive(false);
    }

    //ESC로 패널 끌 때 실행
    public void CloseEmotion()
    {
        _emotionView.OnClickCloseEmotionPanel();
    }

    //메인메뉴에서 감정표현 메뉴 눌렀을 때, 카메라 전환
    public void SetEmotionCamera(bool cameraOn)
    {
        Managers.Instance.Game.CinemachineController.EmotionCamera(cameraOn);
    }

    public void CheckMenuPanel()
    {
        //메뉴 열려있으면 메뉴 닫기
        if (Managers.Instance.Game.UIController.IsMenuPanelOn)
        {
            Managers.Instance.Game.UIController.HideMenuPanel();
            Managers.Instance.Game.UIController.IsMenuPanelOn = false;
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Emotion);
            _animator.CrossFade(EmotionMenuHash, 0.05f);
        }
        else if (!Managers.Instance.Game.UIController.IsMenuPanelOn)
        {
            Managers.Instance.Game.UIController.ShowMenuPanel();
            Managers.Instance.Game.UIController.IsMenuPanelOn = true;
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Menu);
            _animator.Play("Aiming Gun");
        }
    }


    public void SelectHighlightEmotion(int num)
    {
        if (_highlightEmotionNum != num)
        {
            _selectEmotionNum = -1;
        }
        _highlightEmotionNum = num;
    }

    public int GetHighLightEmotionNum()
    {
        return _highlightEmotionNum;
    }

    public void SelectEmotion(int num)
    {
        _selectEmotionNum = num;
    }
    public int GetSelectEmotionButton()
    {
        return _selectEmotionNum;
    }


    public void ApplyEmotion()
    {
        switch (_highlightEmotionNum)
        {
            case 0:
                if (Emotion02 == _selectEmotionNum) //겹치는 경우 수정
                {
                    Emotion02 = -1;
                }
                Emotion01 = _selectEmotionNum;
                break;
            case 1:
                if (Emotion01 == _selectEmotionNum)
                {
                    Emotion01 = -1;
                }
                Emotion02 = _selectEmotionNum;
                break;
        }

        _selectEmotionNum = -1;
    }

    public void PlayExpressEmotion(int num)
    {
        string expressEmotionValue = string.Empty;
        ExpressEmotionDB emotion;

        switch (num)
        {
            case 1:
                emotion = (ExpressEmotionDB)Emotion01;
                expressEmotionValue = emotion.ToString();
                break;
            case 2:
                emotion = (ExpressEmotionDB)Emotion02;
                expressEmotionValue = emotion.ToString();
                break;
        }

        Debug.Log("현재 emotion이름: " + expressEmotionValue);

        if (expressEmotionValue != "Null")
        {
            _animator.Play(expressEmotionValue, default, -1.0f);
        }
    }

    public void ClickPlayOnceEmotionAnimation(int num)
    {
        string expressEmotionValue = string.Empty;
        ExpressEmotionDB emotion;
        emotion = (ExpressEmotionDB)num;
        expressEmotionValue = emotion.ToString();

        _animator.Play(expressEmotionValue, default, -1.0f);
    }

    public int GetInputEmotionNumValue(int keyNum)
    {
        int returnValue = 0;
        ExpressEmotionDB emotion;

        switch (keyNum)
        {
            case 1:
                emotion = (ExpressEmotionDB)Emotion01;
                returnValue = (int)emotion;
                break;
            case 2:
                emotion = (ExpressEmotionDB)Emotion02;
                returnValue = (int)emotion;
                break;
        }

        return returnValue;
    }
}
