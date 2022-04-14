using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public GameObject prefabEnvironment;
    public string seekerTag = "Seeker";
    public string hiderTag = "Hider";

    public int inputSize = 2;
    public int outputSize = 3;
    public int n_agents = 400;
    public int n_generations = 0;
    public int biggestNet = 0;

    [SerializeField] private int maxSteps;

    public Vector3 envOffset = new Vector3(10, 0, 0);

    public Neat neat;
    public PopulationController population;
    public int n_steps;
    public Transform[] startPositions;

    // Start is called before the first frame update
    void Start()
    {
        this.n_steps = 0;
        this.neat = new Neat(n_agents, inputSize, outputSize, "SeekerAgent");
        NeatAgent[] neatAgents = neat.getAgents();
        AgentController[] seekerAgents = new AgentController[n_agents];
        this.startPositions = new Transform[n_agents];

        for (int i = 0; i < neatAgents.Length; i++)
        {
            GameObject env = Instantiate(prefabEnvironment, envOffset * i, Quaternion.identity);

            GameObject seeker = getChildWithTag(env, seekerTag);
            startPositions[i] = seeker.transform;
            GameObject hider = getChildWithTag(env, hiderTag);

            seekerAgents[i] = new AgentController(seeker, (neatAgents[i]), hider);
        }

        this.population = new PopulationController(seekerAgents);
    }

    void FixedUpdate()
    {
        this.n_steps += 1;
        if (this.n_steps > maxSteps)
        {
            this.neat.step();
            this.population.setNewBehaviours(neat.getAgents());
            this.biggestNet = population.biggestNet;
            this.n_steps = 0;
            this.n_generations++;
        }
        this.population.step();
    }

    public GameObject getChildWithTag(GameObject parent, string childTag)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.tag == childTag)
            {
                return child.gameObject;
            }
        }
        // System.Console.WriteLine("child not found");
        return this.gameObject;
    }
}
