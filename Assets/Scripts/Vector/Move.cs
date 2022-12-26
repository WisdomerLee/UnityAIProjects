using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.VectorDevide
{
    //Vector 
    public class Move : MonoBehaviour
    {
        [SerializeField]
        Transform target;
        Vector3 direction;
        [SerializeField]
        float speed = 2f;

        private void LateUpdate()
        {
            direction = target.position - transform.position;
            transform.LookAt(target.position);
            if (direction.magnitude > 2)
            {
                Vector3 velocity = direction.normalized * speed * Time.deltaTime;
                transform.position += velocity;
            }

        }
    }

}
