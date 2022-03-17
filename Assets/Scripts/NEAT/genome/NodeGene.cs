using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGene : Gene
{
    private double layer;
    public List<ConnectionGene> inputs = new List<ConnectionGene>();
    private double output;

    public NodeGene(int innovation) : base(innovation)
    {}

    public double getLayer()
    {
        return this.layer;
    }

    public void setLayer(double newLayer)
    {
        this.layer = newLayer;
    }

    public void addInput(ConnectionGene conn)
    {
        this.inputs.Add(conn);
    }

    public void removeInput(ConnectionGene conn)
    {
        if (this.inputs.Contains(conn))
        {
            this.inputs.Remove(conn);
        }
    }

    public void setOutput(double input)
    {
        this.output = input;
    }

    public double getOutput()
    {
        if (this.output != double.NaN & this.output != 0)
        {
            Activations activate = new Activations();
            return activate.sigmoid(this.output);
        }
        this.output = 0.0;
        bool active = false;

        foreach(ConnectionGene c in inputs)
        {
            double prev = c.getFrom().getOutput();
            if (c.getEnabled())
            {
                this.output += c.getWeight() * c.getFrom().getOutput();
                active = true;
            }
            
        }

        if (active)
        {
            Activations activate = new Activations();
            // Debug.Log("output is " + activate.sigmoid(this.output));
            return activate.sigmoid(this.output);
        }
        return 0;
    }

    public void clearOutput()
    {
        this.output = double.NaN;
        foreach(ConnectionGene c in inputs)
        {
            c.getFrom().clearOutput();
        }
    }

    public bool equals(NodeGene node)
    {
        return innovation_number == node.getInnovation();
    }

    public int compareTo(NodeGene node)
    {
        if(this.layer > node.getLayer())
        {
            return 1;
        }
        if(this.layer > node.getLayer())
        {
            return -1;
        }
        return 0;
    }

    public void logInputWeights()
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            Debug.Log("node " + innovation_number + " has an input connection with a weight of " + inputs[i].getWeight());
        }
    }
}
