using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SjorsGielen.CustomVariables.RuntimeSets;

[RequireComponent(typeof(Agent))]
public class AgentRuntimeSetAdder : AbstractRuntimeSetAdder<Agent>
{
    [SerializeField]
    protected AgentRuntimeSet runTimeSet;

    public override AbstractRuntimeSet<Agent> RuntimeSet => this.runTimeSet;

    public override Agent RuntimesetType => this.GetComponent<Agent>();
}
