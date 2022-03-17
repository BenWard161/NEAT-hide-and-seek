using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionGene : Gene
{
    private NodeGene from;
    private NodeGene to;

    private double weight;
    private bool isEnabled;

    private int middleInov;
    public ConnectionGene(NodeGene from, NodeGene to) : base()
    {
        this.from = from;
        this.to = to;
        this.middleInov = -1;
        this.isEnabled = true;
    }

    public NodeGene getFrom()
    {
        return this.from;
    }

    public void setFrom(NodeGene newFrom)
    {
        this.from = newFrom;
    }

    public NodeGene getTo()
    {
        return this.to;
    }

    public void setTo(NodeGene newTo)
    {
        this.to = newTo;
    }

    public double getWeight()
    {
        return this.weight;
    }

    public void setWeight(double newWeight)
    {
        this.weight = newWeight;
    }

    public bool getEnabled()
    {
        return this.isEnabled;
    }

    public void setEnabled(bool newEnabled)
    {
        this.isEnabled = newEnabled;
    }

    public bool equals(ConnectionGene g)
    {
        ConnectionGene c = (ConnectionGene)g;
        return (from.Equals(c.from) && to.Equals(c.to));
    }

    public int getKey()
    {
        return from.getInnovation() * Neat.MAX_NODES + to.getInnovation();
    }

    public int getMiddleInov()
    {
        return middleInov;
    }

    public void setMiddleInov(int middleInov)
    {
        this.middleInov = middleInov;
    }

    public void setRandomWeight(double min, double max)
    {
        System.Random random = new System.Random();
        this.weight = random.NextDouble() * (max - min) + min;
    }

}
