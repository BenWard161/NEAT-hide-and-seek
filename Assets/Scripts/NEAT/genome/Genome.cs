using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genome
{
    private Neat neat;

    private List<ConnectionGene> connection_list;
    private List<NodeGene> node_list;

    public Genome(Neat neat)
    {
        this.neat = neat;
        this.connection_list = new List<ConnectionGene>();
        this.node_list = new List<NodeGene>();

    }

    public Genome(Neat neat, List<ConnectionGene> connections, List<NodeGene> nodes)
    {
        this.neat = neat;
        this.connection_list = connections;
        this.node_list = nodes;
    }

    public void mutate()
    {
        System.Random random = new System.Random();

        if (random.NextDouble() <= neat.getNewNodeProb())
        {
            mutate_new_node();
        }
        if (random.NextDouble() <= neat.getNewConnProb())
        {
            mutate_new_connection();
        }
        if (random.NextDouble() <= neat.getRandomWeightProb())
        {
            set_weight_random();
        }
        if (random.NextDouble() <= neat.getWeightShiftProb())
        {
            shift_weight_random();
        }
        if (random.NextDouble() <= neat.getToggleEnableProb())
        {
            mutate_toggle_enabled();
        }
    }

    public void mutate_new_connection()
    {
        // finite search for new connetions as there may be no new viable connections
        for (int i = 0; i < 20; i++)
        {
            System.Random random = new System.Random();

            int index1 = random.Next(node_list.Count);
            int index2 = random.Next(node_list.Count);

            NodeGene node1 = node_list[index1];
            NodeGene node2 = node_list[index2];

            ConnectionGene connection;

            if (node1.getLayer() != node2.getLayer())
            {
                if (node1.getLayer() <= node2.getLayer())
                {
                    connection = neat.makeConnection(node1, node2);
                }
                else
                {
                    connection = neat.makeConnection(node2, node1);
                }
                if (!this.connection_list.Contains(connection))
                {
                    connection.setWeight(getRandomWeight(-1, 1));
                    addConnection(connection);
                    return;
                }
            }
        }
    }
    
    public void mutate_new_node()
    {
        System.Random random = new System.Random();
        int rand_index = random.Next(connection_list.Count);
        if (this.connection_list.Count > 0)
        {
            ConnectionGene connection = this.connection_list[rand_index];
            connection.getTo().removeInput(connection);

            NodeGene start = connection.getFrom();
            NodeGene end = connection.getTo();
            NodeGene middle = neat.getMiddleNode(connection);

            ConnectionGene connection1 = neat.makeConnection(start, middle);
            ConnectionGene connection2 = neat.makeConnection(middle, end);

            addNode(middle);
            addConnection(connection1);
            addConnection(connection2);
        }
    }

    public void shift_weight_random()
    {
        System.Random random = new System.Random();
        int rand_index = random.Next(connection_list.Count);
        double oldWeight = connection_list[rand_index].getWeight();
        connection_list[rand_index].setWeight(oldWeight + (getRandomWeight(-1, 1) * neat.getWeightShiftMult()));
    }

    public void set_weight_random()
    {
        System.Random random = new System.Random();
        int rand_index = random.Next(connection_list.Count);
        connection_list[rand_index].setWeight(getRandomWeight(-1, 1) * neat.getWeightMult());
    }

    public void mutate_toggle_enabled()
    {
        System.Random random = new System.Random();
        ConnectionGene connect = connection_list[(int)(random.NextDouble() * connection_list.Count)];
        connect.setEnabled(!connect.getEnabled());
    }

    public List<NodeGene> getNodes()
    {
        return this.node_list;
    }

    public void addNode(NodeGene n)
    {
        if (!this.node_list.Contains(n))
        {
            this.node_list.Add(n);
        }
    }

    public List<ConnectionGene> getConnections()
    {
        return this.connection_list;
    }

    public void addConnection(ConnectionGene c)
    {
        if (c.getFrom().getLayer() == c.getTo().getLayer())
        {
            Debug.LogError("infinite network loop");
        }

        if (!this.connection_list.Contains(c))
        {
            for (int i = 0; i < connection_list.Count; i++)
            {
                if (c.getInnovation() < connection_list[i].getInnovation())
                {
                    c.getTo().addInput(c);
                    this.connection_list.Insert(i, c);
                    break;
                }
            }
            this.connection_list.Add(c);
        }
    }

    private double getRandomWeight(double min, double max)
    {
        System.Random random = new System.Random();
        return random.NextDouble() * (max - min) + min;
    }

    public int getNetworkSize()
    {
        return connection_list.Count + node_list.Count;
    }

    public int getConnectionsSize()
    {
        foreach (ConnectionGene c in connection_list)
        {
            // Debug.Log("connection from " + c.getFrom().getInnovation() + " to " + c.getTo().getInnovation());
        }
        return connection_list.Count;
    }
}
