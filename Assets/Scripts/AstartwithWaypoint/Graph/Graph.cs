using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.AstarwithWayPoint
{
    public class Graph
    {
        List<Edge> edges = new List<Edge>();
        List<Node> nodes = new List<Node>();
        //A* 알고리즘으로 선택된 패스만 넣을 것
        public List<Node> pathList = new List<Node>();

        //노드 더하기
        public void AddNode(GameObject id)
        {
            Node node = new Node(id);
            nodes.Add(node);
        }
        //엣지 더하기(끝부분)
        public void AddEdge(GameObject fromNode, GameObject toNode)
        {
            Node from = FindNode(fromNode);
            Node to = FindNode(toNode);

            if (from != null && to != null)
            {
                Edge e = new Edge(from, to);
                edges.Add(e);
                from.edgeList.Add(e);
            }
        }
        //노드 찾기
        Node FindNode(GameObject id)
        {
            foreach (Node n in nodes)
            {
                if (n.GetId() == id)
                {
                    return n;
                }
            }
            return null;
        }

        //패스 구성 함수
        public void ReconstructPath(Node startId, Node endId)
        {
            pathList.Clear();
            pathList.Add(endId);
            var p = endId.cameFrom;
            while (p != startId && p != null)
            {
                pathList.Insert(0, p);
                p = p.cameFrom;
            }
            pathList.Insert(0, startId);
        }


        //두 노드 사이의 거리 계산 : A* 알고리즘..
        float Distance(Node a, Node b)
        {
            return Vector3.SqrMagnitude(a.GetId().transform.position - b.GetId().transform.position);
        }
        //A* 알고리즘에 따른 경로 탐색
        public bool Astar(GameObject startId, GameObject endId)
        {
            if (startId == endId)
            {
                pathList.Clear();
                return false;
            }
            Node start = FindNode(startId);
            Node end = FindNode(endId);
            if (start == null || end == null)
            {
                return false;
            }
            List<Node> open = new List<Node>();
            List<Node> closed = new List<Node>();
            float tentative_g_score = 0;
            bool tentative_is_better;

            start.g = 0;
            start.h = Distance(start, end);
            start.f = start.g + start.h;

            open.Add(start);

            while (open.Count > 0)
            {
                int i = LowestF(open);
                Node thisNode = open[i];
                if (thisNode.GetId() == endId)
                {
                    ReconstructPath(start, end);
                    return true;
                }

                open.RemoveAt(i);
                closed.Add(thisNode);
                Node neighbour;
                foreach (Edge e in thisNode.edgeList)
                {
                    neighbour = e.endNode;
                    if (closed.IndexOf(neighbour) > -1)
                    {
                        continue;
                    }
                    tentative_g_score = thisNode.g + Distance(thisNode, neighbour);
                    if (open.IndexOf(neighbour) == -1)
                    {
                        open.Add(neighbour);
                        tentative_is_better = true;
                    }
                    else if (tentative_g_score < neighbour.g)
                    {
                        tentative_is_better = true;
                    }
                    else
                    {
                        tentative_is_better = false;
                    }
                    if (tentative_is_better)
                    {
                        neighbour.cameFrom = thisNode;
                        neighbour.g = tentative_g_score;
                        neighbour.h = Distance(thisNode, end);
                        neighbour.f = neighbour.g + neighbour.h;
                    }
                }
            }
            return false;
        }

        //F가 가장 낮은 것이 어디에 있는지 찾기

        int LowestF(List<Node> nodeList)
        {
            float lowestf = 0;
            int count = 0;
            int iterationCount = 0;
            lowestf = nodeList[0].f;
            for (int i = 1; i < nodeList.Count; i++)
            {
                if (nodeList[i].f < lowestf)
                {
                    lowestf = nodeList[i].f;
                    iterationCount = count;
                }
                count++;
            }
            return iterationCount;
        }
    }

}
