using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance = null;
    public Row[] rows;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        var datas = Resources.LoadAll<Item>("Prefabs/Items");

        Debug.Log(datas.Length);
        for(int y = 0; y < rows.Length; y++)
        {
            for(int x = 0; x < rows[y].tiles.Length; x++)
            {
                rows[y].tiles[x].SetTile(x, y, Resources.LoadAll<Item>("Prefabs/Items")[0]);
            }
        }
    }
}
