using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Slider timeSlider;

    public Button refreshButton;
    public Button bombButton;

    public GameOverPopup gameoverPopup;

    public Text scoreText;
    public Text comboText;

    private float refreshDelay = 5.0f;
    private float timeValue = 120.0f;
    [HideInInspector] public float timer;

    [HideInInspector] public int score = 0;
    private IEnumerator comboCoroutine = null;
    private float comboDelay = 0.5f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        refreshButton.onClick.AddListener(OnClickRefresh);
        bombButton.onClick.AddListener(OnClickBomb);

        score = 0;
    }

    public void StartTimer()
    {
        timer = timeValue;
        StartCoroutine(TimeCoroutine());
    }

    private IEnumerator TimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        timer -= 0.1f;
        timeSlider.value = timer / timeValue;

        if(timer <= 10.0f && !AudioManager.instance.timerSource.isPlaying)
        {
            AudioManager.instance.PlayTimer();
        }
        if (timer <= 0.0f)
        {
            StopAllCoroutines();
            AudioManager.instance.StopAudio();
            Board.instance.canControl = false;
            gameoverPopup.gameObject.SetActive(true);
            gameoverPopup.scoreText.text = $"SCORE\n{score}";
        }
        StartCoroutine(TimeCoroutine());
    }

    private void OnClickRefresh()
    {
        if (!Board.instance.canControl)
            return;

        AudioManager.instance.PlayRefresh();
        Board.instance.RefreshAllTiles();
        StartCoroutine(RefreshCoroutine());
    }

    private IEnumerator RefreshCoroutine()
    {
        refreshButton.interactable = false;
        yield return new WaitForSecondsRealtime(refreshDelay);
        refreshButton.interactable = true;
    }

    private void OnClickBomb()
    {
        if (!Board.instance.canControl)
            return;

        AudioManager.instance.PlayBomb();
        Board.instance.Bomb();
    }

    public void ShowCombo(int _combo)
    {
        if(comboCoroutine != null)
        {
            StopCoroutine(comboCoroutine);
            comboCoroutine = null;
        }
        comboCoroutine = ComboCoroutine(_combo);
        StartCoroutine(comboCoroutine);
    }

    private IEnumerator ComboCoroutine(int _combo)
    {
        comboText.text = $"{_combo} Combo!";
        comboText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(comboDelay);
        comboText.gameObject.SetActive(false);
    }

    public void SetScore(List<Tile> _tiles, int _combo)
    {
        int curScore = 0;

        foreach(Tile _tile in _tiles)
        {
            curScore += _tile.item.value;
        }
        if(_combo != 0)
        {
            curScore *= _combo;
        }

        score += curScore;
        scoreText.text = $"SCORE\n{score}";
    }
}
