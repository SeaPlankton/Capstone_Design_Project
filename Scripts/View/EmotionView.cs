using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EmotionView : MonoBehaviour
{
    public GameObject _emotionPanel;


    [Header("���� ���õ� ����ǥ��")]
    public int CurEmotion01 = 0;
    public int CurEmotion02 = 1;

    //����ǥ�� ������ ��� �뵵
    [Header("��� �г� ����ǥ�� ���� UI")]
    [SerializeField]
    private GameObject[] _selectEmotionFrame;
    [SerializeField]
    private Text[] _selectEmotionText;
    [SerializeField]
    private Image[] _selectEmotionIcon;


    [Header("�ϴ� �г� ����ǥ�� ���� UI")]
    [SerializeField]
    private GameObject[] _emotionFrame;
    [SerializeField]
    private Text[] _emotionText;
    [SerializeField]
    private Image[] _emotionIcon;



    //����ǥ�� �г� ON
    public void OnClickEmotionPanel()
    {
        _emotionPanel.SetActive(true);
        _emotionPanel.GetComponent<RectTransform>().DOAnchorPosX(0f, 0.3f);
        Managers.Instance.Game.EmotionPresenter.CheckMenuPanel();
        Managers.Instance.Game.EmotionPresenter.SetEmotionCamera(true);

        // �޴��гο��� ����ǥ�� �гη� ���� ����1�� ������ �Ҵ��ϰ� �����ص� 
        Managers.Instance.Game.EmotionPresenter.SelectHighlightEmotion(0);
        _selectEmotionFrame[0].SetActive(true);
        _selectEmotionFrame[1].SetActive(false);
    }

    //����ǥ�� �г� OFF
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

    //��� ����ǥ���� ������ 
    public void OnClickSelectEmotionButton(int num)
    {
        //Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Melee);
        Managers.Instance.Game.EmotionPresenter.SelectHighlightEmotion(num);

        switch (num)
        {
            case 0:
                _selectEmotionFrame[0].SetActive(true);
                _selectEmotionFrame[1].SetActive(false);
                Debug.Log("���� 1 ����");
                break;
            case 1:
                _selectEmotionFrame[0].SetActive(false);
                _selectEmotionFrame[1].SetActive(true);
                Debug.Log("���� 2 ����");
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

        //���õ� ��ȣ�� -1(�ϴ� ������ ���� X)�� �ƴ϶�� ����ǰ� ���� 
        if (selectEmotionNum != -1)
        {
            //�Ȱ����� �ٸ� ���Կ� ��ü������ ��ü���� �ִ��� -1�� ����

            switch (highlightEmotionNum)
            {
                case 0:
                    if (Managers.Instance.Game.EmotionPresenter.Emotion02 == selectEmotionNum) //��ġ�� ��� ����
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
            Debug.Log("�ϴ� ������ �������� �ʾҽ��ϴ�.");
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
