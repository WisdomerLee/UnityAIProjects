using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.AstarwithWayPoint
{
    //
    public class FollowWP : MonoBehaviour
    {
        Transform goal;
        float speed = 5f;

        float accuracy = 1f;
        float rotSpeed = 2f;

        public GameObject wpManager;
        GameObject[] wps;
        GameObject currentNode;
        int currentWP = 0;

        Graph g;

        private void Start()
        {
            wps = wpManager.GetComponent<WPManager>().waypoints;
            g = wpManager.GetComponent<WPManager>().graph;
            currentNode = wps[0];
        }

        public void GoToHeli()
        {
            g.Astar(currentNode, wps[0]);
            currentWP = 0;
        }

        public void GoToRuin()
        {
            g.Astar(currentNode, wps[1]);
            currentWP = 0;
        }
        private void LateUpdate()
        {
            if (g.pathList.Count == 0 || currentWP == g.pathList.Count)
            {
                return;
            }
            if (Vector3.Distance(g.pathList[currentWP].GetId().transform.position, transform.position) < accuracy)
            {
                currentWP++;
            }
            if (currentWP < g.pathList.Count)
            {
                goal = g.pathList[currentWP].GetId().transform;
                Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
                Vector3 direction = lookAtGoal - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                transform.Translate(0, 0, speed * Time.deltaTime);

            }
        }
    }
}

