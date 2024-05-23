using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public void OnClickStart()
    {
        AudioManager.instance.PlayCLick();
        SceneManager.LoadScene("InGame");
    }
    public void OnClickExit()
    {
        AudioManager.instance.PlayCLick();
        Application.Quit();
    }
}
