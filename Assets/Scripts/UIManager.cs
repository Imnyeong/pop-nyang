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
    public Button soundButton;

    public GameObject gameoverPopup;

    public Text scoreText;
    public Text comboText;

    [HideInInspector] private float refreshDelay = 5.0f;
    [HideInInspector] public float timeValue = 120.0f;
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
    void Start()
    {
        refreshButton.onClick.AddListener(OnClickRefresh);
        bombButton.onClick.AddListener(OnClickBomb);

        score = 0;
        timer = timeValue;
        StartCoroutine(TimeCoroutine());
    }

    public IEnumerator TimeCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        timer -= 0.1f;
        timeSlider.value = timer / timeValue;
        if(timer <= 0.0f)
        {
            StopAllCoroutines();
            gameoverPopup.SetActive(true);
        }
        StartCoroutine(TimeCoroutine());
    }

    public void OnClickRefresh()
    {
        if (!Board.instance.canControl)
            return;

        Board.instance.SetAllTiles();

        StartCoroutine(RefreshCoroutine());
    }

    public IEnumerator RefreshCoroutine()
    {
        refreshButton.interactable = false;
        yield return new WaitForSecondsRealtime(refreshDelay);
        refreshButton.interactable = true;
    }

    public void OnClickBomb()
    {
        if (!Board.instance.canControl)
            return;

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

    public IEnumerator ComboCoroutine(int _combo)
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
