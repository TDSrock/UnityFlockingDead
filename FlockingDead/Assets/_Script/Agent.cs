using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{
    //flocking variables
    [Range(-1,1)][SerializeField]
    protected float cohesion = .5f, seperation = .5f, alignment = .5f, avoidance = .5f;
    [SerializeField]
    protected float speed = 1f;
    [SerializeField]
    protected Vector3 directionVector = Vector3.zero;

    //detection variables
    [SerializeField]
    protected float spaceRange = 1f;
    [SerializeField]
    protected float sightRange = 3f;
    [SerializeField]
    protected AgentRuntimeSet allAgentsRutimeSet;

    //Agent type (set this in the inspector)
    [SerializeField]
    AgentType agentType = AgentType.Undefined;

    void Awake()
    {
        if (agentType == AgentType.Undefined)
        {
            Debug.LogErrorFormat("{0} Agent doesn't have it's agent type defined.", this.gameObject.name);
            Destroy(this.gameObject);
        }

        directionVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
    }

    void FixedUpdate()
    {
        switch (this.agentType)
        {
            case AgentType.Human:
                Flock();
                break;
            case AgentType.Zombie:
                Hunt();
                break;
            case AgentType.Undefined:
                Debug.LogErrorFormat("{0} Agent doesn't have it's agent type defined. This occured after awake! Is there code altering the enum?", this.gameObject.name);
                Destroy(this.gameObject);
                break;
            default:
                Debug.LogWarningFormat("{0} doesn't have a registered agentType, add behavior if intended. Enum found was: {1}", this.gameObject.name, this.agentType);
                break;
        }

        //Act
        Move(Time.fixedDeltaTime);
    }

    protected void Hunt()
    {
        float closestFound = float.MaxValue;
        Agent prey = null;
        //See
        foreach (var agent in allAgentsRutimeSet.Items)
        {
            if (agent.agentType != AgentType.Zombie)
            {
                float distance = Vector3.SqrMagnitude(this.transform.position - agent.transform.position);
                if (distance < sightRange * sightRange && distance < closestFound)
                {
                    closestFound = distance;
                    prey = agent;
                }
            }
        }
        //Think
        if (prey != null)
        {
            // Move towards closest prey.
            directionVector += prey.transform.position - this.transform.position;
        }
    }

    protected void Flock()
    {
        //See
        var myPos = this.transform.position;
        float squaredSightRange = sightRange * sightRange;
        foreach (var agent in allAgentsRutimeSet.Items)
        {
            //ignore self
            if (agent == this) continue;
            //sqr magnitude is faster then magnitude
            float squaredDistance = Vector3.SqrMagnitude(myPos - agent.transform.position);
            if (squaredDistance < this.sightRange * this.sightRange) {
                switch (agent.agentType)
                {
                    case AgentType.Human:
                        //Flock with other humans
                        {
                            //Think
                            //seperation
                            if (squaredDistance < spaceRange * spaceRange)
                            {
                                // Create space. (seperation)
                                directionVector += (myPos - agent.transform.position) * seperation;
                            }
                            else if (squaredDistance < squaredSightRange)
                            {
                                // Flock together. (Cohesion)
                                directionVector += (agent.transform.position - myPos) * cohesion;
                            }
                            if (squaredDistance < squaredSightRange)
                            {
                                // Align movement. (Alignment)
                                directionVector += agent.transform.position * alignment;
                            }
                        }
                        break;
                    case AgentType.Zombie:
                        //Flee from any zombies
                        if (squaredDistance < squaredSightRange)
                        {
                            // Avoid zombies.
                            directionVector += (myPos - agent.transform.position) *  avoidance;
                        }
                        break;

                }
            }
        }
    }

    protected void Move(float deltaTime)
    {
        directionVector.y = 0;
        //Set orientation of this agent to what the directionVector is.
        this.transform.rotation = Quaternion.LookRotation(directionVector);
        if(directionVector.magnitude > speed)
        {
            directionVector = directionVector.normalized;
        }
        this.transform.position += directionVector * speed * deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, spaceRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, sightRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward);
    }
}
