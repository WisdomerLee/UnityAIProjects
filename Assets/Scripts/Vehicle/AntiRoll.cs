using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.Vehicle
{
    //
    public class AntiRoll : MonoBehaviour
    {
        public float antiRoll = 5000f;
        public WheelCollider wheelLFront;
        public WheelCollider wheelRFront;
        public WheelCollider wheelLBack;
        public WheelCollider wheelRBack;

        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

        }

        void GroundWheels(WheelCollider wl, WheelCollider wr)
        {
            WheelHit hit;
            float travelL = 1;
            float travelR = 1;

            bool groundedL = wl.GetGroundHit(out hit);
            if (groundedL)
            {
                travelL = (-wl.transform.InverseTransformPoint(hit.point).y - wl.radius) / wl.suspensionDistance;
            }
            bool groundedR = wr.GetGroundHit(out hit);
            if (groundedR)
            {
                travelR = (-wr.transform.InverseTransformPoint(hit.point).y - wr.radius) / wr.suspensionDistance;
            }

            float antiRollForce = (travelL - travelR) * antiRoll;

            if (groundedL)
            {
                rb.AddForceAtPosition(wl.transform.up * -antiRollForce, wl.transform.position);
            }
            if (groundedR)
            {
                rb.AddForceAtPosition(wr.transform.up * antiRollForce, wr.transform.position);
            }
        }
        private void Update()
        {
            GroundWheels(wheelLFront, wheelRFront);
            GroundWheels(wheelLBack, wheelRBack);
        }
    }

}
