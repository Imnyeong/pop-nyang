using System;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Button button;

    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public Item item;

    public Tile top => y == 0 ? null : Board.instance.rows[y - 1].tiles[x];
    public Tile bottom => y == Board.instance.rows.Length - 1 ? null : Board.instance.rows[y + 1].tiles[x];
    public Tile left => x == 0 ? null : Board.instance.rows[y].tiles[x - 1];
    public Tile right => x == Board.instance.rows[y].tiles.Length - 1 ? null : Board.instance.rows[y].tiles[x + 1];

    public Tile[] checkTiles => new Tile[] { top, bottom, left, right };
    public Image background;
    public Image image;

    public void Start()
    {
        //y = Array.IndexOf(Board.instance.rows, this.transform.parent.GetComponentInParent<Row>());
        //x = Array.IndexOf(Board.instance.rows[y].tiles, this);
        button.onClick.AddListener(delegate { Board.instance.OnClickTile(this); });
    }

    public void SetTile(Item _item)
    {
        item = _item;
        image.sprite = _item.image;
    }
    public void SetTile(int _x, int _y, Item _item)
    {
        x = _x;
        y = _y;

        item = _item;
        image.sprite = _item.image;
    }
}