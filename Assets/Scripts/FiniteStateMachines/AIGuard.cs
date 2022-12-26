using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.FiniteStateMachine
{
    public class AIGuard : MonoBehaviour
    {
        NavMeshAgent agent;
        Animator anim;
        
        State currentState;
        public Transform player;

        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            currentState = new Idle(gameObject, agent, anim, player);
        }

        // Update is called once per frame
        void Update()
        {
            currentState = currentState.Process();
        }
    }

}
