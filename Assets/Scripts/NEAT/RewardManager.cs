using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager
{
    float previousDistance;

    public RewardManager()
    {
        previousDistance = 0f;
    }

    // Start is called before the first frame update
    public float getReward(Transform agent1, Transform agent2)
    {
        float reward = 0f;
        float distanceToTarget = Vector3.Distance(agent1.transform.position, agent2.transform.position);

        //if there is something blocking the line of sight between the 2 agents
        if (Physics.Linecast(agent1.position, agent2.position))
        {
            reward += 0.01f;
        }
        else
        {
            reward -= 0.01f;
        }


        if (distanceToTarget <= 1.5)
        {
            reward += 10;
        }

        return reward;
    }
}
