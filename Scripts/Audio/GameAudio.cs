using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    [Header("#BGM#")]
    public AudioClip BgmClip;
    public float BgmVolume;

    [Header("#SFX#")]
    public AudioClip[] SfxClip;
    public float SfxVolume;

    public int Channels;

    AudioSource _bgmPlayer;
    AudioSource[] _sfxPlayers;
    AudioHighPassFilter _bgmEffect;

    int channelIndex;

    private Coroutine currentBgmCoroutine; //���� �������� Bgm�� �����ϱ� ���� ���� 

    public enum Sfx
    {
        LevelUp,
        Pistol,
        Rifle,
        Shotgun,
        Sniper,
        Select = 5,
        RPG,
        RPGBoom,
        Knife,
        Javelin
    }

    private void Awake()
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

        // ȿ���� �÷��̾� �ʱ�ȭ 
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[Channels];
        for (int index = 0; index < _sfxPlayers.Length; index++)
        {
            _sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[index].playOnAwake = false;
            _sfxPlayers[index].bypassListenerEffects = true;
            _sfxPlayers[index].volume = SfxVolume;
        }
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

    public void EffectBgm(bool isPlay)
    {
        _bgmEffect.enabled = isPlay;
    }


    public void PlaySfx(Sfx sfx)
    {
        //Debug.Log("ȿ���� ���");
        for (int index = 0; index < _sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % _sfxPlayers.Length; //  % _sfxPlayers.Length : ���� �Ѿ�°� ����

            if (_sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;

            _sfxPlayers[loopIndex].clip = SfxClip[(int)sfx];

            //Debug.Log("���� �ε��� : " + loopIndex);
            _sfxPlayers[loopIndex].Play();
            break;
        }
    }

    private IEnumerator FadeOutBgm(float duration, Action onFadeComplete)
    {
        float startVolume = _bgmPlayer.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _bgmPlayer.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        _bgmPlayer.volume = 0f;
        onFadeComplete?.Invoke(); //���̵� �ƿ��� �Ϸ�Ǹ� ���� �۾� ����
    }

    private IEnumerator FadeInBgm(float duration)
    {
        float startVolume = 0f;
        _bgmPlayer.volume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            _bgmPlayer.volume = Mathf.Lerp(startVolume, 1f, t / duration);
            yield return null;
        }

        //TODO : ȯ�ἳ������ ���� ���� ���� ���� �������� �����ؾ��� OR ȯ�漳�� ���� ���� ���� ���� ���� 
        _bgmPlayer.volume = 1f; // ���� ������ 1�� ���� 

    }

    public void ChangeBgmVolume(float volume)
    {
        _bgmPlayer.volume = volume;
    }
    public void ChangeSfxVolume(float volume)
    {
        for (int i = 0; i < _sfxPlayers.Length; i++)
        {
            _sfxPlayers[i].volume = volume;
        }
    }
}
