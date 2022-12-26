using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.Vehicle
{
    public class WheelDrive : MonoBehaviour
    {
        public WheelCollider wc;
        public float torque = 200;
        public GameObject wheelMesh;
        public float maxTorque = 200;
        public float maxSteerAngle = 30;

        public float maxBrakeTorque = 500;
        public bool canTurn = false;
        private void Start()
        {
            wc = GetComponent<WheelCollider>();
        }

        public void Go(float accel, float steer, float brake)
        {
            accel = Mathf.Clamp(accel, -1, 1);

            float thrustTorque = accel * torque * maxTorque;
            wc.motorTorque = thrustTorque;
            if (canTurn)
            {
                steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
                wc.steerAngle = steer;
            }
            else
            {
                brake = Mathf.Clamp(brake, -1, 1) * maxBrakeTorque;
                wc.brakeTorque = brake;
            }
            wc.GetWorldPose(out Vector3 position, out Quaternion quat);
            wheelMesh.transform.position = position;
            wheelMesh.transform.rotation = quat;
        }

        //private void Update()
        //{
        //    float a = Input.GetAxis("Vertical");
        //    float s = Input.GetAxis("Horizontal");
        //    Go(a, s);
        //}
    }

}
