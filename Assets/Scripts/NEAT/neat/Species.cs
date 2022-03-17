using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Species
{
    private List<NeatAgent> members = new List<NeatAgent>();
    private NeatAgent template_member;
    private Neat neat;
    private double total_score = 0;
    
    public Species(NeatAgent individual, Neat n)
    {
        this.template_member = individual;
        this.members.Add(individual);
        this.neat = n;
    }

    public bool addIndividual(NeatAgent agent)
    {
        Genome g1 = template_member.getGenome();
        double diff = difference(g1, agent.getGenome());

        if (diff <= neat.getSpeciationThreashold())
        {
            for (int i = 0; i < members.Count; i++)
            {
                if (agent.getScore() >= ((NeatAgent)members[i]).getScore())
                {
                    members.Insert(i, agent);
                    total_score += agent.getScore();
                    return true;
                }
            }
            members.Add(agent);
            total_score += agent.getScore();
            return true;
        }
        else
        {
            return false;
        }
    }

    public NeatAgent produceNewChild()
    {
        NeatAgent parent1 = this.members[0];
        NeatAgent parent2 = this.members[0];

        bool parents_valid = false;
        while (!parents_valid)
        {
            parent1 = getRandomParent();
            parent2 = getRandomParent();
            if (parent1 != parent2)
            {
                parents_valid = true;
                break;
            }
        }

        if (parent1 == parent2)
        {
            return parent1;
        }

        Genome child;
        if (parent1.getScore() >= parent2.getScore())
        {
            child = crossover(parent1.getGenome(), parent2.getGenome());
        }
        else
        {
            child = crossover(parent2.getGenome(), parent1.getGenome());
        }

        child.mutate();
        NeatAgent child_agent = new NeatAgent(child);
        return child_agent;
    }

    public NeatAgent getRandomParent()
    {
        // Debug.Log("Selecting random parent");
        System.Random random = new System.Random();
        //double r = random.NextDouble() * total_score;
        //foreach (NeatAgent agent in members)
        //{
        //    if (r <= agent.getScore())
        //    {
        //        return agent;
        //    }
        //}
        int index = random.Next(0, members.Count);
        return (NeatAgent)members[index];
    }

    public double difference(Genome g1, Genome g2)
    {
        
        List<ConnectionGene> connections1 = g1.getConnections();
        int g1_size = connections1.Count;
        
        List<ConnectionGene> connections2 = g2.getConnections();
        int g2_size = connections2.Count;

        int index1 = 0;
        int index2 = 0;

        int disjoint = 0;
        int similar = 0;
        double weight_difference = 0;

        while (index1 < g1_size && index2 < g2_size)
        {
            ConnectionGene c1 = connections1[index1];
            ConnectionGene c2 = connections2[index2];

            int i1 = c1.getInnovation();
            int i2 = c2.getInnovation();

           if (i1 == i2)
            {
                similar++;
                weight_difference += Mathf.Abs((float)(c1.getWeight() - c2.getWeight()));
                index1++;
                index2++;
            }
            else if (i1 < i2)
            {
                disjoint++;
                index1++;
            }
            else
            {
                disjoint++;
                index2++;
            }
        }

        int N = Mathf.Max(connections1.Count, connections2.Count);
        if (N < 20)
        {
            N = 1;
        }

        weight_difference /= similar;
        int excess = Mathf.Abs(g1_size - g2_size);

        return (((neat.getExcessWeight() * excess) + (neat.getDisjointWeight() * disjoint))/N) +
            (neat.getWeightDiffWeight() * weight_difference);
    }

    //parent1 must have greater fitness than parent2
    public Genome crossover(Genome parent1, Genome parent2)
    {
        int parent1_size = parent1.getConnections().Count;
        List<ConnectionGene> connections1 = parent1.getConnections();

        int parent2_size = parent2.getConnections().Count;
        List<ConnectionGene> connections2 = parent2.getConnections();

        List<ConnectionGene> childConnections = new List<ConnectionGene>();
        List<NodeGene> childNodes = new List<NodeGene>();

        int index1 = 0;
        int index2 = 0;

        while (index1 < parent1_size && index2 < parent2_size)
        {
            ConnectionGene c1 = connections1[index1];
            ConnectionGene c2 = connections2[index2];

            int i1 = c1.getInnovation();
            int i2 = c2.getInnovation();

            if (i1 == i2)
            {
                childConnections.Add(neat.makeConnection(c1));
                index1++;
                index2++;
            }
            else if (i1 < i2)
            {
                childConnections.Add(neat.makeConnection(c1));
                index1++;
            }
            else
            {
                childConnections.Add(neat.makeConnection(c2));
                index2++;
            }
        }

        for (int i = index1;  i < parent1_size; i++)
        {
            ConnectionGene c = connections1[i];
            childConnections.Add(neat.makeConnection(c));
        }

        foreach (ConnectionGene c in childConnections)
        {
            if (!childNodes.Contains(c.getFrom()))
            {
                NodeGene node = new NodeGene(c.getFrom().getInnovation());
                node.setLayer(c.getFrom().getLayer());
                childNodes.Add(node);
            }
            if (!childNodes.Contains(c.getTo()))
            {
                NodeGene node = new NodeGene(c.getTo().getInnovation());
                node.setLayer(c.getTo().getLayer());
                childNodes.Add(node);
            }
        }

        Genome child = new Genome(neat);
        foreach (NodeGene node in childNodes)
        {
            child.addNode(node);
            //Debug.Log(node.getLayer());
        }
        foreach (ConnectionGene connection in childConnections)
        {
            child.addConnection(connection);
        }

        return child;
    }

    public void killByPercentage(double surviveRate)
    {
        int targetPop = System.Convert.ToInt32(neat.max_agents * surviveRate);
        while (targetPop < this.members.Count)
        {
            removeWorstMember();
        }
    }

    public void removeWorstMember()
    {
        // NeatAgent removedAgent = (NeatAgent)members[members.Count - 1];
        this.members.RemoveAt(this.members.Count - 1);
    }

    public NeatAgent getEliteMember()
    {
        return members[0];
    }

    public void clear()
    {
        this.members.Clear();
        this.total_score = 0;
    }

    public bool isSurvivable()
    {
        if (members.Count >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<NeatAgent> getMembers()
    {
        return this.members;
    }
}
