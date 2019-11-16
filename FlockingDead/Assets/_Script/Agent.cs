using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    //flocking variables
    [Range(-1,1)][SerializeField]
    protected float cohesion = .5f, seperation = .5f, alignment = .5f;

    //detection variables
    [SerializeField]
    protected float sightRange;
    [SerializeField]
    protected AgentRuntimeSet allAgentsRutimeSet;
    protected List<Agent> agentsInRange = new List<Agent>();

    //Agent type (set this in the inspector)
    [SerializeField]
    AgentType agentType = AgentType.Undefined;

    void Awake()
    {
        if(agentType == AgentType.Undefined)
        {
            Debug.LogErrorFormat("{0} Agent doesn't have it's agent type defined.", this.gameObject.name);
        }
    }

    void FixedUpdate()
    {
        //See
        agentsInRange.Clear();
        foreach(var agent in allAgentsRutimeSet.Items)
        {
            //ignore self
            if (agent == this) continue;
            //sqr magnitude is faster then magnitude
            if (Vector3.SqrMagnitude(this.transform.position - agent.transform.position) < this.sightRange * this.sightRange)
                this.agentsInRange.Add(agent);
        }

        //Think


        //Act

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, sightRange);

        foreach(var agent in agentsInRange)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(this.transform.position, agent.transform.position);
        }
    }
}
