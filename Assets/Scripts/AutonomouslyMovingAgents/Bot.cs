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
            //Ÿ�ٰ� ������Ʈ�� ���� ���� ���
            float relativeHeading = Vector3.Angle(transform.forward, transform.TransformVector(target.transform.forward));
            //Ÿ�ٱ����� ���
            float toTarget = Vector3.Angle(transform.forward, transform.TransformVector(targetDir));
            //Ÿ���� �ڷ� ���ų�(90�� �̻�) Ÿ�ٰ� �ش� ������Ʈ�� �̵��ϴ� ������ 20�� �̸��̸�... Ÿ������ �̵�!
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
        //������Ʈ�� ���� ���� ��������� Ŀ�� ��� ������Ʈ ���η� �������� �������� ������ �߻���...
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
        //���� ������ �ذ��ϰ��� �ؿ��� ....
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

            //���� ������Ʈ �ݶ��̴�
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
                    //���� ����� �ƴ�...
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
