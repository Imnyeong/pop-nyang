using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    public bool isMute = false;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private AudioSource tileSource;
    public AudioSource timerSource;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Button muteButton;

    public Sprite[] muteSprites;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        isMute = PlayerPrefs.GetInt("Mute") == 1;
    }

    private void Start()
    {
        muteButton.GetComponent<Image>().sprite = muteSprites[Convert.ToInt32(isMute)];
        muteButton.onClick.AddListener(OnClickMute);

        PlayBGM();
    }

    public void OnClickMute()
    {
        isMute = !isMute;

        PlayerPrefs.SetInt("Mute", Convert.ToInt32(isMute));
        PlayCLick();
        muteButton.GetComponent<Image>().sprite = muteSprites[Convert.ToInt32(isMute)];
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

    public void PlayCLick()
    {
        if (isMute)
            return;
        tileSource.clip = Array.Find(clips, clip => clip.name == "Click");
        tileSource.Play();
    }
    public void PlaySwap()
    {
        if (isMute)
            return;
        tileSource.clip = Array.Find(clips, clip => clip.name == "Swap");
        tileSource.Play();
    }

    public void PlayFail()
    {
        if (isMute)
            return;
        tileSource.clip = Array.Find(clips, clip => clip.name == "Fail");
        tileSource.Play();
    }

    public void PlayPop()
    {
        if (isMute)
            return;
        tileSource.clip = Array.Find(clips, clip => clip.name == "Pop");
        tileSource.Play();
    }

    public void PlayBomb()
    {
        if (isMute)
            return;
        effectSource.clip = Array.Find(clips, clip => clip.name == "Bomb");
        effectSource.Play();
    }

    public void PlayRefresh()
    {
        if (isMute)
            return;
        effectSource.clip = Array.Find(clips, clip => clip.name == "Refresh");
        effectSource.Play();
    }

    public void PlayTimer()
    {
        if (isMute)
            return;
        timerSource.Play();
    }
}
