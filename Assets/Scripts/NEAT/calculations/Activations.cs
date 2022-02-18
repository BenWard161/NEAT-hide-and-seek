using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activations
{
    public double sigmoid(double val)
    {
        return (1.0 / (1 + Mathf.Exp((float)val))) - 0.5;
    }
}
