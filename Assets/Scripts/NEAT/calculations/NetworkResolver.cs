using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkResolver
{
    private List<NodeGene>input_nodes = new List<NodeGene>();
    private List<NodeGene> output_nodes = new List<NodeGene>();


    public NetworkResolver(Genome g)
    {
        List<NodeGene> node_list = g.getNodes();

        foreach(NodeGene n in node_list){


            if(n.getLayer() == -1.0)
            {
                input_nodes.Add(n);
            }
            else if(n.getLayer() == 2.0)
            {
                output_nodes.Add(n);
            }
        }
    }

    public double[] getOutput(double[] input)
    {
        int number = input_nodes.Count;
        Debug.Log(input_nodes.Count + " input nodes and " + input.Length + " inputs");
        foreach (NodeGene n in input_nodes)
        {
            Debug.Log("innov num = " + n.getInnovation());
            Debug.Log("layer = " + n.getLayer());
        }

        foreach (NodeGene n in output_nodes)
        {
            Debug.Log("output has " + n.inputs.Count + " input connections");
        }

        for (int i = 0; i < input_nodes.Count - 1; i++)
        {
            input_nodes[i].setOutput(input[i]);
        }

        double[] output = new double[output_nodes.Count];
        for (int i = 0; i < output_nodes.Count; i++)
        {
            double debugDouble = output_nodes[i].getOutput();
            output[i] = output_nodes[i].getOutput();
            
        }
        // Debug.Log("output is " + output[0] + " " + output[1] + " " + output[2]);
        return output;
    }
}
