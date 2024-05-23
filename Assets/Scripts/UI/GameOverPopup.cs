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
        replayButton.onClick.AddListener(delegate {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    });
        exitButton.onClick.AddListener(delegate {
            Application.Quit();
	    });
    }
    
}
