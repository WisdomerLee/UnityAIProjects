using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.CrowdSimulation.Flock
{
    public class Flock : MonoBehaviour
    {
        float speed;
        bool turning = false;
        private void Start()
        {
            speed = Random.Range(FlockController.fc.minSpeed, FlockController.fc.maxSpeed);
        }

        private void Update()
        {
            //유니티 구조체 : 반경을 제한하는 것...?
            Bounds bounds = new Bounds(FlockController.fc.transform.position, FlockController.fc.swimLimits * 2);
            //해당 반경에 없으면....?
            if (!bounds.Contains(transform.position))
            {
                turning = true;
            }
            else
            {
                turning = false;
            }

            if (turning)
            {
                Vector3 direction = FlockController.fc.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    Quaternion.LookRotation(direction), 
                    FlockController.fc.rotationSpeed * Time.deltaTime);
            }
            else
            {

                if (Random.Range(0, 100) < 10)
                {
                    speed = Random.Range(FlockController.fc.minSpeed, FlockController.fc.maxSpeed);
                }
                if (Random.Range(0, 100) < 10)
                {
                    ApplyRules();
                }
            }
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        private void ApplyRules()
        {
            GameObject[] gameObjects;
            gameObjects = FlockController.fc.allFlocks;
            //무리의 중심
            Vector3 vcenter = Vector3.zero;
            //충돌 방지를 위한 벡터
            Vector3 vavoid = Vector3.zero;
            float gspeed = 0.01f;
            float nDistance;
            int groupSize = 0;

            foreach(var nextFlock in gameObjects)
            {
                if(nextFlock != gameObject)
                {
                    nDistance = Vector3.Distance(nextFlock.transform.position, transform.position);
                    if(nDistance <= FlockController.fc.neighbourDistance)
                    {
                        vcenter += nextFlock.transform.position;
                        groupSize++;
                        if(nDistance < 1)
                        {
                            vavoid += (transform.position - nextFlock.transform.position);
                        }
                        Flock anotherFlock = nextFlock.GetComponent<Flock>();
                        gspeed += anotherFlock.speed;
                    }
                }
            }
            if (groupSize > 0)
            {
                vcenter = vcenter / groupSize + (FlockController.fc.goalPos - transform.position);
                speed = gspeed / groupSize;
                if(speed > FlockController.fc.maxSpeed)
                {
                    speed = FlockController.fc.maxSpeed;
                }
                Vector3 direction = (vcenter + vavoid) - transform.position;
                if(direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockController.fc.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

}
