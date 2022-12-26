using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.AstarwithWayPoint
{
    public class Node
    {
        public List<Edge> edgeList = new List<Edge>();
        public Node path = null;
        //��� ��ġ�� ���� ���� ���ӿ�����Ʈ
        GameObject id;


        //A* algorithm���� ���� ��
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