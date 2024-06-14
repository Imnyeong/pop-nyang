using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Text scoreText;

    private void Awake()
    {
        replayButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlayEffect(AudioManager.EffectType.Click);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    });
        exitButton.onClick.AddListener(delegate
        {
            AudioManager.instance.PlayEffect(AudioManager.EffectType.Click);
            SceneManager.LoadScene("Intro");
        });
    }
    
    public void SetScore(int _value)
    {
        scoreText.text = $"SCORE\n{_value}";
    }
}
