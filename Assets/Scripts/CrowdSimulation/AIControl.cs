using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

namespace AITutorial.CrowdSimulation
{
    public class AIControl : MonoBehaviour
    {
        GameObject[] goalLocations;
        NavMeshAgent agent;
        Animator anim;
        float speedMult;
        float detectionRadius = 20;
        float fleeRadius = 10;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            goalLocations = GameObject.FindGameObjectsWithTag("goal");
            int goalIndex = Random.Range(0, goalLocations.Length);
            agent.SetDestination(goalLocations[goalIndex].transform.position);
            anim = GetComponent<Animator>();
            anim.SetFloat("walkOffset", Random.Range(0, 1f));
            ResetAgent();
        }
        
        void ResetAgent()
        {
            speedMult = Random.Range(0.5f, 2);
            anim.SetFloat("speedMult", speedMult);
            agent.speed *= speedMult;
            anim.SetTrigger("isWalking");
            agent.angularSpeed = 120;
            agent.ResetPath();
        }

        public void DetectNewObstacle(Vector3 position)
        {
            if (Vector3.Distance(position, transform.position) < detectionRadius)
            {
                Vector3 fleeDirection = (transform.position - position).normalized;
                Vector3 newgoal = transform.position + fleeDirection * fleeRadius;

                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(newgoal, path);
                if(path.status != NavMeshPathStatus.PathInvalid)
                {
                    agent.SetDestination(path.corners[path.corners.Length - 1]);
                    anim.SetTrigger("isRunning");
                    agent.speed = 10;
                    agent.angularSpeed = 500;
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (agent.remainingDistance < 1)
            {
                ResetAgent();
                int goalIndex = Random.Range(0, goalLocations.Length);
                agent.SetDestination(goalLocations[goalIndex].transform.position);
            }
        }
    }

}
