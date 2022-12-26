using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.Timespace
{
    public class UpdateMove : MonoBehaviour
    {
        float timeStartOffset = 0;
        bool gotStartTime = false;
        [SerializeField]
        float speed = 0.5f;
        // Update is called once per frame
        void Update()
        {
            if (!gotStartTime)
            {
                timeStartOffset = Time.realtimeSinceStartup;
                gotStartTime = true;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y, Time.realtimeSinceStartup - timeStartOffset);

        }

        private void FixedUpdate()
        {
            transform.Translate(0, 0, Time.fixedDeltaTime * speed);
        }

        private void LateUpdate()
        {
            transform.Translate(0, 0, Time.deltaTime * speed);
        }
    }

}
