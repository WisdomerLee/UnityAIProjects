using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.Navigation
{
    public class AgentManager : MonoBehaviour
    {
        List<NavMeshAgent> agents = new List<NavMeshAgent>();
        // Start is called before the first frame update
        void Start()
        {
            GameObject[] tempAgents = GameObject.FindGameObjectsWithTag("AI");
            foreach(GameObject agent in tempAgents)
            {
                agents.Add(agent.GetComponent<NavMeshAgent>());
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
                {
                    foreach(NavMeshAgent agent in agents)
                    {
                        agent.SetDestination(hit.point);
                    }
                }
            }
        }
    }

}
