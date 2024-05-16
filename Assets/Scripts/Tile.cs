using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Button button;

    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public Item item;

    [HideInInspector] public Tile top;
    [HideInInspector] public Tile bottom;
    [HideInInspector] public Tile left;
    [HideInInspector] public Tile right;

    public Tile[] checkTiles;
    public Image image;

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

        button.onClick.AddListener(delegate { Board.instance.OnClickTile(this); });

        top = y == 0 ? null : Board.instance.rows[y - 1].tiles[x];
        bottom = y == Board.instance.rows.Length - 1 ? null : Board.instance.rows[y + 1].tiles[x];
        left = x == 0 ? null : Board.instance.rows[y].tiles[x - 1];
        right = x == Board.instance.rows[y].tiles.Length - 1 ? null : Board.instance.rows[y].tiles[x + 1];

        checkTiles = new Tile[] { top, bottom, left, right };
    }
}