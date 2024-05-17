using System;
using System.Collections.Generic;
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
    private float moveDelay = 0.2f;
    private int popCount = 0;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public async void Start()
    {
        items = Resources.LoadAll<Item>("Prefabs/Items");

        for (int y = 0; y < rows.Length; y++)
        {
            for(int x = 0; x < rows[y].tiles.Length; x++)
            {
                rows[y].tiles[x].SetTile(x, y, items[UnityEngine.Random.Range(0, items.Length)]);
            }
        }
        await CheckAllTiles();
    }

    public async void OnClickTile(Tile _tile)
    {
        if (selectedTiles.Count > 1)
            return;

        selectedTiles.Add(_tile);

        if (selectedTiles.Count == 2)
        {
            if (Array.Exists(selectedTiles[0].checkTiles, x => x == _tile))
            {
                await DoSwap(selectedTiles[0], selectedTiles[1]);
                await CheckAllTiles();

                if (canPop != true && popCount == 0)
                {
                    await DoSwap(selectedTiles[1], selectedTiles[0]);
                    canPop = false;
                }
                else
                {
                    await CheckAllTiles();
                }
            }
            selectedTiles.Clear();
            popCount = 0;
        }
    }

    public async Task DoSwap(Tile _tile1, Tile _tile2)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Join(_tile1.image.transform.DOMove(_tile2.transform.position, moveDelay))
                .Join(_tile2.image.transform.DOMove(_tile1.transform.position, moveDelay));

        await sequence.Play().AsyncWaitForCompletion();

        _tile1.image.transform.SetParent(_tile2.transform);
        _tile2.image.transform.SetParent(_tile1.transform);

        Image tmpImage = _tile1.image;
        _tile1.image = _tile2.image;
        _tile2.image = tmpImage;

        Item tmpItem = _tile1.item;
        _tile1.item = _tile2.item;
        _tile2.item = tmpItem;
    }

    public void CheckTile(Tile _tile)
    {
        if (checkedTiles.Count == 0)
        {
            checkedTiles.Add(_tile);
        }

        for (int i = 0; i < _tile.checkTiles.Length; i++)
        {
            if(_tile.checkTiles[i] != null && _tile.checkTiles[i].item == checkedTiles[0].item && !checkedTiles.Contains(_tile.checkTiles[i]) &&
                (_tile.checkTiles[i].x == checkedTiles[0].x || _tile.checkTiles[i].y == checkedTiles[0].y))
            {
                checkedTiles.Add(_tile.checkTiles[i]);
                CheckTile(_tile.checkTiles[i]);
            }
        }
    }

    public async Task CheckAllTiles()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].tiles.Length; x++)
            {
                checkedTiles.Clear();
                CheckTile(rows[y].tiles[x]);
                if(checkedTiles.Count > 2)
                {
                    canPop = true;
                    popTiles.AddRange(checkedTiles);
                    popCount++;
                }
            }
        }
        if(canPop)
        {
            await Pop(popTiles);
        }
    }

    public async Task Pop(List<Tile> _tiles)
    {
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < _tiles.Count; i++)
        {
            sequence.Join(_tiles[i].image.transform.DOScale(Vector3.zero, moveDelay));
        }
        await sequence.Play().AsyncWaitForCompletion();

        for (int i = 0; i < _tiles.Count; i++)
        {
            _tiles[i].SetTile(items[UnityEngine.Random.Range(0, items.Length)]);
        }

        sequence = DOTween.Sequence();

        for (int i = 0; i < _tiles.Count; i++)
        {
            sequence.Join(_tiles[i].image.transform.DOScale(Vector3.one, moveDelay));
        }
        await sequence.Play().AsyncWaitForCompletion();

        popTiles.Clear();
        canPop = false;
        await CheckAllTiles();
    }
}