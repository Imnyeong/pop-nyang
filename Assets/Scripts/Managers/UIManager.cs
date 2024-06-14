using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private Slider timeSlider;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button bombButton;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;

    [HideInInspector] public float timer;
    [HideInInspector] public int score = 0;

    private const float refreshDelay = 5.0f;
    private const float timeDelay = 0.1f;
    private const float timeValue = 120.0f;
    private const float timeLimit = 10.0f;

    private IEnumerator comboCoroutine = null;
    private float comboDelay = 0.5f;

    public GameOverPopup gameoverPopup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        Init();
    }
    private void Init()
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
    private void OnClickRefresh()
    {
        if (!Board.instance.canControl)
            return;

        AudioManager.instance.PlayEffect(AudioManager.EffectType.Refresh);
        Board.instance.RefreshAllTiles();
        StartCoroutine(RefreshCoroutine());
    }
    private void OnClickBomb()
    {
        if (!Board.instance.canControl)
            return;

        AudioManager.instance.PlayEffect(AudioManager.EffectType.Bomb);
        Board.instance.Bomb();
    }
    public void SetBombButton(bool _isActive)
    {
        bombButton.interactable = _isActive;
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
    #region Coroutine
    private IEnumerator TimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        timer -= timeDelay;
        timeSlider.value = timer / timeValue;

        if (timer <= timeLimit && !AudioManager.instance.GetTimerPlaying())
        {
            AudioManager.instance.PlayTimer();
        }
        if (timer <= 0.0f)
        {
            StopAllCoroutines();
            AudioManager.instance.StopAudio();
            Board.instance.canControl = false;
            gameoverPopup.SetScore(score);
            gameoverPopup.gameObject.SetActive(true);
        }
        StartCoroutine(TimeCoroutine());
    }
    private IEnumerator RefreshCoroutine()
    {
        refreshButton.interactable = false;
        yield return new WaitForSecondsRealtime(refreshDelay);
        refreshButton.interactable = true;
    }
    private IEnumerator ComboCoroutine(int _combo)
    {
        comboText.text = $"{_combo} Combo!";
        comboText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(comboDelay);
        comboText.gameObject.SetActive(false);
    }
    #endregion
}
