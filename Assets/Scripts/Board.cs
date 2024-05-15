using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board instance = null;

    public Row[] rows;
    private List<Tile> selectedTiles = new List<Tile>();

    private float moveDelay = 0.2f;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        var items = Resources.LoadAll<Item>("Prefabs/Items");

        for (int y = 0; y < rows.Length; y++)
        {
            for(int x = 0; x < rows[y].tiles.Length; x++)
            {
                rows[y].tiles[x].SetTile(x, y, items[UnityEngine.Random.Range(0, items.Length)]);
            }
        }
    }

    public void OnClickTile(Tile _tile)
    {
        Debug.Log($"current tile x = {_tile.x}, y = {_tile.y}, listCount = {selectedTiles.Count}");

        if(selectedTiles.Count == 0)
        {
            selectedTiles.Add(_tile);
        }
        else if(selectedTiles.Count == 1)
        {
            if (Array.Exists(selectedTiles[0].checkTiles, x => x == _tile))
            {
                Debug.Log("Exist");
                selectedTiles.Add(_tile);
                DoSwap(selectedTiles[0], selectedTiles[1]);
            }
            else
            {
                Debug.Log("Not Exist");
                selectedTiles.Clear();
            }
        }
    }

    public async void DoSwap(Tile _tile1, Tile _tile2)
    {
        Debug.Log($"_tile1 x = {_tile1.x}, y = {_tile1.y}\n" +
            $"_tile2 x = {_tile2.x}, y = {_tile2.y}");

        var sequence = DOTween.Sequence();

        sequence.Join(_tile1.image.transform.DOMove(_tile2.transform.position, moveDelay))
                .Join(_tile2.image.transform.DOMove(_tile1.transform.position, moveDelay));

        await sequence.Play().AsyncWaitForCompletion();

        _tile1.image.transform.SetParent(_tile2.transform);
        _tile2.image.transform.SetParent(_tile1.transform);

        Image tmpImage = _tile1.image;
        _tile1.image = _tile2.image;
        _tile2.image = tmpImage;

        selectedTiles.Clear();
    }
}
