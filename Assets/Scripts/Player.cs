using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Player : Agent
{
    public TileBoard board;
    public GameManager manager;

    //public int maxTurns = 10000;
    //private int turns = 0;
   
    public override void OnActionReceived(ActionBuffers actions){
        //board.MakeMove(actions.ContinuousActions);
        // float[] moves = { actions.ContinuousActions[0], actions.ContinuousActions[1] , actions.ContinuousActions[2] , actions.ContinuousActions[3]};
        // board.MakeMove(moves);
        //if(turns >= maxTurns){
        //    End();
        //}
        //turns++;
        //Debug.Log(actions.DiscreteActions[0]);
        board.MakeMove(actions.DiscreteActions[0]);
        //Debug.Log("->" + actions.DiscreteActions[0]);
    }

    public void RewardSet(float reward){
       SetReward(reward);
       //Debug.Log(reward);
    }

    public void RewardAdd(float reward){
        AddReward(reward);
        //Debug.Log(reward);
    }

    public void End(){
        EndEpisode();
    }

    public override void OnEpisodeBegin(){
        //turns = 0;
        manager.NewGame();
        SetReward(0);
    }

    public override void CollectObservations(VectorSensor sensor){
        int[,] matrix = board.grid.GetGridMatrix();
        for(int x = 0; x < 4; x++){
            for(int y = 0; y < 4; y++){
                sensor.AddObservation((int)Math.Log(matrix[x,y], 2));
            }
        }
        
        // for (int i = 0; i < 4; i++)
        // {
        //     Debug.Log("------------");
        //     if (!board.CanMakeMove(i))
        //     {
        //         ActionMasks.SetActionEnabled(0, i, false); // Disable this action
        //         if(i == 0){
        //             Debug.Log("down");
        //         } else if(i == 1){
        //             Debug.Log("up");
        //         } else if(i == 2){
        //             Debug.Log("left");
        //         } else if(i == 3){
        //             Debug.Log("right");
        //         }
        //     }
        // }
    }

    // public override void CollectDiscreteActionMasks()
    // {
    //     List<int> mask = new List<int>();

    //     for(int i = 0; i < 4; i++)
    //     {

    //     }
    // }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        Debug.Log("------------");
        for(int i = 0; i < 4; i++){
            bool possible = board.CanMakeMove(i);
            actionMask.SetActionEnabled(0, i, possible);
            if(!possible){
                //Debug.Log(i);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            discreteActions[0] = 1;
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            discreteActions[0] = 2;
        } else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            discreteActions[0] = 0;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            discreteActions[0] = 3;
        }
    }


}
