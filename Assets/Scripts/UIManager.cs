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

    private float refreshDelay = 5.0f;
    public float timeValue = 120.0f;
    public float timer;

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

}
