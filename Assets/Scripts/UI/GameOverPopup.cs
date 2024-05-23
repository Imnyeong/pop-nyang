using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    public Button replayButton;
    public Button exitButton;

    public Text scoreText;

    void Awake()
    {
        replayButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlayCLick();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    });
        exitButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlayCLick();
            Application.Quit();
	    });
    }
    
}
