using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TileBoard board;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI countText;

    public int gameCount = 0;

    [SerializeField] private Player player;

    public int[] highestTiles = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public float[] percentageTiles = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int highest = 2;

    [SerializeField] private GameObject[] sliders;

    public float moveTime = 0.01f;

    public int score { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    public void NewGame()
    {
        // reset score
        SetScore(0);

        // update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;

        //request first agent decision
        player.RequestDecision();
    }

    private void updateGraph()
    {
        //find most common tile reached
        int topIndex = 0;
        for (int i = 0; i < highestTiles.Length; i++)
        {
            if (highestTiles[i] > highestTiles[topIndex])
            {
                topIndex = i;
            }
        }

        //update graph
        for (int i = 0; i < highestTiles.Length; i++)
        {
            sliders[i].GetComponent<Slider>().value = ((float)highestTiles[i] / (float)highestTiles[topIndex]);
            percentageTiles[i] = ((float)highestTiles[i] / (float)highestTiles[topIndex]);
        }
    }

    public void GameOver()
    {
        //save highest number reached and update graph
        int log = (int)Math.Log(board.highestNum, 2);
        highestTiles[log - 1] = highestTiles[log - 1] + 1;
        updateGraph();

        //end the game
        board.enabled = false;
        gameCount++;
        countText.text = gameCount.ToString();
        player.End();
    }


    public void IncreaseScore(int points)
    {
        //increases the current score
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        //sets current score
        this.score = score;
        scoreText.text = score.ToString();
    }

}
