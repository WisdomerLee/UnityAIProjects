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
    /// ��(����) ��Ģ
    /// 1. ���� ��� ��ġ�� �̵�
    /// 2. ���� ����Ű�� ��� �������� ����
    /// 3. ���� �ٸ� �ֵ�� �浹 ����
    /// 
    /// ������ ��� ��ġ : ��� ��ġ(����)�� ���ϰ� ���� ���ڷ� ����
    /// ������ ��� ���� : 
    /// ������ �ٸ� �ֵ�� �浹 ���� 
    /// 3���� ������� ���� ���͸� ��� �ջ��Ͽ� �ش� ��ü�� �̵��� ������ ������
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
