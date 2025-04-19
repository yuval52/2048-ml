using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlayer : MonoBehaviour
{
    public RandomTileBoard board;
    public RandomGameManager manager;

    public void RequestDecision()
    {
        int action = Random.Range(0, 4);
        board.MakeMove(action);
    }
    public void End(){
        //if(manager.gameCount < manager.countLimit){
        manager.NewGame();
        //}
    }
}
