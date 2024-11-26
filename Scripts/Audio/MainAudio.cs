using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainAudio : MonoBehaviour
{
    [Header("#BGM#")]
    public AudioClip BgmClip;
    public float BgmVolume;

    AudioSource _bgmPlayer;
    AudioHighPassFilter _bgmEffect;

    int channelIndex;

    private Coroutine currentBgmCoroutine; //현재 실행중인 Bgm을 추적하기 위한 변수 

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true;
        _bgmPlayer.volume = BgmVolume;
        _bgmPlayer.clip = BgmClip;

        _bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        PlayBgm(true);
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            _bgmPlayer.Play();
        }
        else
        {
            _bgmPlayer.Stop();
        }
    }

    public void ChangeBgmVolume(float volume)
    {
        _bgmPlayer.volume = volume;
    }
}
