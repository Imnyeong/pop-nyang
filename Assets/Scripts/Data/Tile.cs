using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private int x { get; set; }
    private int y { get; set; }

    [HideInInspector] private Tile top => y == 0 ? null : Board.instance.rows[y - 1].tiles[x];
    [HideInInspector] private Tile bottom => y == Board.instance.rows.Length - 1 ? null : Board.instance.rows[y + 1].tiles[x];
    [HideInInspector] private Tile left => x == 0 ? null : Board.instance.rows[y].tiles[x - 1];
    [HideInInspector] private Tile right => x == Board.instance.rows[y].tiles.Length - 1 ? null : Board.instance.rows[y].tiles[x + 1];
    [HideInInspector] public Item item;

    public Tile[] checkTiles => new Tile[] { top, bottom, left, right };

    public Image image;
    public Image icon;

    private void Start()
    {
        y = Array.IndexOf(Board.instance.rows, this.transform.parent.GetComponentInParent<Row>());
        x = Array.IndexOf(Board.instance.rows[y].tiles, this);
    }
    public void SetTile(Item _item)
    {
        item = _item;
        icon.sprite = _item.image;
    }
}