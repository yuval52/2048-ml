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
   
    public override void OnActionReceived(ActionBuffers actions){
        //make the chosen move
        board.MakeMove(actions.DiscreteActions[0]);
    }

    public void RewardSet(float reward){
        //sets reward to a value
        SetReward(reward);
    }

    public void RewardAdd(float reward){
        //adds to current reward
        AddReward(reward);
    }

    public void End(){
        //ends curremt episode
        EndEpisode();
    }

    public override void OnEpisodeBegin(){
        //start next game
        manager.NewGame();
    }

    public override void CollectObservations(VectorSensor sensor){
        //input observations into the model
        int[,] matrix = board.grid.GetGridMatrix();
        for(int x = 0; x < 4; x++){
            for(int y = 0; y < 4; y++){
                sensor.AddObservation((int)Math.Log(matrix[x,y], 2));
            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //mask invalid moves
        for(int i = 0; i < 4; i++){
            bool possible = board.CanMakeMove(i);
            actionMask.SetActionEnabled(0, i, possible);
        }
    }
}
