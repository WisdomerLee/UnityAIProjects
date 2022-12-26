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
        //위치만 같으면 같은 곳 >> 
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

    //A* 길찾기 알고리즘
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
        //길 찾기 완료 시
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
                    //미로 생성 스크립트에서 1이 아닌 경우는 빈 공간임 1이면 벽을 생성함 : 곧 이는 움직일 수 있는 곳들임
                    if (maze.map[x, z] != 1)
                    {
                        locations.Add(new MapLocation(x, z));
                    }
                }
            }
            //위치를 섞어서
            locations.Shuffle();

            Vector3 startlocation = new Vector3(locations[0].x * maze.scale, 0, locations[0].z * maze.scale);
            startNode = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0, Instantiate(start, startlocation, Quaternion.identity), null);
            Vector3 goallocation = new Vector3(locations[1].x * maze.scale, 0, locations[1].z * maze.scale);
            goalNode = new PathMarker(new MapLocation(locations[1].x, locations[1].z), 0, 0, 0, Instantiate(end, goallocation, Quaternion.identity), null);

            //찾기 위한 초기화
            open.Clear();
            closed.Clear();

            open.Add(startNode);
            lastPosition = startNode;

        }
        //경로 찾기 함수
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
                //벽일 경우 무시함
                if (maze.map[neighbour.x, neighbour.z] == 1)
                {
                    continue;
                }
                //미로 크기를 벗어난 위치에 있을 경우 무시
                if (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth)
                {
                    continue;
                }

                if (IsClosed(neighbour))
                {
                    continue;
                }
                //이동한 거리
                float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
                //골 위치까지의 거리
                float H = Vector2.Distance(neighbour.ToVector(), goalNode.location.ToVector());
                //총 소모
                float F = G + H;

                GameObject pathBlock = Instantiate(pathPoint, new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity);

                TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();

                values[0].text = "G: " + G.ToString("0.00");
                values[1].text = "H: " + H.ToString("0.00");
                values[2].text = "F: " + F.ToString("0.00");
                //이웃을 찾고 나서 다시 값들을 업데이트 해야 함
                if (!UpdateMarker(neighbour, G, H, F, thisNode))
                {
                    open.Add(new PathMarker(neighbour, G, H, F, pathBlock, thisNode));
                }
            }
            //F값 기준으로 정렬, 그 다음 H (목적지까지의 거리 순으로 재배열
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

        //막힌 것인지 확인하기
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
                //부모를 역으로 계속 추적해 나감
                begin = begin.parent;
            }
            //마지막으로 시작 지점에도 마커를 두기
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
