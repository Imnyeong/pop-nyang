using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board instance = null;
    public Row[] rows;

    private Item[] items;

    private List<Tile> selectedTiles = new List<Tile>();
    private List<Tile> checkedTiles = new List<Tile>();
    private List<Tile> popTiles = new List<Tile>();

    private bool canPop = false;
    public bool canControl = false;

    private float moveDelay = 0.2f;
    private int popCount = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        items = Resources.LoadAll<Item>("Prefabs/Items");
        SetAllTiles();
    }

    private void Update()
    {
        TouchCheck();
    }

    private void TouchCheck()
    {
        if (canControl && Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(touchPos, Camera.main.transform.forward);

            if (ray.collider != null)
            {
                Tile touchTile = ray.transform.gameObject.GetComponent<Tile>();
                if (touchTile != null && !selectedTiles.Contains(touchTile))
                {
                    OnClickTile(touchTile);
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedTiles.Count == 1)
        {
            selectedTiles.Clear();
        }
    }

    public async void SetAllTiles()
    {
        canControl = false;
        popCount = 0;
        foreach (Row _row in rows)
        {
            foreach (Tile _tile in _row.tiles)
            {
                _tile.SetTile(items[UnityEngine.Random.Range(0, items.Length)]);
            }
        }
        await CheckAllTiles();
    }

    private async void OnClickTile(Tile _tile)
    {
        popCount = 0;

        if (!canControl)
            return;

        if (selectedTiles.Count > 1)
            return;

        selectedTiles.Add(_tile);

        if (selectedTiles.Count == 2)
        {
            if (Array.Exists(selectedTiles[0].checkTiles, x => x == _tile))
            {
                canControl = false;
                AudioManager.instance.PlaySwap();

                await DoSwap(selectedTiles[0], selectedTiles[1]);
                await CheckAllTiles();

                if (canPop == false && popCount == 0)
                {
                    AudioManager.instance.PlayFail();
                    await DoSwap(selectedTiles[1], selectedTiles[0]);
                }
            }
            selectedTiles.Clear();
        }
    }

    private async Task DoSwap(Tile _tile1, Tile _tile2)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Join(_tile1.image.transform.DOMove(_tile2.transform.position, moveDelay))
                .Join(_tile2.image.transform.DOMove(_tile1.transform.position, moveDelay));

        sequence.OnComplete(delegate
        {
            _tile1.image.transform.SetParent(_tile2.transform);
            _tile2.image.transform.SetParent(_tile1.transform);

            Image tmpImage = _tile1.image;
            _tile1.image = _tile2.image;
            _tile2.image = tmpImage;

            Image tmpIcon = _tile1.icon;
            _tile1.icon = _tile2.icon;
            _tile2.icon = tmpIcon;

            Item tmpItem = _tile1.item;
            _tile1.item = _tile2.item;
            _tile2.item = tmpItem;

        });

        await sequence.Play().AsyncWaitForCompletion();
    }

    private async Task CheckAllTiles()
    {
        canPop = false;

        foreach (Row _row in rows)
        {
            foreach (Tile _tile in _row.tiles)
            {
                checkedTiles.Clear();
                await CheckTile(_tile);

                if (checkedTiles.Count > 2)
                {
                    canPop = true;
                    popTiles.AddRange(checkedTiles);
                }
            }
        }
        if (canPop)
        {
            popTiles = popTiles.Distinct().ToList();
            await Pop(popTiles);
            await CheckAllTiles();
        }
        else
        {
            popTiles.Clear();
            canControl = true;
        }
    }

    private async Task CheckTile(Tile _tile)
    {
        if (checkedTiles.Count == 0)
        {
            checkedTiles.Add(_tile);
        }

        for (int i = 0; i < _tile.checkTiles.Length; i++)
        {
            if(_tile.checkTiles[i] != null && !checkedTiles.Contains(_tile.checkTiles[i]) && _tile.checkTiles[i].item == checkedTiles[0].item)
            {
                checkedTiles.Add(_tile.checkTiles[i]);
                await CheckTile(_tile.checkTiles[i]);
            }
        }
    }

    private async Task Pop(List<Tile> _tiles)
    {
        Sequence sequence = DOTween.Sequence();

        foreach(Tile _tile in _tiles)
        {
            sequence.Join(_tile.image.transform.DOScale(Vector3.zero, moveDelay));
        }

        AudioManager.instance.PlayPop();

        await sequence.Play().AsyncWaitForCompletion();

        foreach (Tile _tile in _tiles)
        {
            _tile.SetTile(items[UnityEngine.Random.Range(0, items.Length)]);
        }

        sequence = DOTween.Sequence();

        foreach (Tile _tile in _tiles)
        {
            sequence.Join(_tile.image.transform.DOScale(Vector3.one, moveDelay));
        }
        await sequence.Play().AsyncWaitForCompletion();

        popCount++;

        UIManager.instance.SetScore(_tiles, popCount);

        popTiles.Clear();

        if (popCount > 2)
        {
            UIManager.instance.bombButton.interactable = true;
            UIManager.instance.ShowCombo(popCount);
        }
    }

    public async void Bomb()
    {
        UIManager.instance.bombButton.interactable = false;
        canControl = false;
        popCount = 0;

        UIManager.instance.timer += 0.5f;

        Item item = items[UnityEngine.Random.Range(0, items.Length)];

        foreach (Row _row in rows)
        {
            foreach (Tile _tile in _row.tiles)
            {
                if(_tile.item == item)
                {
                    popTiles.Add(_tile);
                }
            }
        }
        await Pop(popTiles);
        await CheckAllTiles();
    }
}