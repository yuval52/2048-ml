using UnityEngine;

public class RandomTileRow : MonoBehaviour
{
    public RandomTileCell[] cells { get; private set; }

    private void Awake()
    {
        cells = GetComponentsInChildren<RandomTileCell>();
    }

}
