using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.Velocity
{
    public class FireShell : MonoBehaviour
    {
        [SerializeField]
        GameObject bullet, turret, enemy;
        [SerializeField]
        Transform turretBase;
        [SerializeField]
        float speed = 15;
        float moveSpeed = 3;
        [SerializeField]
        float rotationSpeed = 2;

        //총알 생성
        private void CreateBullet()
        {
            GameObject shell = Instantiate(bullet, turret.transform.position, Quaternion.identity);
            shell.GetComponent<Rigidbody>().velocity = speed * turretBase.forward;

        }

        float? RotateTurret()
        {
            float? angle = CalculateAngle(true);
            if (angle != null)
            {
                turretBase.localEulerAngles = new Vector3(360f - (float)angle, 0, 0);
            }
            return angle;
        }

        float? CalculateAngle(bool low)
        {
            Vector3 targetDirection = enemy.transform.position - transform.position;
            float y = targetDirection.y;
            targetDirection.y = 0;
            float x = targetDirection.magnitude - 1;
            float gravity = 9.8f;
            float sSqr = speed * speed;
            float underTheSqrRoot = (sSqr * sSqr) - gravity * (gravity * x * x + 2 * y * sSqr);

            if (underTheSqrRoot >= 0)
            {
                float root = Mathf.Sqrt(underTheSqrRoot);
                float highAngle = sSqr + root;
                float lowAngle = sSqr - root;
                if (low)
                {
                    return (Mathf.Atan2(lowAngle, gravity * x) * Mathf.Rad2Deg);
                }
                else
                {
                    return (Mathf.Atan2(highAngle, gravity * x) * Mathf.Rad2Deg);
                }
            }
            else
            {
                return null;
            }
        }
        //궤적 계산 함수
        //Vector3 CalculateTrajectory()
        //{
        //    Vector3 towardenemy = enemy.transform.position - transform.position;
        //    Vector3 enemyVelocity = enemy.transform.forward * enemy.GetComponent<Drive>().Speed;
        //    float bulletSpeed = bullet.GetComponent<MoveShell>().Speed;
        //    //벡터 2차방정식
        //    float a = Vector3.Dot(enemyVelocity, enemyVelocity) - bulletSpeed * bulletSpeed;
        //    float b = Vector3.Dot(towardenemy, enemyVelocity);
        //    float c = Vector3.Dot(towardenemy, towardenemy);
        //    float d = b * b - a * c;
        //    if (d < 0.1f)
        //    {
        //        return Vector3.zero;
        //    }
        //    float sqrt = Mathf.Sqrt(d);
        //    float t1 = (-b - sqrt) / c;
        //    float t2 = (-b + sqrt) / c;

        //    float t = 0;
        //    if(t1<0 && t2 < 0)
        //    {
        //        return Vector3.zero;
        //    }
        //    else if (t1 < 0)
        //    {
        //        t = t2;
        //    }
        //    else if (t2 < 0)
        //    {
        //        t = t1;
        //    }
        //    else
        //    {
        //        t = Mathf.Max(new float[] { t1, t2 });
        //    }
        //    return t * towardenemy + enemyVelocity;
        //}
        // Update is called once per frame
        void Update()
        {
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            float? angle = RotateTurret();
            if (angle != null)
            {
                CreateBullet();
            }
            else
            {
                transform.Translate(0, 0, moveSpeed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CreateBullet();
            }
        }
    }

}
