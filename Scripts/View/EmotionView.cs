using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EmotionView : MonoBehaviour
{
    public GameObject _emotionPanel;


    [Header("현재 선택된 감정표현")]
    public int CurEmotion01 = 0;
    public int CurEmotion02 = 1;

    //감정표현 아이콘 띄울 용도
    [Header("상단 패널 감정표현 관련 UI")]
    [SerializeField]
    private GameObject[] _selectEmotionFrame;
    [SerializeField]
    private Text[] _selectEmotionText;
    [SerializeField]
    private Image[] _selectEmotionIcon;


    [Header("하단 패널 감정표현 관련 UI")]
    [SerializeField]
    private GameObject[] _emotionFrame;
    [SerializeField]
    private Text[] _emotionText;
    [SerializeField]
    private Image[] _emotionIcon;



    //감정표현 패널 ON
    public void OnClickEmotionPanel()
    {
        _emotionPanel.SetActive(true);
        _emotionPanel.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f);
        Managers.Instance.Game.EmotionPresenter.CheckMenuPanel();
        Managers.Instance.Game.EmotionPresenter.SetEmotionCamera(true);

        // 메뉴패널에서 감정표현 패널로 갈때 슬롯1로 무조건 할당하게 설정해둠 
        Managers.Instance.Game.EmotionPresenter.SelectHighlightEmotion(0);
        _selectEmotionFrame[0].SetActive(true);
        _selectEmotionFrame[1].SetActive(false);
    }

    //감정표현 패널 OFF
    public void OnClickCloseEmotionPanel()
    {
        for (int i = 0; i < _emotionFrame.Length; i++)
        {
            _emotionFrame[i].SetActive(false);
        }

        Managers.Instance.Game.EmotionPresenter.SelectHighlightEmotion(-1);

        _emotionPanel.GetComponent<RectTransform>().DOAnchorPosX(2000f, 0.3f);
        Managers.Instance.Game.EmotionPresenter.CheckMenuPanel();
        Managers.Instance.Game.EmotionPresenter.SetEmotionCamera(false);
        _emotionPanel.SetActive(false);
    }

    //상단 감정표현을 누를때 
    public void OnClickSelectEmotionButton(int num)
    {
        //Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Melee);
        Managers.Instance.Game.EmotionPresenter.SelectHighlightEmotion(num);

        switch (num)
        {
            case 0:
                _selectEmotionFrame[0].SetActive(true);
                _selectEmotionFrame[1].SetActive(false);
                Debug.Log("슬롯 1 선택");
                break;
            case 1:
                _selectEmotionFrame[0].SetActive(false);
                _selectEmotionFrame[1].SetActive(true);
                Debug.Log("슬롯 2 선택");
                break;
        }

        for (int i = 0; i < _emotionFrame.Length; i++)
        {
            _emotionFrame[i].SetActive(false);
        }
        Managers.Instance.Game.EmotionPresenter.SelectEmotion(-1);
    }

    public void OnClickEmotionButton(int num)
    {
        Managers.Instance.Game.EmotionPresenter.SelectEmotion(num);
        Managers.Instance.Game.EmotionPresenter.ClickPlayOnceEmotionAnimation(num);

        _emotionFrame[num].SetActive(true);


        for (int i = 0; i < _emotionFrame.Length; i++)
        {
            if (num == i)
            {
                _emotionFrame[num].SetActive(true);
            }
            else
            {
                _emotionFrame[i].SetActive(false);
            }
        }
    }

    public void OnClickApplyEmotionButton()
    {
        int highlightEmotionNum = Managers.Instance.Game.EmotionPresenter.GetHighLightEmotionNum();
        int selectEmotionNum = Managers.Instance.Game.EmotionPresenter.GetSelectEmotionButton();

        //선택된 번호가 -1(하단 슬롯을 선택 X)이 아니라면 적용되게 설정 
        if (selectEmotionNum != -1)
        {
            //똑같은걸 다른 슬롯에 교체했을때 교체전에 있던것 -1로 변경

            switch (highlightEmotionNum)
            {
                case 0:
                    if (Managers.Instance.Game.EmotionPresenter.Emotion02 == selectEmotionNum) //겹치는 경우 수정
                    {
                        _selectEmotionIcon[highlightEmotionNum + 1].sprite = null;
                        _selectEmotionText[highlightEmotionNum + 1].text = "";
                    }
                    break;
                case 1:
                    if (Managers.Instance.Game.EmotionPresenter.Emotion01 == selectEmotionNum)
                    {
                        _selectEmotionIcon[highlightEmotionNum - 1].sprite = null;
                        _selectEmotionText[highlightEmotionNum - 1].text = "";
                    }
                    break;
            }
            Managers.Instance.Game.UIController.SetEmotionIcon(selectEmotionNum + 1, _selectEmotionIcon[highlightEmotionNum]);
            _selectEmotionText[highlightEmotionNum].text = _emotionText[selectEmotionNum].text;

            Managers.Instance.Game.EmotionPresenter.ApplyEmotion();

            for (int i = 0; i < _emotionFrame.Length; i++)
            {
                _emotionFrame[i].SetActive(false);
            }

        }
        else
        {
            Debug.Log("하단 슬롯을 선택하지 않았습니다.");
        }
    }

    public void OnClickOtherSide()
    {
        int selectNum = Managers.Instance.Game.EmotionPresenter.GetSelectEmotionButton();
        Debug.Log(selectNum);
        if (selectNum != -1)
        {
            _emotionFrame[selectNum].SetActive(false);
        }
        Managers.Instance.Game.EmotionPresenter.SelectEmotion(-1);
    }
}
