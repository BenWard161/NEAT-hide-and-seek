using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeatAgent
{
    private Genome genes;
    private double score;
    private NetworkResolver network;

    public NeatAgent(Genome genome)
    {
        this.genes = genome;
        this.score = 0;
        this.network = new NetworkResolver(genome);
    }

    public void setBehaviours(Genome g)
    {
        this.genes = g;
        this.network = new NetworkResolver(g);
    }

    public void getConnectionsSize()
    {
        // Debug.Log("network has " + genes.getConnectionsSize() + " connections");
    }

    public double[] getActions(double[] input)
    {
        return network.getOutput(input);
    }

    public void addReward(double reward)
    {
        this.score += reward;
    }

    public void setScore(double score)
    {
        this.score = score;
    }

    public double getScore()
    {
        return this.score;
    }

    public Genome getGenome()
    {
        return this.genes;
    }

}
