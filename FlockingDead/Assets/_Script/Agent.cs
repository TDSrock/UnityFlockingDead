using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Agent : MonoBehaviour
{
    //flocking variables
    [SerializeField][Header("Flocking variables")]
    protected float speed = 1f;
    [Range(-1,1)][SerializeField]
    protected float cohesion = .5f, seperation = .5f, alignment = .5f, avoidance = .5f, nesting= .5f;
    [SerializeField]
    protected Vector3 directionVector = Vector3.zero;
    protected Vector3 movementVector = Vector3.zero;
    //Agent type (set this in the inspector)
    [SerializeField]
    protected AgentType agentType = AgentType.Undefined;

    //detection variables
    [SerializeField]
    [Header("Detection variables")]
    protected float spaceRange = 1f;
    [SerializeField]
    protected float sightRange = 3f;
    [SerializeField][Range(0.5f, 10f)]
    protected float nestingDistance = 3f;
    
    [SerializeField]
    protected AgentRuntimeSet allAgentsRutimeSet;

    static readonly float boundary = 10;

    void Awake()
    {
        if (agentType == AgentType.Undefined)
        {
            Debug.LogErrorFormat("{0} Agent doesn't have it's agent type defined.", this.gameObject.name);
            Destroy(this.gameObject);
        }
        //randomize starting position and starting movement direction
        directionVector = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        this.transform.position = new Vector3(Random.Range(-boundary, boundary), this.transform.position.y, Random.Range(-boundary, boundary));
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
                Debug.LogWarningFormat("{0} doesn't have a registered agentType, add behavior if intended. Enum found in FixedUpdate was: {1}", this.gameObject.name, this.agentType);
                break;
        }

        //Act
        CheckSpeed();
        Move(Time.fixedDeltaTime);
    }

    protected void Hunt()
    {
        float closestFound = float.MaxValue;
        Agent prey = null;
        //See

        /** Opdracht 5 
         * Aan de Hunt methode in Agent.cs voeg een manier toe om de dichtstbijzijnde 
         * NIET zombie te vinden die
         * wel binnen de sightrange valt.
         **/


        //Think
        if (prey != null)
        {
            /** Opdracht 5
             * Maak het think stukje af zodat de zombie ook egt zijn prooi zal na jagen
             **/
        }
        else
        {
            //Nest
            if (Vector3.SqrMagnitude(this.transform.position) > nestingDistance * nestingDistance)
            {
                directionVector += -this.transform.position.normalized * nesting;
            }
        }
    }

    protected void Flock()
    {
        //See
        var myPos = this.transform.position;
        float squaredSightRange = sightRange * sightRange;
        foreach (var agent in allAgentsRutimeSet.Items)
        {
            //Nest
            if (Vector3.SqrMagnitude(myPos) > nestingDistance * nestingDistance)
            {
                directionVector += -this.transform.position.normalized * nesting;
            }
            //ignore self
            if (agent == this) continue;
            //sqr magnitude is faster then magnitude
            float squaredDistance = Vector3.SqrMagnitude(myPos - agent.transform.position);
            if (squaredDistance < this.sightRange * this.sightRange) {
                switch (agent.agentType)
                {
                    case AgentType.Human:
                        // Flock with other humans
                        {
                            if (squaredDistance < spaceRange * spaceRange)
                            {
                                /** Opdracht 2
                                 * Maak de seperation code in Agent.cs af.
                                 * 
                                 * Gebruik hiervoor het al bestaande seperation variabel. 
                                 * Implementeer dit in de Flock methode.
                                 **/
                            }
                            else if (squaredDistance < squaredSightRange)
                            {
                                /** Opdracht 1
                                 * Maak de cohesion code in Agent.cs af.
                                 * 
                                 * Gebruik hiervoor het al bestaande cohesion variabel. 
                                 * Implementeer dit in de Flock methode.
                                 **/
                            }
                            if (squaredDistance < squaredSightRange)
                            {
                                /** Opdracht 3
                                 * Maak de alignment code in Agent.cs af.
                                 * 
                                 * Gebruik hiervoor het al bestaande alignment variabel. 
                                 * Implementeer dit in de Flock methode.
                                 **/
                            }
                        }
                        break;
                    case AgentType.Zombie:
                        //Flee from any zombies
                        if (squaredDistance < squaredSightRange)
                        {
                            /** Opdracht 4
                             * Maak de avoidance code in Agent.cs af.
                             * 
                             * Gebruik hiervoor het al bestaande avoidance variabel. 
                             * Implementeer dit in de Flock methode.
                             **/
                        }
                        break;
                    default:
                        Debug.LogWarningFormat("{0} doesn't have a registered agentType, add behavior if intended. Enum found in Flock was: {1}", agent.gameObject.name, this.agentType);
                        break;
                }
            }
        }
    }

    protected void Move(float deltaTime)
    {
        //Set orientation of this agent to what the directionVector is.
        this.transform.rotation = Quaternion.LookRotation(directionVector.normalized);
        this.transform.position += directionVector * deltaTime;
    }

    protected void CheckSpeed()
    {
        directionVector.y = 0;
        float val = directionVector.magnitude;
        if (val > speed)
        {
            directionVector = directionVector.normalized * speed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Draw the space seperation range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, spaceRange);

        //Draw sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, sightRange);

        //Draw the forward vector
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward);

        //Draw the boundry
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(boundary * 2f, 1f, boundary * 2f));

        //Draw the nest
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(Vector3.zero, nestingDistance);
    }
}