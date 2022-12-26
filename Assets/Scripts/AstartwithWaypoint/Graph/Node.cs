using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.AstarwithWayPoint
{
    public class Node
    {
        public List<Edge> edgeList = new List<Edge>();
        public Node path = null;
        //노드 위치를 갖고 있을 게임오브젝트
        GameObject id;


        //A* algorithm에서 쓰일 값
        public float f, g, h;
        public Node cameFrom;

        public Node(GameObject i)
        {
            id = i;
            path = null;
        }
        public GameObject GetId()
        {
            return id;
        }

    }
}