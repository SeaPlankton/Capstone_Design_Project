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

    private Coroutine currentBgmCoroutine; //���� �������� Bgm�� �����ϱ� ���� ���� 

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        // ����� �÷��̾� �ʱ�ȭ
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
