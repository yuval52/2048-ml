using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }

    public int Size => cells.Length;
    public int Height => rows.Length;
    public int Width => Size / Height;

    private void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();

        for (int i = 0; i < cells.Length; i++) {
            cells[i].coordinates = new Vector2Int(i % Width, i / Width);
        }
    }

    public int[,] GetGridMatrix(){
        int[,] matrix = new int[4,4];

        for(int x = 0; x < 4; x++){
            for(int y = 0; y < 4; y++){
                TileCell cell = GetCell(x, y);
                Tile tile = cell.tile;
                if(tile == null){
                    matrix[x,y] = 0;
                } else{
                    matrix[x,y] = tile.state.number;
                }
            }
        }

        return matrix;
    }

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height) {
            return rows[y].cells[x];
        } else {
            return null;
        }
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].Occupied)
        {
            index = Random.Range(0, cells.Length);

            // if (index >= cells.Length) {
            //     index = 0;
            // }

            // // all cells are occupied
            // if (index == startingIndex) {
            //     return null;
            // }
        }

        return cells[index];
    }

}
