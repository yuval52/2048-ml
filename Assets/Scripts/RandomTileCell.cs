using UnityEngine;

public class RandomTileCell : MonoBehaviour
{
    public Vector2Int coordinates { get; set; }
    public RandomTile tile { get; set; }

    public bool Empty => tile == null;
    public bool Occupied => tile != null;
}
