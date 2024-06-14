using System;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    [SerializeField] private Sprite[] muteSprites;
    [SerializeField] private Image image;
    [SerializeField] private Button button;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        image.sprite = muteSprites[Convert.ToInt32(AudioManager.instance.isMute)];
        button.onClick.AddListener(delegate
        {
            AudioManager.instance.OnClickMute();
            image.sprite = muteSprites[Convert.ToInt32(AudioManager.instance.isMute)];
        });
    }
}
