using UnityEngine;

public class PlayerTileCell : MonoBehaviour
{
    public Vector2Int coordinates { get; set; }
    public PlayerTile tile { get; set; }

    public bool Empty => tile == null;
    public bool Occupied => tile != null;
}
