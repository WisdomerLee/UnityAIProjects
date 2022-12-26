using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//해당 내용은 Unity Documentation Sample의 스크립트를 일부 변형한 것..
//Udemy에서 발췌
namespace AITutorial.VectorDevide
{
    public class Drive : MonoBehaviour
    {
        [SerializeField]
        float speed, rotationSpeed;
        [SerializeField]
        Transform fuel;
        [SerializeField]
        bool isAutoPilot = false;
        [SerializeField]
        float movingSpeed = 1f;
        [SerializeField]
        float roationSpeed = 0.2f;

        [SerializeField]
        Transform transGun;
        [SerializeField]
        Transform gun;
        [SerializeField]
        GameObject bulletObject;
        public float Speed
        {
            get => speed;
        }
        // Update is called once per frame
        void Update()
        {
            float translation = Input.GetAxis("Vertical") * speed;
            float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
            translation *= Time.deltaTime;
            rotation *= Time.deltaTime;
            transform.Translate(0, 0, translation);
            transform.Rotate(0, rotation, 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CalculateDistance();
                CalculateAngle();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                isAutoPilot = !isAutoPilot;
            }

            if (CalculateDistance() < 3)
            {
                isAutoPilot = false;
            }

            if (isAutoPilot)
            {
                AutoPilot();
            }

            if (Input.GetKey(KeyCode.Y))
            {
                transGun.RotateAround(transGun.position, transGun.right, -2);
            }
            else if (Input.GetKey(KeyCode.H))
            {
                transGun.RotateAround(transGun.position, transGun.right, 2);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                Instantiate(bulletObject, gun.position, gun.rotation);
            }
        }
        //거리 계산 함수
        float CalculateDistance()
        {
            //피타고라스 정리를 이용한 수학 계산(y제외)
            float distance = Mathf.Sqrt(Mathf.Pow(fuel.position.x - transform.position.x, 2) + Mathf.Pow(fuel.position.z - transform.position.z, 2));
            Vector3 fuelPos = new Vector3(fuel.transform.position.x, 0, fuel.transform.position.z);
            Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
            //유니티 내부 함수
            float uDistance = Vector3.Distance(pos, fuelPos);

            Vector3 toFuel = fuelPos - pos;
            print("Distance :" + distance);
            print("UnityDistance: " + uDistance);
            print("Vector toFuel:" + toFuel.magnitude);
            //아래의 방식이 magnitude계산보다 더 빠름, 숫자는 더 큼..(연산 하나를 덜하기 때문)
            print("Vector sqrtoFuel:" + toFuel.sqrMagnitude);
            return uDistance;
        }
        //각도 계산
        void CalculateAngle()
        {
            Vector3 forward = transform.forward;
            Vector3 fuelDirection = fuel.transform.position - transform.position;

            Debug.DrawRay(transform.position, forward * 10, Color.green, 2);
            Debug.DrawRay(transform.position, fuelDirection, Color.red, 2);

            //수학적인 정의를 그대로 옮겨 코드로 구현 : 벡터 내적
            float dot = forward.x * fuelDirection.x + forward.y * fuelDirection.y + forward.z * fuelDirection.z;

            //해당 계산에서 얻어지는 각도는 radian이므로....
            float angle = Mathf.Acos(Vector3.Dot(forward, fuelDirection) / (forward.magnitude * fuelDirection.magnitude));
            //해당 각도를 360도의 일반 각도 형식으로 바꾸어줌
            print("Angle:" + angle * Mathf.Rad2Deg);
            //유니티 내부 함수를 이용하면...?
            print("UnityAngle:" + Vector3.Angle(forward, fuelDirection));


            int clockwise = 1;
            if (Cross(forward, fuelDirection).z < 0)
            {
                clockwise = -1;
            }
            if (angle * Mathf.Rad2Deg > 10)
            {
                transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockwise * rotationSpeed);
            }
        }
        //수학적인 정의를 이용하여 만든 외적 함수
        Vector3 Cross(Vector3 firstVector, Vector3 secondVector)
        {
            float xMult = firstVector.y * secondVector.z - firstVector.z * secondVector.y;
            float yMult = firstVector.x * secondVector.z - firstVector.z * secondVector.x;
            float zMult = firstVector.x * secondVector.y - firstVector.y * secondVector.x;
            return new Vector3(xMult, yMult, zMult);
        }
        //자동 이동
        void AutoPilot()
        {
            CalculateAngle();
            transform.position += transform.forward * movingSpeed * Time.deltaTime;
        }
    }
}
