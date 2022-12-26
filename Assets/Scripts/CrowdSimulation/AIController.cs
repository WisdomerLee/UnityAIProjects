using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.CrowdSimulation
{
    public class AIController : MonoBehaviour
    {
        public GameObject goal;
        NavMeshAgent agent;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.SetDestination(goal.transform.position);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
