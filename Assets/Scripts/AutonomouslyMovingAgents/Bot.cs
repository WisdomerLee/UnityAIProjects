using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.AutonomouslyMovingAgent
{
    public class Bot : MonoBehaviour
    {
        NavMeshAgent agent;
        public GameObject target;
        Drive ds;
        Vector3 wanderTarget = Vector3.zero;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            ds = target.GetComponent<Drive>();
        }

        void Seek(Vector3 location)
        {
            agent.SetDestination(location);
        }
        void Flee(Vector3 location)
        {
            Vector3 fleeVector = location - transform.position;
            agent.SetDestination(transform.position - fleeVector);
        }

        void Pursue()
        {
            Vector3 targetDir = target.transform.position - transform.position;
            //타겟과 오브젝트의 진행 방향 경로
            float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
            //타겟까지의 경로
            float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));
            //타겟이 뒤로 가거나(90도 이상) 타겟과 해당 오브젝트가 이동하는 각도가 20도 미만이면... 타겟으로 이동!
            if((toTarget > 90 && relativeHeading <20) || ds.currentSpeed < 0.01f)
            {
                Seek(target.transform.position);
                return;
            }
            float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
            Seek(target.transform.position + target.transform.forward * lookAhead);
        }

        void Evade()
        {
            Vector3 targetDir = target.transform.position - transform.position;
            float lookAhead = targetDir.magnitude / (agent.speed + ds.currentSpeed);
            Flee(target.transform.position + target.transform.forward * lookAhead);
        }

        

        void Wander()
        {
            float wanderRadius = 10;
            float wanderDistance = 20;
            float wanderJitter = 1;

            wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
            //localposition to world position
            Vector3 targetWorld = transform.InverseTransformVector(targetLocal);
            Seek(targetWorld);
        }
        //오브젝트가 작을 경우는 상관없으나 커질 경우 오브젝트 내부로 목적지가 정해지는 문제가 발생함...
        void Hide()
        {
            float distance = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;

            for(int i = 0; i< World.Instance.GetHidingSpots().Length; i++)
            {
                Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
                Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;
                if(Vector3.Distance(transform.position, hidePos) < distance)
                {
                    chosenSpot = hidePos;
                    distance = Vector3.Distance(transform.position, hidePos);
                }
            }
            Seek(chosenSpot);
        }
        //위의 문제를 해결하고자 밑에서 ....
        void SmartHide()
        {
            float distance = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;

            Vector3 chosenDir = Vector3.zero;

            GameObject chosenGameObject = World.Instance.GetHidingSpots()[0];

            for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
            {
                Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
                Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;

                if (Vector3.Distance(transform.position, hidePos) < distance)
                {
                    chosenSpot = hidePos;
                    chosenDir = hideDir;
                    chosenGameObject = World.Instance.GetHidingSpots()[i];
                    distance = Vector3.Distance(transform.position, hidePos);
                }
            }

            //숨을 오브젝트 콜라이더
            Collider hideCol = chosenGameObject.GetComponent<Collider>();
            Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);

            float rayDistance = 100;
            hideCol.Raycast(backRay, out RaycastHit hitInfo, rayDistance);


            //
            Seek(hitInfo.point + chosenDir.normalized * 5);
        }

        bool CanSeeTarget()
        {
            Vector3 rayToTarget = target.transform.position - transform.position;
            float lookAngle = Vector3.Angle(transform.forward, rayToTarget);

            if(lookAngle < 60 && Physics.Raycast(transform.position, rayToTarget, out RaycastHit raycastInfo))
            {
                if(raycastInfo.transform.gameObject.tag == "cop")
                {
                    return true;
                }
            }
            return false;
        }

        bool CanSeeMe()
        {
            Vector3 rayFromTarget = transform.position - target.transform.position;
            float lookAngle = Vector3.Angle(target.transform.forward, rayFromTarget);
            if (lookAngle < 60)
            {
                return true;
            }
            return false;
        }
        bool coolDown = false;
        void CoolDownBehaviour()
        {
            coolDown = false;
        }

        bool TargetInRange()
        {
            bool isRange = Vector3.Distance(transform.position, target.transform.position) < 10;
            return isRange;
        }

        // Update is called once per frame
        void Update()
        {
            if (!coolDown)
            {
                if (!TargetInRange())
                {
                    Wander();
                }
                else if (CanSeeTarget() && CanSeeMe())
                {
                    SmartHide();
                    coolDown = true;
                    //좋은 방식은 아님...
                    Invoke(nameof(CoolDownBehaviour), 5);
                }
                else
                {
                    Pursue();
                }
            }
            
        }
    }


}
