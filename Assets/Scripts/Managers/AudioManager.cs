using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public bool isMute { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource tileSource;
    [SerializeField] private AudioSource timerSource;

    [SerializeField] private AudioClip[] clips;

    public enum EffectType
    {
        Pop,
        Bomb,
        Fail,
        Swap,
        Refresh,
        Click
    };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);

        if(!PlayerPrefs.HasKey("Mute"))
        {
            PlayerPrefs.SetInt("Mute", 0);
        }
        isMute = PlayerPrefs.GetInt("Mute") == 1;
    }
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        PlayBGM();
    }
    public bool GetTimerPlaying()
    {
        return timerSource.isPlaying;
    }
    public void OnClickMute()
    {
        isMute = !isMute;

        PlayerPrefs.SetInt("Mute", Convert.ToInt32(isMute));
        PlayEffect(EffectType.Click);

        bgmSource.volume = isMute ? 0.0f : 1.0f;

        if (!isMute)
        {
            bgmSource.Play();
        }
    }
    public void PlayBGM()
    {
        if (isMute)
            return;
        bgmSource.Play();
    }
    public void StopAudio()
    {
        bgmSource.Stop();
        effectSource.Stop();
        timerSource.Stop();
    }
    public void PlayEffect(EffectType _type)
    {
        if (isMute)
            return;

        switch (_type)
        {
            case EffectType.Fail:
                tileSource.clip = Array.Find(clips, clip => clip.name == "Fail");
                break;
            case EffectType.Swap:
                tileSource.clip = Array.Find(clips, clip => clip.name == "Swap");
                break;
            case EffectType.Pop:
                tileSource.clip = Array.Find(clips, clip => clip.name == "Pop");
                break;
            case EffectType.Bomb:
                effectSource.clip = Array.Find(clips, clip => clip.name == "Bomb");
                break;
            case EffectType.Refresh:
                effectSource.clip = Array.Find(clips, clip => clip.name == "Refresh");
                break;
            case EffectType.Click:
                effectSource.clip = Array.Find(clips, clip => clip.name == "Click");
                break;
        }
        switch(_type)
        {
            case EffectType.Bomb:
            case EffectType.Refresh:
            case EffectType.Click:
                effectSource.Play();
                break;
            case EffectType.Pop:
            case EffectType.Fail:
            case EffectType.Swap:
                tileSource.Play();
                break;
        }
    }
    public void PlayTimer()
    {
        if (isMute)
            return;
        timerSource.Play();
    }
}
