using AITutorial.AstarwithWayPoint;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.Navigation
{
    public class FollowWP : MonoBehaviour
    {
        
        public GameObject wpManager;
        GameObject[] wps;
        GameObject currentNode;
        NavMeshAgent agent;

        private void Start()
        {
            wps = wpManager.GetComponent<WPManager>().waypoints;
            currentNode = wps[0];
            agent = GetComponent<NavMeshAgent>();
        }

        public void GoToHeli()
        {
            //g.Astar(currentNode, wps[0]);
            agent.SetDestination(wps[0].transform.position);
        }

        public void GoToRuin()
        {
            //g.Astar(currentNode, wps[1]);
            agent.SetDestination(wps[1].transform.position);
        }
        private void LateUpdate()
        {
            
        }
    }

}
