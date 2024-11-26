using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionView : MonoBehaviour
{
    public GameObject _optionPanel;

    public Slider CameraSlider;
    public Slider BgmSlider;
    public Slider SfxSlider;

    //ī�޶� �ΰ��� �ؽ�Ʈ
    [SerializeField]
    private Text _cameraText;
    //����� �ؽ�Ʈ
    [SerializeField]
    private Text _bgmText;
    //ȿ���� �ؽ�Ʈ
    [SerializeField]
    private Text _sfxText;

    private int _cameraSize;
    private int _bgmSize;
    private int _sfxSize;

    //�ɼ��г� ON
    public void OnClickOptionPanel()
    {
        _optionPanel.SetActive(true);
        ShowSensitiveSize();
        Managers.Instance.Game.OptionPresenter.ShowAllPanelUI();
        Managers.Instance.Game.OptionPresenter.CheckMenuPanel();
    }

    //�ɼ��г� OFF
    public void OnClickCloseOptionPanel()
    {
        _optionPanel.SetActive(false);      
        Managers.Instance.Game.OptionPresenter.CheckMenuPanel();
    }

    public void ShowSensitiveSize()
    {
        _cameraSize = (int)(Managers.Instance.Game.OptionPresenter.ShowSensitivitySize() * 100);
        _cameraText.text = _cameraSize.ToString();
    }

    public void ShowBGMSize()
    {
        _bgmSize = (int)(Managers.Instance.Game.OptionPresenter.ShowBGMVolumeSize() * 100);
        _bgmText.text = _bgmSize.ToString();
    }

    public void ShowSFXSize()
    {
        _sfxSize = (int)(Managers.Instance.Game.OptionPresenter.ShowSFXVolumeSize() * 100);
        _sfxText.text = _sfxSize.ToString();
    }
}
