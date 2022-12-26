using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.Navigation
{
    public class AIController : MonoBehaviour
    {
        public NavMeshAgent agent;
        public GameObject target;
        Animator anim;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
        }
        private void Update()
        {
            agent.SetDestination(target.transform.position);
            if(agent.remainingDistance < 2)
            {
                anim.SetBool("isMoving", false);
            }
            else
            {
                anim.SetBool("isMoving", true);
            }
        }
    }

}
