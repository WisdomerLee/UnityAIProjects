using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AITutorial.APathFindingAlgorithm
{
    public class PathMarker
    {
        public MapLocation location;
        public float G;
        public float H;
        public float F;
        public GameObject marker;
        public PathMarker parent;

        public PathMarker(MapLocation location, float g, float h, float f, GameObject marker, PathMarker parent)
        {
            this.location = location;
            G = g;
            H = h;
            F = f;
            this.marker = marker;
            this.parent = parent;
        }
        //��ġ�� ������ ���� �� >> 
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
                return location.Equals(((PathMarker)obj).location);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }

    //A* ��ã�� �˰���
    public class FindPathAStar : MonoBehaviour
    {
        public Maze maze;
        public Material closedMaterial;
        public Material openMaterial;

        List<PathMarker> open = new List<PathMarker>();
        List<PathMarker> closed = new List<PathMarker>();

        public GameObject start;
        public GameObject end;
        public GameObject pathPoint;

        PathMarker goalNode;
        PathMarker startNode;
        PathMarker lastPosition;
        //�� ã�� �Ϸ� ��
        bool done = false;

        void RemoveAllMarkers()
        {
            GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
            foreach (GameObject marker in markers)
            {
                Destroy(marker);
            }
        }

        void BeginSearch()
        {
            done = false;
            RemoveAllMarkers();

            List<MapLocation> locations = new List<MapLocation>();
            for (int z = 1; z < maze.depth - 1; z++)
            {
                for (int x = 1; x < maze.width - 1; x++)
                {
                    //�̷� ���� ��ũ��Ʈ���� 1�� �ƴ� ���� �� ������ 1�̸� ���� ������ : �� �̴� ������ �� �ִ� ������
                    if (maze.map[x, z] != 1)
                    {
                        locations.Add(new MapLocation(x, z));
                    }
                }
            }
            //��ġ�� ���
            locations.Shuffle();

            Vector3 startlocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
            startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, Instantiate(start, startlocation, Quaternion.identity), null);
            Vector3 goallocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);
            goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0, Instantiate(end, goallocation, Quaternion.identity), null);

            //ã�� ���� �ʱ�ȭ
            open.Clear();
            closed.Clear();

            open.Add(startNode);
            lastPosition = startNode;

        }
        //��� ã�� �Լ�
        void Search(PathMarker thisNode)
        {
            if (thisNode == null)
            {
                return;
            }
            if (thisNode.Equals(goalNode))
            {
                done = true;
                return;
            }

            foreach (MapLocation dir in maze.directions)
            {
                MapLocation neighbour = dir + thisNode.location;
                //���� ��� ������
                if (maze.map[neighbour.x, neighbour.z] == 1)
                {
                    continue;
                }
                //�̷� ũ�⸦ ��� ��ġ�� ���� ��� ����
                if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth)
                {
                    continue;
                }

                if (IsClosed(neighbour))
                {
                    continue;
                }
                //�̵��� �Ÿ�
                float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
                //�� ��ġ������ �Ÿ�
                float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
                //�� �Ҹ�
                float F = G + H;

                GameObject pathBlock = Instantiate(pathPoint, new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity);

                TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();

                values[0].text = "G: " + G.ToString("0.00");
                values[1].text = "H: " + H.ToString("0.00");
                values[2].text = "F: " + F.ToString("0.00");
                //�̿��� ã�� ���� �ٽ� ������ ������Ʈ �ؾ� ��
                if (!UpdateMarker(neighbour, G, H, F, thisNode))
                {
                    open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
                }
            }
            //F�� �������� ����, �� ���� H (������������ �Ÿ� ������ ��迭
            open = open.OrderBy(p => p.F).ThenBy(n => n.H).ToList<PathMarker>();

            PathMarker pm = (PathMarker)open.ElementAt(0);
            closed.Add(pm);
            open.RemoveAt(0);
            pm.marker.GetComponent<Renderer>().material = closedMaterial;

            lastPosition = pm;
        }

        bool UpdateMarker(MapLocation pos, float g, float h, float f, PathMarker pathMarker)
        {
            foreach (var p in open)
            {
                if (p.location.Equals(pos))
                {
                    p.G = g;
                    p.H = h;
                    p.F = f;
                    p.parent = pathMarker;
                    return true;
                }
            }
            return false;
        }

        //���� ������ Ȯ���ϱ�
        bool IsClosed(MapLocation marker)
        {
            foreach (PathMarker p in closed)
            {
                if (p.location.Equals(marker))
                {
                    return true;
                }
            }
            return false;
        }
        //
        void GetPath()
        {
            RemoveAllMarkers();
            PathMarker begin = lastPosition;

            while (!startNode.Equals(begin) && begin != null)
            {
                Instantiate(pathPoint, new Vector3(begin.location.x * maze.scale, 0, begin.location.z * maze.scale), Quaternion.identity);
                //�θ� ������ ��� ������ ����
                begin = begin.parent;
            }
            //���������� ���� �������� ��Ŀ�� �α�
            Instantiate(pathPoint, new Vector3(startNode.location.x * maze.scale, 0, startNode.location.z * maze.scale), Quaternion.identity);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                BeginSearch();
            }
            if (Input.GetKeyDown(KeyCode.C) && !done)
            {
                Search(lastPosition);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                GetPath();
            }
        }
    }

}
