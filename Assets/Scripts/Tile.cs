using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Image image;
    [HideInInspector]
    public int x;
    [HideInInspector]
    public int y;
    [HideInInspector]
    public Item item;

    public void SetTile(int _x, int _y, Item _item)
    {
        x = _x;
        y = _y;
        item = _item;

        image.sprite = _item.image;
    }
}