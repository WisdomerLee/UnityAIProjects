using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.AstarwithWayPoint
{
    public class Edge
    {
        public Node startNode;
        public Node endNode;

        public Edge(Node from, Node to)
        {
            startNode = from;
            endNode = to;
        }

    }

}
