using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.CrowdSimulation.Flock
{
    /// <summary>
    /// Flocking Rule
    /// 1. Move toward average position of the group
    /// 2. Align with the average heading of the group
    /// 3. Avoid crowding other group members
    /// 떼(무리) 규칙
    /// 1. 떼의 평균 위치로 이동
    /// 2. 떼가 가리키는 평균 방향으로 정렬
    /// 3. 각기 다른 애들과 충돌 방지
    /// 
    /// 무리의 평균 위치 : 모든 위치(벡터)를 더하고 무리 숫자로 나눔
    /// 무리의 평균 방향 : 
    /// 무리의 다른 애들과 충돌 방지 
    /// 3가지 방법으로 구한 벡터를 모두 합산하여 해당 객체가 이동할 방향을 결정함
    /// </summary>
    public class FlockController : MonoBehaviour
    {
        public static FlockController fc;
        public GameObject flockPrefab;
        public int numFlock = 20;
        public GameObject[] allFlocks;
        public Vector3 swimLimits = new Vector3(5, 5, 5);

        public Vector3 goalPos = Vector3.zero;

        [Header("Fish Settings")]
        [Range(0, 5f)]
        public float minSpeed;
        [Range(0, 5f)]
        public float maxSpeed;
        [Range(1, 10f)]
        public float neighbourDistance;
        [Range(1, 5f)]
        public float rotationSpeed;


        // Start is called before the first frame update
        void Start()
        {
            allFlocks = new GameObject[numFlock];
            for(int i = 0; i < numFlock; i++)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                    Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
                allFlocks[i] = Instantiate(flockPrefab, pos, Quaternion.identity);

            }
            fc = this;
            goalPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(Random.Range(0, 100) < 10)
            {
                goalPos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                    Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
            }
        }
    }
}
