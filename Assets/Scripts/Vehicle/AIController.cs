using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.Vehicle
{
    public class AIController : MonoBehaviour
    {
        [Header("Car Settings")]
        public float steeringSensitivity = 0.01f;
        public float lookAhead = 30;
        public float maxTorque = 200;
        public float maxSteerAngle = 60;
        public float maxBrakeTorque = 500;
        public float accelCornerMax = 20;
        public float brakeCornerMax = 10;
        public float accelVelocityThreshold = 20;
        public float brakeVelocityThreshold = 10;
        public float antiroll = 5000;









        WheelDrive[] ds;
        public Circuit circuit;
        Vector3 target;
        [SerializeField]
        int currentWP = 0;
        Rigidbody rb;
        public GameObject brakeLight;
        GameObject tracker;
        int currentTrackerWP = 0;

        float trackerSpeed = 15;
        AvoidDetector avoid;

        // Start is called before the first frame update
        void Start()
        {
            ds = GetComponentsInChildren<WheelDrive>();
            target = circuit.waypoints[currentWP].transform.position;
            rb = GetComponent<Rigidbody>();

            tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            DestroyImmediate(tracker.GetComponent<Collider>());
            //tracker.GetComponent<MeshRenderer>().enabled = false;
            tracker.transform.position = transform.position;
            tracker.transform.rotation = transform.rotation;

            avoid = GetComponent<AvoidDetector>();

            GetComponent<AntiRoll>().antiRoll = antiroll;
            foreach (WheelDrive drive in ds)
            {
                drive.maxTorque = maxTorque;
                drive.maxSteerAngle = maxSteerAngle;
                drive.maxBrakeTorque = maxBrakeTorque;
            }
        }

        void ProgressTracker()
        {
            Debug.DrawLine(transform.position, tracker.transform.position);
            if (Vector3.Distance(transform.position, tracker.transform.position) > lookAhead)
            {
                trackerSpeed -= 1;
                if (trackerSpeed < 2)
                {
                    trackerSpeed = 2;
                }
                return;
            }

            if (Vector3.Distance(transform.position, tracker.transform.position) < lookAhead * 0.5f)
            {
                trackerSpeed += 1;
                if (trackerSpeed > 15)
                {
                    trackerSpeed = 15;
                }
            }
            tracker.transform.LookAt(circuit.waypoints[currentTrackerWP].transform.position);
            tracker.transform.Translate(0, 0, trackerSpeed * Time.deltaTime);

            if (Vector3.Distance(tracker.transform.position, circuit.waypoints[currentTrackerWP].transform.position) < 1)
            {
                currentTrackerWP++;
                if (currentTrackerWP >= circuit.waypoints.Length)
                {
                    currentTrackerWP = 0;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            ProgressTracker();
            target = tracker.transform.position;

            Vector3 localTarget;

            if (Time.time < avoid.avoidTime)
            {
                localTarget = tracker.transform.right * avoid.avoidPath;
            }
            else
            {
                localTarget = transform.InverseTransformPoint(target);
            }


            //float distanceToTarget = Vector3.Distance(target, transform.position);
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0, 90);
            float cornerFactor = corner / 90f;


            float a = 1f;
            if (corner > accelCornerMax && rb.velocity.magnitude > accelVelocityThreshold)
            {
                a = Mathf.Lerp(0, 1, 1 - cornerFactor);
            }

            //회전 각도 최대치 조정, 왼쪽, 오른쪽으로 꺾는 것은 현재 속도의 sign값으로 결정
            float s = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(rb.velocity.magnitude);
            float b = 0;
            if (corner > brakeCornerMax && rb.velocity.magnitude > brakeVelocityThreshold)
            {
                b = Mathf.Lerp(0, 1, cornerFactor);
            }

            if (avoid.reverse)
            {
                a = -a;
                s = -s;
            }

            for (int i = 0; i < ds.Length; i++)
            {
                ds[i].Go(a, s, b);
            }

            if (b > 0)
            {
                brakeLight.SetActive(true);
            }
            else
            {
                brakeLight.SetActive(false);
            }
            //if (distanceToTarget < 4)
            //{
            //    currentWP++;
            //    print(currentWP);
            //    if (currentWP >= circuit.waypoints.Length)
            //    {
            //        currentWP = 0;
            //    }
            //    target = circuit.waypoints[currentWP].transform.position;
            //}
        }
    }

}
