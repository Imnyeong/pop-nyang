using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public void OnClickStart()
    {
        AudioManager.instance.PlayEffect(AudioManager.EffectType.Click);
        SceneManager.LoadScene("InGame");
    }
    public void OnClickExit()
    {
        AudioManager.instance.PlayEffect(AudioManager.EffectType.Click);
        Application.Quit();
    }
}
