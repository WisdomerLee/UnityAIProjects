using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.Acceleration
{
    public class AIShell : MonoBehaviour
    {
        [SerializeField]
        GameObject explosion;
        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            transform.forward = rb.velocity;
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
    }
}
