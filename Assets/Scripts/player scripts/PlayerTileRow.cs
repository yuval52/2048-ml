using UnityEngine;

public class PlayerTileRow : MonoBehaviour
{
    public PlayerTileCell[] cells { get; private set; }

    private void Awake()
    {
        cells = GetComponentsInChildren<PlayerTileCell>();
    }

}
