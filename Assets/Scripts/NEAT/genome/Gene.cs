using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{
    protected int innovation_number;

    public Gene(int innovation_number)
    {
        this.innovation_number = innovation_number;
    }

    public Gene()
    {

    }

    public int getInnovation()
    {
        return this.innovation_number;
    }

    public void setInnovation(int innovation_number)
    {
        this.innovation_number = innovation_number;
    }
}
