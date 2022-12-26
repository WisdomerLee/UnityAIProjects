using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace AITutorial.FiniteStateMachine
{
    //FINITE STATE MACHINE : 처음, 업데이트, 끝이 있음
    public class State
    {
        public enum STATE
        {
            IDLE, PATROL, PURSUE, ATTACK, SLEEP, RUNAWAY
        };

        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        };
        public STATE name;
        protected EVENT stage;
        protected GameObject npc;
        protected Animator anim;
        protected Transform player;
        protected State nextState;
        protected NavMeshAgent agent;

        float visDist = 10f;
        float visAngle = 30f;
        float shootDist = 7f;

        public State(GameObject npc, NavMeshAgent agent, Animator anim, Transform player)
        {
            this.npc = npc;
            this.agent = agent;
            this.anim = anim;
            stage = EVENT.ENTER;
            this.player = player;
        }

        public virtual void Enter()
        {
            stage = EVENT.UPDATE;
        }

        public virtual void Update()
        {
            stage = EVENT.UPDATE;
        }

        public virtual void Exit()
        {
            stage = EVENT.EXIT;
        }
        //Finite State Machine의 상태에 따라 이벤트 조건을 설정
        public State Process()
        {
            if (stage == EVENT.ENTER)
            {
                Enter();
            }
            if(stage == EVENT.UPDATE)
            {
                Update();
            }
            if(stage == EVENT.EXIT)
            {
                Exit();
                return nextState;
            }
            return this;
        }

        public bool IsPlayerBehind()
        {
            Vector3 direction = npc.transform.position - player.position;
            float angle = Vector3.Angle(direction, npc.transform.position);
            if(direction.magnitude<2 && angle < 30)
            {
                return true;
            }
            return false;
        }

        public bool CanSeePlayer()
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            if(direction.magnitude < visDist && angle < visAngle)
            {
                return true;
            }
            return false;
        }
        public bool CanAttackPlayer()
        {
            Vector3 direction = player.position - npc.transform.position;
            bool canAttack = direction.magnitude < shootDist;
            return canAttack;
        }
    }

    public class Idle : State
    {
        public Idle(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
        {
            name = STATE.IDLE;
        }

        public override void Enter()
        {
            anim.SetTrigger("isIdle");
            base.Enter();
        }
        public override void Update()
        {
            if (CanSeePlayer())
            {
                nextState = new Pursue(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if(Random.Range(0, 100) < 10)
            {
                nextState = new Patrol(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
        public override void Exit()
        {
            anim.ResetTrigger("isIdle");
            base.Exit();
        }
    }

    public class Patrol : State
    {
        int currentIndex = -1;

        public Patrol(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
        {
            name = STATE.PATROL;
            agent.speed = 2;
            agent.isStopped = false;
        }

        public override void Enter()
        {
            float lastDist = Mathf.Infinity;
            for(int i = 0; i< GameEnvironment.Singleton.CheckPoints.Count; i++)
            {
                GameObject wayPoint = GameEnvironment.Singleton.CheckPoints[i];
                float distance = Vector3.Distance(npc.transform.position, wayPoint.transform.position);
                if(distance < lastDist)
                {
                    currentIndex = i - 1;
                    lastDist = distance;
                }
            }
            anim.SetTrigger("isWalking");
            base.Enter();
        }
        public override void Update()
        {
            if(agent.remainingDistance < 1)
            {
                if(currentIndex >= GameEnvironment.Singleton.CheckPoints.Count - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }
                agent.SetDestination(GameEnvironment.Singleton.CheckPoints[currentIndex].transform.position);
            }

            if (CanSeePlayer())
            {
                nextState = new Pursue(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
            else if (IsPlayerBehind())
            {
                nextState = new RunAway(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
        public override void Exit()
        {
            anim.ResetTrigger("isWalking");
            base.Exit();
        }
    }

    public class Pursue : State
    {
        public Pursue(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
        {
            name = STATE.PURSUE;
            agent.speed = 5;
            agent.isStopped = false;

        }
        public override void Enter()
        {
            anim.SetTrigger("isRunning");
            base.Enter();
        }
        public override void Update()
        {
            agent.SetDestination(player.position);
            //agent 시스템은 목적지를 바꾸어도 바로 경로로 이동되지 않으므로... 해당 목적지로 이동중인 상태일 때를 확인
            if (agent.hasPath)
            {
                if (CanSeePlayer())
                {
                    nextState = new Attack(npc, agent, anim, player);
                    stage = EVENT.EXIT;
                }
                else if (!CanSeePlayer())
                {
                    nextState = new Patrol(npc, agent, anim, player);
                    stage = EVENT.EXIT;
                }
            }
        }
        public override void Exit()
        {
            base.Exit();
        }
    }

    public class Attack: State
    {
        float rotationSpeed = 2f;
        AudioSource shoot;

        public Attack(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
        {
            name = STATE.ATTACK;
            shoot = npc.GetComponent<AudioSource>();
        }
        public override void Enter()
        {
            anim.SetTrigger("isShooting");
            agent.isStopped = true;
            shoot.Play();
            base.Enter();
        }
        public override void Update()
        {
            Vector3 direction = player.position - npc.transform.position;
            float angle = Vector3.Angle(direction, npc.transform.forward);
            //땅속으로 숙이거나 공중으로 들리는 것을 막기 위함
            direction.y = 0;

            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);
            if (!CanAttackPlayer())
            {
                //Idle : 어느 상태로도 변화 가능한 상태이므로...
                nextState = new Idle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }

        public override void Exit()
        {
            anim.ResetTrigger("isShooting");
            shoot.Stop();
            base.Exit();
        }
    }
    //상태가 늘어날 수록 더 유용..
    public class RunAway : State
    {
        GameObject safeLocation;
        public RunAway(GameObject npc, NavMeshAgent agent, Animator anim, Transform player) : base(npc, agent, anim, player)
        {
            name = STATE.RUNAWAY;
            safeLocation = GameObject.FindGameObjectWithTag("Safe");
        }

        public override void Enter()
        {
            anim.SetTrigger("isRunning");
            agent.isStopped = false;
            agent.speed = 6;
            agent.SetDestination(safeLocation.transform.position);
            base.Enter();
        }
        public override void Update()
        {
            if(agent.remainingDistance < 1)
            {
                nextState = new Idle(npc, agent, anim, player);
                stage = EVENT.EXIT;
            }
        }
        public override void Exit()
        {
            anim.ResetTrigger("isRunning");
            base.Exit();
        }
    }
}
