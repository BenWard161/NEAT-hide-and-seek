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

        for (int i = 0; i < node_list.Count; i++)
        {
            if (node_list[i].getLayer() == -1)
            {
                input_nodes.Add(node_list[i]);
                continue;
            }
            else if (node_list[i].getLayer() == 2)
            {
                output_nodes.Add(node_list[i]);
            }
            node_list[i].logInputWeights();
        }
    }

    public double[] getOutput(double[] input)
    {
        int number = input_nodes.Count;
        Debug.Log(input_nodes.Count + " input nodes and " + input.Length + " inputs");
        foreach (NodeGene n in input_nodes)
        {
            //Debug.Log("innov num = " + n.getInnovation());
            //Debug.Log("layer = " + n.getLayer());
        }

        for (int i = 0; i < input_nodes.Count - 1; i++)
        {
            input_nodes[i].setOutput(input[i]);
        }

        double[] output = new double[output_nodes.Count];
        for (int i = 0; i < output_nodes.Count; i++)
        {
            output[i] = output_nodes[i].getOutput();
            
        }
        // Debug.Log("output is " + output[0] + " " + output[1] + " " + output[2]);
        return output;
    }
}
