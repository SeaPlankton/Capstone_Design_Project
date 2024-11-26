using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPresenter : MonoBehaviour
{
    [SerializeField]
    private OptionView _optionView;

    private float _sensitivity = 0.5f;
    private float _bgmSize;
    private float _sfxSize;

    [HideInInspector]
    public int xRatio = 16;
    [HideInInspector]
    public int yRatio = 9;

    //UIArea ũ�� �޾Ƽ� ParentPanelũ�� ����
    [HideInInspector]
    public Vector2 UIAreaRect;

    public delegate void ChangeResolution();

    //�ػ� ���� �� UIArea ���� �缳��(CameraResolutionOptimizer)
    public event ChangeResolution OnChangeUIAreaResolution;

    //�θ� �г� �ػ󵵿� �°� ũ������(ParentPanelResolutionControl)
    public event ChangeResolution OnChangeParentResolution;

    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
    }

    private void Start()
    {
        OnChangeUIAreaResolution();
        OnChangeParentResolution();

        InitializeSliderVolume();
    }

    private void Init()
    {
        Managers.Instance.Game.OptionPresenter = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }

    public void CloseOption()
    {
        _optionView.OnClickCloseOptionPanel();
        Managers.Instance.Game.UIController.HideAllPanel();
    }


    public void ShowAllPanelUI()
    {
        Managers.Instance.Game.UIController.ShowAllPanel();
    }


    public void CheckMenuPanel()
    {
        //�޴� ���������� �޴� �ݱ�
        if (Managers.Instance.Game.UIController.IsMenuPanelOn)
        {
            Managers.Instance.Game.UIController.HideMenuPanel();
            Managers.Instance.Game.UIController.IsMenuPanelOn = false;
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Option);
        }
        else if (!Managers.Instance.Game.UIController.IsMenuPanelOn)
        {
            Managers.Instance.Game.UIController.ShowMenuPanel();
            Managers.Instance.Game.UIController.IsMenuPanelOn = true;
            Managers.Instance.Game.UIController.SetCurrentUIState(UIController.UIStates.Menu);
        }
    }

    /// <summary>
    /// �ػ� 0. 16:9  1. 4:3  2. 20:9
    /// </summary>
    public void SelectResolution(int num)
    {
        switch (num)
        {
            case 0:
                xRatio = 16;
                yRatio = 9;
                OnChangeUIAreaResolution();
                OnChangeParentResolution();
                break;
            case 1:
                xRatio = 4;
                yRatio = 3;
                OnChangeUIAreaResolution();
                OnChangeParentResolution();
                break;
            case 2:
                xRatio = 20;
                yRatio = 9;
                OnChangeUIAreaResolution();
                OnChangeParentResolution();
                break;
        }
    }

    public void InitializeSliderVolume()
    {
        _optionView.BgmSlider.value = Managers.Instance.Game.GameAudio.BgmVolume;
        _optionView.SfxSlider.value = Managers.Instance.Game.GameAudio.SfxVolume;
    }

    public void SetCameraSensitive(float sensitivity)
    {
        _sensitivity = sensitivity;
        Managers.Instance.Game.InputManager.ChangeSensitivity(sensitivity);
        _optionView.ShowSensitiveSize();
    }

    public void SetBgmVolume(float volume)
    {
        _bgmSize = volume;
        Managers.Instance.Game.GameAudio.ChangeBgmVolume(volume);
        _optionView.ShowBGMSize();
    }
    public void SetSfxVolume(float volume)
    {
        _sfxSize = volume;
        Managers.Instance.Game.GameAudio.ChangeSfxVolume(volume);
        _optionView.ShowSFXSize();
    }

    public float ShowSensitivitySize()
    {
        return _sensitivity;
    }
    public float ShowBGMVolumeSize()
    {
        return _bgmSize;
    }
    public float ShowSFXVolumeSize()
    {
        return _sfxSize;
    }
}
