using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.Velocity
{

    public class MoveShell : MonoBehaviour
    {
        [SerializeField]
        float speed = 1.5f;
        public float Speed
        {
            get => speed;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(0, 0, Time.deltaTime * speed);
        }

    }

}