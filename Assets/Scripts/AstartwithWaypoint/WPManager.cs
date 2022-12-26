using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.AstarwithWayPoint
{
    [System.Serializable]
    public struct Link
    {
        public enum direction { UNI, BI }
        public GameObject node1;
        public GameObject node2;
        public direction dir;
    }
    //��������Ʈ �Ŵ���

    public class WPManager : MonoBehaviour
    {
        public GameObject[] waypoints;
        public Link[] links;
        public Graph graph = new Graph();

        private void Start()
        {
            if (waypoints.Length > 0)
            {
                foreach (var wp in waypoints)
                {
                    graph.AddNode(wp);
                }
                foreach (Link l in links)
                {
                    graph.AddEdge(l.node1, l.node2);
                    //��������� ������ �� ������..
                    if (l.dir == Link.direction.BI)
                    {
                        graph.AddEdge(l.node2, l.node1);
                    }
                }
            }
        }

    }

}
