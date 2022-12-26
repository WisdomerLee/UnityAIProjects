using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.GoalDrivenBehaviour
{
    public abstract class GAction : MonoBehaviour
    {
        public string actionName = "Action";
        public float cost = 1f;
        public GameObject target;
        public string targetTag;
        public float duration = 0;
        public WorldState[] preConditions;
        public WorldState[] afterEffects;
        public NavMeshAgent agent;

        public Dictionary<string, int> preconditions;
        public Dictionary<string, int> effects;

        public WorldStates agentBeliefs;
        public WorldStates beliefs;
        public GInventory inventory;

        public bool running = false;

        public GAction()
        {
            preconditions = new Dictionary<string, int>();
            effects = new Dictionary<string, int>();
        }

        public void Awake()
        {
            agent = GetComponent<NavMeshAgent>();

            if(preConditions != null)
            {
                foreach(WorldState w in preConditions)
                {
                    preconditions.Add(w.key, w.value);
                }
            }
            if (afterEffects != null)
            {
                foreach (WorldState w in afterEffects)
                {
                    effects.Add(w.key, w.value);
                }
            }
            inventory = GetComponent<GAgent>().inventory;
            beliefs = GetComponent<GAgent>().beliefs;
        }

        public bool IsArchievable()
        {
            return true;
        }
        public bool IsArchievableGiven(Dictionary<string, int> conditions)
        {
            foreach(KeyValuePair<string, int> p in preconditions)
            {
                if (!conditions.ContainsKey(p.Key))
                {
                    return false;
                }
            }
            return true;
        }

        public abstract bool PrePerform();
        public abstract bool PostPerform();
    }
}
