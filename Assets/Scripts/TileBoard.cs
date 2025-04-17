using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;

public class TileBoard : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileState[] tileStates;

    public TileGrid grid;
    //public TileGrid demoGrid;
    private List<Tile> tiles;
    //private List<Tile> demoTiles;
    private bool waiting;

    public int empty = 16;
    public int prevEmpty = 16;
    public Player player;

    public int highestNum = 2;

    public GameManager manager;

    private int movesWithoutChange = 0;

    private void Awake()
    {
        //grid = GetComponentInChildren<TileGrid>();
        //manager = GetComponent<GameManager>();
        tiles = new List<Tile>(16);
        //demoTiles = new List<Tile>(16);
    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells) {
            cell.tile = null;
        }

        foreach (var tile in tiles) {
            Destroy(tile.gameObject);
        }

        tiles.Clear();

        // foreach (var cell in demoGrid.cells) {
        //     cell.tile = null;
        // }

        // foreach (var tile in demoTiles) {
        //     Destroy(tile.gameObject);
        // }

        // demoTiles.Clear();
        highestNum = 2;
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, grid.transform);

        
        int num = UnityEngine.Random.Range(0, 10);
        if(num == 9){
            tile.SetState(tileStates[1]);
        } else{
            tile.SetState(tileStates[0]);
        }
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
        
        // empty = grid.Size - tiles.Count;
        // player.RewardAdd(3 * (empty-prevEmpty));
        // prevEmpty = empty;
        player.RewardAdd(1f * (float)empty / 16.0f);
    }

    // private void Update()
    // {
    //     if (waiting) return;

    //     if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
    //         Move(Vector2Int.up, 0, 1, 1, 1);
    //     } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
    //         Move(Vector2Int.left, 1, 1, 0, 1);
    //     } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
    //         Move(Vector2Int.down, 0, 1, grid.Height - 2, -1);
    //     } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
    //         Move(Vector2Int.right, grid.Width - 2, -1, 0, 1);
    //     }
    // }

    // public void MakeMove(float[] moves){
    //     if (waiting) return;

    //     int[] moveArr = { 0, 1, 2, 3};

    //     float large = moves[0];
    //     int index = 0;
    //     for (int i = 0; i < 4; i++) {
    //         if (moves[i] > large)
    //         {
    //             large = moves[i];
    //             index = i;
    //         }
    //     }
    //     if (index != 0)
    //     {
    //         float buffer = moves[0];
    //         moves[0] = moves[index];
    //         moves[index] = buffer;

    //         int numBuffer = moveArr[0];
    //         moveArr[0] = moveArr[index];
    //         moveArr[index] = numBuffer;
    //     }

    //     large = moves[1];
    //     index = 1;
    //     for (int i = 1; i < 4; i++)
    //     {
    //         if (moves[i] > large)
    //         {
    //             large = moves[i];
    //             index = i;
    //         }
    //     }
    //     if (index != 1)
    //     {
    //         float buffer = moves[1];
    //         moves[1] = moves[index];
    //         moves[index] = buffer;

    //         int numBuffer = moveArr[1];
    //         moveArr[1] = moveArr[index];
    //         moveArr[index] = numBuffer;
    //     }

    //     if (moves[3] > moves[2])
    //     {
    //         float buffer = moves[2];
    //         moves[2] = moves[3];
    //         moves[3] = buffer;

    //         int numBuffer = moveArr[2];
    //         moveArr[2] = moveArr[3];
    //         moveArr[3] = numBuffer;
    //     }

    //     for (int i = 0; i < moveArr.Length; i++)
    //     {
    //         int move = moveArr[i];
    //         bool changed = false;
    //         if (move == 1)
    //         {
    //             changed = Move(Vector2Int.up, 0, 1, 1, 1);
    //         }
    //         else if (move == 2)
    //         {
    //             changed = Move(Vector2Int.left, 1, 1, 0, 1);
    //         }
    //         else if (move == 0)
    //         {
    //             changed = Move(Vector2Int.down, 0, 1, grid.Height - 2, -1);
    //         }
    //         else if (move == 3)
    //         {
    //             changed = Move(Vector2Int.right, grid.Width - 2, -1, 0, 1);
    //         }

    //         if (changed)
    //         {
    //             break;
    //         }

    //     }


        

    // }

    public void MakeMove(int move){
        if (move == 1)
        {
            Move(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (move == 2)
        {
            Move(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (move == 0)
        {
            Move(Vector2Int.down, 0, 1, grid.Height - 2, -1);
        }
        else if (move == 3)
        {
            Move(Vector2Int.right, grid.Width - 2, -1, 0, 1);
        }
    }

    public bool CanMakeMove(int move)
    {
        if (move == 1)
        {
            return CanMove(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (move == 2)
        {
            return CanMove(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (move == 0)
        {
            return CanMove(Vector2Int.down, 0, 1, grid.Height - 2, -1);
        }
        else if (move == 3)
        {
            return CanMove(Vector2Int.right, grid.Width - 2, -1, 0, 1);
        } else
        {
            return false;
        }
    }

    private bool Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.Occupied) {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed) {
            StartCoroutine(WaitForChanges());
            movesWithoutChange = 0;
            // return changed;
        } else{
            if(movesWithoutChange <= 10){
               player.RewardAdd(-0.1f);
               player.RequestDecision();
               movesWithoutChange++;
            } else{
               player.RewardAdd(-0.1f);
               movesWithoutChange = 0;
               player.End();
            }
            //return changed;
            // player.RewardAdd(-0.1f);
            // player.RequestDecision();
            
        }
        return changed;

        
    }

    public bool CanMove(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.Occupied) {
                    changed |= CanMoveTile(cell.tile, direction);
                }
            }
        }
        return changed;
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.Occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    public bool CanMoveTile(Tile tile, Vector2Int direction)
    {
        //TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        if (adjacent.Occupied)
        {
            if (CanMerge(tile, adjacent.tile))
            {
                //MergeTiles(tile, adjacent.tile);
                return true;
            } else
            {
                return false;
            }
        } else{
            return true;
        }
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.state == b.state && !b.locked;
    }

    private void MergeTiles(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);

        // if(newState.number > highestNum){
        //     highestNum = newState.number;
        //     player.AddReward((int)Math.Log(highestNum, 2));
        // } else{
        //     player.AddReward((int)Math.Log(newState.number, 2));
        // }

        // player.RewardAdd(1f);
        player.AddReward((int)Math.Log(highestNum, 2));
        if(newState.number > highestNum){
            highestNum = newState.number;
            player.AddReward(1f);
        }

        // if(newState.number > highestNum){
        //     highestNum = newState.number;
        // }

        GameManager.Instance.IncreaseScore(newState.number);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i]) {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        waiting = true;

        yield return new WaitForSeconds(manager.moveTime);

        waiting = false;

        foreach (var tile in tiles) {
            tile.locked = false;
        }

        if (tiles.Count != grid.Size) {
            CreateTile();
        }

        if (CheckForGameOver()) {
            GameManager.Instance.GameOver();
        }

        player.RequestDecision();
    }

    public bool CheckForGameOver()
    {
        if (tiles.Count != grid.Size) {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile)) {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile)) {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile)) {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile)) {
                return false;
            }
        }

        return true;
    }

}
