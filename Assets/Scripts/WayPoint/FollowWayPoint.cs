using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.WayPointSet
{
    //길 포인트 찾아 따라가기
    public class FollowWayPoint : MonoBehaviour
    {
        public GameObject[] wayPoints;
        int currentWP = 0;

        public float speed = 10.0f;
        public float rotSpeed = 10.0f;
        public float lookAhead = 10.0f;

        GameObject tracker;



        void ProgressTracker()
        {
            if (Vector3.Distance(tracker.transform.position, transform.position) > lookAhead)
            {
                return;
            }

            if (Vector3.Distance(tracker.transform.position, wayPoints[currentWP].transform.position) < 10)
            {
                currentWP++;
            }
            if (currentWP >= wayPoints.Length)
            {
                currentWP = 0;
            }
            tracker.transform.LookAt(wayPoints[currentWP].transform);
            tracker.transform.Translate(0, 0, (speed + 2) * Time.deltaTime);
        }

        private void Start()
        {
            tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            DestroyImmediate(tracker.GetComponent<Collider>());
            tracker.GetComponent<MeshRenderer>().enabled = false;
            tracker.transform.position = transform.position;
            tracker.transform.rotation = transform.rotation;
        }
        private void Update()
        {
            ProgressTracker();

            //transform.LookAt(wayPoints[currentWP].transform);
            #region Slerp Rotation
            Quaternion lookatWP = Quaternion.LookRotation(tracker.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

            #endregion

            transform.Translate(0, 0, speed * Time.deltaTime);

        }


    }

}
