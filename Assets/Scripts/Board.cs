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

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        items = Resources.LoadAll<Item>("Prefabs/Items");
        SetAllTiles();
    }

    public async void SetAllTiles()
    {
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

    public async void OnClickTile(Tile _tile)
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

                await DoSwap(selectedTiles[0], selectedTiles[1]);
                await CheckAllTiles();

                if (canPop == false && popCount == 0)
                {
                    await DoSwap(selectedTiles[1], selectedTiles[0]);
                }
            }
            selectedTiles.Clear();
        }
    }

    public async Task DoSwap(Tile _tile1, Tile _tile2)
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

    public async Task CheckAllTiles()
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

    public async Task CheckTile(Tile _tile)
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

    public async Task Pop(List<Tile> _tiles)
    {
        Sequence sequence = DOTween.Sequence();

        foreach(Tile _tile in _tiles)
        {
            sequence.Join(_tile.image.transform.DOScale(Vector3.zero, moveDelay));
        }
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

        popTiles.Clear();

        popCount++;

        if(popCount > 2)
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