using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController
{
    private AgentController[] agents;
    private AgentController[] targets;
    public GameObject environmentPrefab;
    public int biggestNet = 0;

    public PopulationController(AgentController[] agents)//, AgentController[] targets)
    {
        this.agents = agents;
        //this.targets = targets;
    }

    public void step ()
    {
        foreach (AgentController agent in agents)
        {
            agent.takeActions();
        }

        //foreach (AgentController target in targets)
        //{
        //    target.takeActions();
        //}

        foreach (AgentController agent in agents)
        {
            agent.addReward();
        }
    }

    public void setNewBehaviours(NeatAgent[] newBrains)
    {
        int min = 0;
        for (int i = 0; i < agents.Length; i++)
        {
            if (min < ((NeatAgent)newBrains[i]).getGenome().getNetworkSize())
            {
                min = ((NeatAgent)newBrains[i]).getGenome().getNetworkSize();
            }
            agents[i].setBrain(newBrains[i]);

        }
        this.biggestNet = min;
    }

    public AgentController[] getAgentControllers()
    {
        return this.agents;
    }
}
