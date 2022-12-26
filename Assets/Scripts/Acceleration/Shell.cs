using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.Acceleration
{
    public class Shell : MonoBehaviour
    {
        [SerializeField]
        GameObject explosion;
        [SerializeField]
        float speed = 0f;
        float yspeed = 0;
        float mass = 10;
        float force = 2;
        float drag = 1;
        float gravity = -9.8f;
        float gAccel;
        float acceleration;

        private void Start()
        {
            acceleration = force / mass;
            speed += acceleration * 1;
            gAccel = gravity / mass;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "tank")
            {
                GameObject exp = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(exp, 0.5f);
                Destroy(gameObject);
            }
        }
        // Update is called once per frame
        void LateUpdate()
        {
            speed *= (1 - Time.deltaTime * drag);
            yspeed += gAccel * Time.deltaTime;
            transform.Translate(0, yspeed, speed * Time.deltaTime);
        }
    }

}
