using System;
using UnityEngine.UI;

public class MuteButton : Button
{
    private void Start()
    {
        this.GetComponent<Image>().sprite = AudioManager.instance.muteSprites[Convert.ToInt32(AudioManager.instance.isMute)];
        this.onClick.AddListener(OnClickMute);
    }
    public void OnClickMute()
    {
        AudioManager.instance.OnClickkMute();
        AudioManager.instance.PlayCLick();
        this.GetComponent<Image>().sprite = AudioManager.instance.muteSprites[Convert.ToInt32(AudioManager.instance.isMute)];
    }
}
