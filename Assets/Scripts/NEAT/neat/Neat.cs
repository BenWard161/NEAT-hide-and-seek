using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neat
{
    public const int MAX_NODES = 1000000000;

    public const double new_conn_prob = 0.01;
    public const double new_node_prob = 0.1;
    public const double weight_shift_prob = 0.02;
    public const double weight_random_prob = 0.02;
    public const double toggel_enable_prob = 0.001;
    public const double survival_rate = 0.75;
    public const bool elitism = true;
    public const bool maximise = true;

    private double weight_mult = 10;
    private double weight_shift_mult = 0.4;

    //variables to determine weighting in difference function
    private double excess_weight = 1;
    private double disjoind_weight = 1;
    private double weight_diff_weight = 1;

    private double speciation_threashold = 5.0;

    private NeatAgent bestAgent;

    private Dictionary<int, ConnectionGene> all_connections = new Dictionary<int, ConnectionGene>();
    private Dictionary<int, NodeGene> all_nodes = new Dictionary<int, NodeGene>();
    private List<Species> species = new List<Species>();
    private NeatAgent[] agents;

    public int max_agents;
    private int input_size;
    private int output_size;
    
    public Neat(int n_agents, int input_size, int output_size)
    {
        this.max_agents = n_agents;
        this.input_size = input_size;
        this.output_size = output_size;

        this.agents = new NeatAgent[max_agents];

        // generate input nodes with appropriate layer
        // input and output layers set outside of NextDouble() range
        for (int i = 0; i < input_size; i++)
        {
            NodeGene node = makeNode(-1.0);
            //node.setLayer(-1.0);
        }

        //generate output nodes with appropriate layer
        for (int i = 0; i < output_size; i++)
        {
            NodeGene node = makeNode(2.0);
            //node.setLayer(2.0);
        }

        //for (int i = 0; i < input_size; i++)
        //{
        //    NodeGene
        //    for (int k = 0; k < output_size; i++)
        //    {
        //
        //    }
        //}

        for (int i = 0; i < max_agents; i++)
        {
            // default genome will return no output
            // a random connection is added between input and output layers
            Genome g = defaultGenome();

            NeatAgent agent = new NeatAgent(g);
            agents[i] = agent;
        }
    }

    public void step()
    {
        sortParents();
        killUnfitAgents();
        //this.agents = new NeatAgent[max_agents];
        makeNewPopulation();

    }

    private void sortParents()
    {
        this.bestAgent = agents[0];
        foreach (NeatAgent agent in agents)
        {
            if (agent.getScore() > bestAgent.getScore())
            {
                this.bestAgent = agent;
            }

            bool added = false;
            foreach (Species s in species)
            {
                if (s.addIndividual(agent))
                {
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                Species s = new Species(agent, this);
                species.Add(s);
            }
        }

        // remove non viable species
        List<Species> tempList = new List<Species>(species);
        foreach (Species s in tempList)
        {
            if (!s.isSurvivable())
            {
                species.Remove(s);
            }
        }
    }

    public void killUnfitAgents()
    {
        foreach (Species s in species)
        {
            s.killByPercentage(survival_rate);
        }

        foreach (Species s in species)
        {
            if (!s.isSurvivable())
            {
                species.Remove(s);
            }
        }
    }

    public void makeNewPopulation()
    {
        if (elitism) 
        {
            for (int i = 0; i < species.Count; i++)
            {
                agents[i] = species[i].getEliteMember();
            }
        }

        // Debug.Log("Elite agents added");
        // Debug.Log("number of species = " + species.Count);

        //int n = 100;
        //foreach (Species s in species)
        //{
        //    if (s.getMembers().Count < n)
        //    {
        //        n = s.getMembers().Count;
        //    }
        //}
        //Debug.Log("number of members = " + n);

        for (int i = species.Count; i < max_agents; i++)
        {
            // Debug.Log(i + " agents produced");
            NeatAgent new_agent = species[i % (species.Count)].produceNewChild();
            
            agents[i] = new_agent;
        }

        foreach (Species s in species)
        {
            s.clear();
        }
    }

    public Genome defaultGenome()
    {
        Genome default_genome = new Genome(this);
        for (int i = 0; i < (output_size + input_size); i++)
        {
            default_genome.addNode(getNode(i));
        }

        List<NodeGene> nodes = default_genome.getNodes();
        for (int i = 0; i < input_size; i++)
        {
            NodeGene start_node = nodes[i];
            for (int k = input_size; k < (input_size + output_size); k++)
            {
                Debug.Log("node " + i + " to node " + k);
                NodeGene end_node = nodes[k];
                default_genome.addConnection(makeConnection(start_node, end_node));
            }
        }

        return default_genome;
    }

    public ConnectionGene makeConnection(NodeGene n1, NodeGene n2)
    {
        ConnectionGene connect = new ConnectionGene(n1, n2);
        connect.setRandomWeight(-1 * weight_mult, 1 * weight_mult);
        int key = connect.getKey();

        ConnectionGene con;
        if (all_connections.TryGetValue(key, out con))
        {
            connect.setInnovation(con.getInnovation());
        } else
        {
            connect.setInnovation(all_connections.Count);
            all_connections.Add(connect.getKey(), connect);
        }
        return connect;
    }

    public ConnectionGene makeConnection(ConnectionGene c)
    {
        ConnectionGene newConnection = new ConnectionGene(c.getFrom(), c.getTo());
        newConnection.setInnovation(c.getInnovation());
        newConnection.setWeight(c.getWeight());
        return newConnection;
    }

    public NodeGene getMiddleNode(ConnectionGene connection)
    {
        ConnectionGene con;
        if (all_connections.TryGetValue(connection.getKey(), out con))
        {
        }

        if (con.getMiddleInov() != -1)
        {
            return getNode(con.getMiddleInov());
        }
        else
        {
            double startLayer = con.getFrom().getLayer();
            double endLayer = con.getTo().getLayer();
            NodeGene middle = makeNode((startLayer + endLayer) / 2.0);
            con.setMiddleInov(middle.getInnovation());
            return middle;
        }
    }

    public NodeGene makeNode(double layer)
    {
        NodeGene node = new NodeGene(this.all_nodes.Count);
        node.setLayer(layer);
        this.all_nodes.Add(node.getInnovation(), node);
        return node;
    }

    public NodeGene getNode(int innovation)
    {
        //if(innovation <= all_nodes.Count)
        //{
        NodeGene template = all_nodes[innovation];
        NodeGene node = new NodeGene(innovation);
        node.setLayer(template.getLayer());
        return node;
        //}
        //return makeNode();
    }

    public NeatAgent[] getAgents()
    {
        return this.agents;
    }

    public double getExcessWeight()
    {
        return this.excess_weight;
    }

    public double getDisjointWeight()
    {
        return this.disjoind_weight;
    }

    public double getWeightDiffWeight()
    {
        return this.weight_diff_weight;
    }

    public double getWeightMult()
    {
        return this.weight_mult;
    }

    public double getWeightShiftMult()
    {
        return this.weight_shift_mult;
    }
    
    public double getNewConnProb()
    {
        return new_conn_prob;
    }

    public double getNewNodeProb()
    {
        return new_node_prob;
    }

    public double getWeightShiftProb()
    {
        return weight_shift_prob;
    }

    public double getRandomWeightProb()
    {
        return weight_random_prob;
    }

    public double getToggleEnableProb()
    {
        return toggel_enable_prob;
    }

    public double getSpeciationThreashold()
    {
        return speciation_threashold;
    }
}
