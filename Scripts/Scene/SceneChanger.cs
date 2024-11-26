using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public enum VideoType
{
    VideoWait = 0,
    VideoPlaying = 1,
}

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer _videoPlayer;

    [SerializeField]
    private GameObject _videoPanel;

    [SerializeField]
    private VideoType _videoType;

    [SerializeField]
    private GameObject _timeLine;


    public void Awake()
    {
        _videoType = VideoType.VideoWait;
        if (_videoPanel != null)
        {
            _videoPanel.SetActive(false);
        }
    }

    public void Update()
    {
        if (_videoType == VideoType.VideoPlaying)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Managers.Instance.TimeManager.RestartTimeManager();
                SceneManager.LoadScene("SampleScene");
            }
        }
    }


    public void OnClickStartBtn()
    {
        _timeLine.SetActive(true);
    }

    public void PlayScenario()
    {
        Debug.Log("샘플 씬");
        Managers.Instance.TimeManager.RestartTimeManager();

        if (_videoPanel != null)
        {
            _videoPanel.SetActive(true);
        }
        if (_videoPlayer != null)
        {
            _videoType = VideoType.VideoPlaying;
            _videoPlayer.loopPointReached += CheckVideoOver;
        }
    }

    void CheckVideoOver(UnityEngine.Video.VideoPlayer videoPlayer)
    {
        print("Video Is Over");
        Managers.Instance.TimeManager.RestartTimeManager();
        SceneManager.LoadScene("SampleScene");
    }


    public void OnClickExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }



}
