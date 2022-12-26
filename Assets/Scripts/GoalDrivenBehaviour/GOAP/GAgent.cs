using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

namespace AITutorial.GoalDrivenBehaviour
{
    public class SubGoal
    {
        public Dictionary<string, int> sgoals;
        public bool remove;

        public SubGoal(string key, int value, bool remove)
        {
            sgoals = new Dictionary<string, int>();
            sgoals.Add(key, value);
            this.remove = remove;
        }
    }
    
    public class GAgent : MonoBehaviour
    {
        public List<GAction> actions = new List<GAction>();
        public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
        public GInventory inventory = new GInventory();
        public WorldStates beliefs = new WorldStates();

        GPlanner planner;

        Queue<GAction> actionQueue;
        public GAction currentAction;
        SubGoal currentGoal;
        bool invoked = false;
        // Start is called before the first frame update
        protected virtual void Start()
        {
            GAction[] acts = GetComponents<GAction>();
            foreach(GAction action in acts)
            {
                actions.Add(action);
            }
        }
        void CompleteAction()
        {
            currentAction.running = false;
            currentAction.PostPerform();
            invoked = false;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if(currentAction != null && currentAction.running)
            {
                float distanceToTarget = Vector3.Distance(currentAction.target.transform.position, transform.position);
                if (currentAction.agent.hasPath && distanceToTarget < 2f)
                {
                    if (!invoked)
                    {
                        Invoke(nameof(CompleteAction), currentAction.duration);
                        invoked = true;
                    }
                }
                return;
            }


            //움직일 행동, 움직임이 없는 상태라면..
            if(planner == null || actionQueue == null)
            {
                planner = new GPlanner();

                var sortedGoals = from entry in goals orderby entry.Value descending select entry;

                foreach(var sg in sortedGoals)
                {
                    actionQueue = planner.Plan(actions, sg.Key.sgoals, beliefs);
                    if(actionQueue != null)
                    {
                        currentGoal = sg.Key;
                        break;
                    }
                }
            }
            //모든 행동이 다 끝나면..?
            if(actionQueue != null && actionQueue.Count == 0)
            {
                if (currentGoal.remove)
                {
                    goals.Remove(currentGoal);
                }
                planner = null;
            }
            if(actionQueue != null && actionQueue.Count > 0)
            {
                currentAction = actionQueue.Dequeue();
                if (currentAction.PrePerform())
                {
                    
                    if (currentAction.target == null && currentAction.targetTag != "")
                    {
                        currentAction.target = GameObject.FindWithTag(currentAction.targetTag);
                    }

                    if(currentAction.target != null)
                    {
                        currentAction.running = true;
                        currentAction.agent.SetDestination(currentAction.target.transform.position);
                    }
                }
                else
                {
                    actionQueue = null;
                }
            }
        }
    }

}
