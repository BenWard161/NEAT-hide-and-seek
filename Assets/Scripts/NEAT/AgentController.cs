using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController
{
    public GameObject agentObject;
    private Rigidbody rBody;
    public NeatAgent brain;
    public GameObject target;
    public double toTarget;
    public Vector3 startPosition;
    public Quaternion startRotation;


    public AgentController(GameObject g, NeatAgent b, GameObject t)
    {
        this.agentObject = g;
        this.rBody = g.GetComponent<Rigidbody>();
        this.brain = b;
        this.target = t;
        this.startPosition = g.transform.position;
        this.startRotation = g.transform.rotation;
    }

    public float forceMultiplier = 100;
    public void takeActions()
    {
        double[] outputs = this.brain.getActions(CollectObservations());
        //Debug.Log("outputs are " + outputs[0] + " " + outputs[1]);// + " " + outputs[2]);

        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = (float)outputs[0];
        controlSignal.z = (float)outputs[1];
        rBody.AddForce(controlSignal * forceMultiplier);
    }

    public double[] CollectObservations()
    {
        double[] input = new double[2];


        // add direction to target as input
        input[0] = agentObject.transform.position.x - target.transform.position.x;
        input[1] = agentObject.transform.position.z - target.transform.position.z;

        // add inputs from raycasts
        //Debug.Log("inputs are " + input[0] + " " + input[1]);// + " " + outputs[2]);

        return input;
    }

    public void addReward()
    {
        double reward = 0;
        double newToTarget = Vector3.Distance(agentObject.transform.position, target.transform.position);
        reward += 0.01 * (toTarget - newToTarget);
        this.toTarget = newToTarget;

        if (toTarget <= 1)
        {
            reward += 10;
        }

        /*
         * To Do
         * 
         * add reward when agent can see target
         */

        brain.addReward(reward);
    }

    public void setBrain(NeatAgent newBrain)
    {
        this.brain = newBrain;
        this.agentObject.transform.position = this.startPosition;
        this.agentObject.transform.rotation = this.startRotation;
        newBrain.getConnectionsSize();
    }

    public int getNetworkSize()
    {
        return this.brain.getGenome().getNetworkSize();
    }
}
