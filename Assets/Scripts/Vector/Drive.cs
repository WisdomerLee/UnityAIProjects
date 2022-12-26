using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ش� ������ Unity Documentation Sample�� ��ũ��Ʈ�� �Ϻ� ������ ��..
//Udemy���� ����
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
        //�Ÿ� ��� �Լ�
        float CalculateDistance()
        {
            //��Ÿ��� ������ �̿��� ���� ���(y����)
            float distance = Mathf.Sqrt(Mathf.Pow(fuel.position.x - transform.position.x, 2) + Mathf.Pow(fuel.position.z - transform.position.z, 2));
            Vector3 fuelPos = new Vector3(fuel.transform.position.x, 0, fuel.transform.position.z);
            Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
            //����Ƽ ���� �Լ�
            float uDistance = Vector3.Distance(pos, fuelPos);

            Vector3 toFuel = fuelPos - pos;
            print("Distance :" + distance);
            print("UnityDistance: " + uDistance);
            print("Vector toFuel:" + toFuel.magnitude);
            //�Ʒ��� ����� magnitude��꺸�� �� ����, ���ڴ� �� ŭ..(���� �ϳ��� ���ϱ� ����)
            print("Vector sqrtoFuel:" + toFuel.sqrMagnitude);
            return uDistance;
        }
        //���� ���
        void CalculateAngle()
        {
            Vector3 forward = transform.forward;
            Vector3 fuelDirection = fuel.transform.position - transform.position;

            Debug.DrawRay(transform.position, forward * 10, Color.green, 2);
            Debug.DrawRay(transform.position, fuelDirection, Color.red, 2);

            //�������� ���Ǹ� �״�� �Ű� �ڵ�� ���� : ���� ����
            float dot = forward.x * fuelDirection.x + forward.y * fuelDirection.y + forward.z * fuelDirection.z;

            //�ش� ��꿡�� ������� ������ radian�̹Ƿ�....
            float angle = Mathf.Acos(Vector3.Dot(forward, fuelDirection) / (forward.magnitude * fuelDirection.magnitude));
            //�ش� ������ 360���� �Ϲ� ���� �������� �ٲپ���
            print("Angle:" + angle * Mathf.Rad2Deg);
            //����Ƽ ���� �Լ��� �̿��ϸ�...?
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
        //�������� ���Ǹ� �̿��Ͽ� ���� ���� �Լ�
        Vector3 Cross(Vector3 firstVector, Vector3 secondVector)
        {
            float xMult = firstVector.y * secondVector.z - firstVector.z * secondVector.y;
            float yMult = firstVector.x * secondVector.z - firstVector.z * secondVector.x;
            float zMult = firstVector.x * secondVector.y - firstVector.y * secondVector.x;
            return new Vector3(xMult, yMult, zMult);
        }
        //�ڵ� �̵�
        void AutoPilot()
        {
            CalculateAngle();
            transform.position += transform.forward * movingSpeed * Time.deltaTime;
        }
    }
}
