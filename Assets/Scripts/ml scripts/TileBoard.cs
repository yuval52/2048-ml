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
    private List<Tile> tiles;

    private int empty = 16;
    [SerializeField] private Player player;

    public int highestNum = 2;

    [SerializeField] private GameManager manager;

    private int movesWithoutChange = 0;

    private void Awake()
    {
        //create list of tiles
        tiles = new List<Tile>(16);
    }

    public void ClearBoard()
    {
        //loop through grid and reset the board
        foreach (var cell in grid.cells) {
            cell.tile = null;
        }

        foreach (var tile in tiles) {
            Destroy(tile.gameObject);
        }

        tiles.Clear();

        //resets the highest number reached
        highestNum = 2;
    }

    public void CreateTile()
    {
        //creates a tile in a random empty spot after a move
        Tile tile = Instantiate(tilePrefab, grid.transform);
        
        int num = UnityEngine.Random.Range(0, 10);
        if(num == 9){
            tile.SetState(tileStates[1]);
        } else{
            tile.SetState(tileStates[0]);
        }
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
        
        //rewards for fraction of empty cells
        player.RewardAdd((float)empty / 16.0f);
    }

    public void MakeMove(int move){
        //decode move and make it
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
        //decode and check if move is possible without making it
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

    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;

        //move all tiles in the direction of the move
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
            //wait for move to end
            StartCoroutine(WaitForChanges());
            movesWithoutChange = 0;
        } else{
            //agent made an illegal move
            if(movesWithoutChange <= 10){
               player.RewardAdd(-0.1f);
               player.RequestDecision();
               movesWithoutChange++;
            } else{
               player.RewardAdd(-0.1f);
               movesWithoutChange = 0;
               player.End();
            }
        }

    }

    public bool CanMove(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        //check if any tiles can move in a direction
        for (int x = startX; x >= 0 && x < grid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.Height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.Occupied) {
                    if(CanMoveTile(cell.tile, direction)){
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.Occupied)
            {
                //merge tile if possible
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

        //move tile if possible
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    public bool CanMoveTile(Tile tile, Vector2Int direction)
    {
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        //can tile be moved or merged
        if (adjacent.Occupied)
        {
            return CanMerge(tile, adjacent.tile);
        } else{
            return true;
        }
    }

    private bool CanMerge(Tile a, Tile b)
    {
        //are tiles the same
        return a.state == b.state && !b.locked;
    }

    private void MergeTiles(Tile a, Tile b)
    {
        //merge tiles
        tiles.Remove(a);
        a.Merge(b.cell);

        //set new tile state
        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];
        b.SetState(newState);

        //reward for merge
        player.AddReward((int)Math.Log(highestNum, 2));
        //reward for new highest tile
        if(newState.number > highestNum){
            highestNum = newState.number;
            player.AddReward(1f);
        }

        //increase curreent score
        GameManager.Instance.IncreaseScore(newState.number);
    }

    private int IndexOf(TileState state)
    {
        //return index of tile state
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
        //wait for ongoing board changes
        yield return new WaitForSeconds(manager.moveTime);

        foreach (var tile in tiles) {
            tile.locked = false;
        }

        if (tiles.Count != grid.Size) {
            CreateTile();
        }

        if (CheckForGameOver()) {
            GameManager.Instance.GameOver();
        }

        //request next move from agent
        player.RequestDecision();
    }

    public bool CheckForGameOver()
    {
        //check if board is full
        if (tiles.Count != grid.Size) {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);
            
            //check if tile can move or merge
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
