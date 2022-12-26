using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.CrowdSimulation
{
    public class DropCylinder : MonoBehaviour
    {
        public GameObject obstacle;
        GameObject[] agents;

        // Start is called before the first frame update
        void Start()
        {
            agents = GameObject.FindGameObjectsWithTag("agent");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray.origin, ray.direction, out RaycastHit hitInfo))
                {
                    Instantiate(obstacle, hitInfo.point, obstacle.transform.rotation);
                    foreach (var agent in agents)
                    {
                        agent.GetComponent<AIControl>().DetectNewObstacle(hitInfo.point);
                    }
                }
            }
        }
    }

}
