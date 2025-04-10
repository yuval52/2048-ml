using System.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TileBoard board;
    //[SerializeField] private CanvasGroup gameOver;
    [SerializeField] private TextMeshProUGUI scoreText;
    //[SerializeField] private TextMeshProUGUI hiscoreText;

    public Player player;

    public int[] highestTiles = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public float[] percentageTiles = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int highest = 2;

    public GameObject[] sliders;

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

    private void Start()
    {
        //NewGame();
    }

    public void NewGame()
    {
        // reset score
        SetScore(0);
        //hiscoreText.text = LoadHiscore().ToString();

        // // hide game over screen
        // gameOver.alpha = 0f;
        // gameOver.interactable = false;

        // update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
        //Debug.Log(numGames);
        Debug.Log("[" + highestTiles[0] + ", " + highestTiles[1] + ", " + highestTiles[2] + ", " + highestTiles[3] + ", " + highestTiles[4] + ", " + highestTiles[5] + ", " + highestTiles[6] + ", " + highestTiles[7] + ", " + highestTiles[8] + ", " + highestTiles[9] + ", " + highestTiles[10] + "]");
        //Debug.Log("[" + percentageTiles[0] + ", " + percentageTiles[1] + ", " + percentageTiles[2] + ", " + percentageTiles[3] + ", " + percentageTiles[4] + ", " + percentageTiles[5] + ", " + percentageTiles[6] + ", " + percentageTiles[7] + ", " + percentageTiles[8] + ", " + percentageTiles[9] + ", " + percentageTiles[10] + "]");
        //Debug.Log(highest);
        player.RequestDecision();
    }

    private void updateGraph()
    {
        int topIndex = 0;
        for (int i = 0; i < highestTiles.Length; i++)
        {
            if (highestTiles[i] > highestTiles[topIndex])
            {
                topIndex = i;
            }
        }

        for (int i = 0; i < highestTiles.Length; i++)
        {
            sliders[i].GetComponent<Slider>().value = ((float)highestTiles[i] / (float)highestTiles[topIndex]);
            percentageTiles[i] = ((float)highestTiles[i] / (float)highestTiles[topIndex]);
        }
    }

    public void GameOver()
    {
        int log = (int)Math.Log(board.highestNum, 2);
        highestTiles[log - 1] = highestTiles[log - 1] + 1;
        updateGraph();
        //highest = board.highestNum;
        board.enabled = false;
        //player.RewardAdd(-100);
        //numGames++;
        player.End();
        //gameOver.interactable = true;

        //StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        float duration = 0.5f;
        float from = canvasGroup.alpha;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();

        //SaveHiscore();
    }

    // private void SaveHiscore()
    // {
    //     int hiscore = LoadHiscore();

    //     if (score > hiscore) {
    //         PlayerPrefs.SetInt("hiscore", score);
    //     }
    // }

    // private int LoadHiscore()
    // {
    //     return PlayerPrefs.GetInt("hiscore", 0);
    // }

}
