using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.Vehicle
{
    //������ �� ������� ������
    public class Flip : MonoBehaviour
    {
        Rigidbody rb;

        float lastTimeChecked;


        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void RightCar()
        {
            transform.position += Vector3.up;
            transform.rotation = Quaternion.LookRotation(transform.forward);
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.up.y > 0 || rb.velocity.magnitude > 1)
            {
                lastTimeChecked = Time.time;
            }

            if (Time.time > lastTimeChecked + 3)
            {
                RightCar();
            }
        }
    }

}
