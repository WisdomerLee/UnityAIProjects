using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AITutorial.BehaviourTrees
{
    public class RobberBehaviour : MonoBehaviour
    {
        BehaviourTree tree;
        public GameObject diamond;
        public GameObject van;
        public GameObject backdoor;
        public GameObject frontdoor;

        [Range(0, 1000)]
        public int money = 800;

        NavMeshAgent agent;

        public enum ActionState { IDLE, WORKING};
        ActionState state = ActionState.IDLE;

        Node.Status treeStatus = Node.Status.RUNNING;
        // Start is called before the first frame update
        void Start()
        {
            agent = GetComponent<NavMeshAgent>();

            tree = new BehaviourTree();
            Sequence steal = new Sequence("Steal Something");
            Leaf goTobackDoor = new Leaf("Go to Backdoor", GoToBackdoor);
            Leaf gotoFrontdoor = new Leaf("Go to Frontdoor", GoToFrontdoor);
            Leaf goToDiamond = new Leaf("Go to Diamond", GoToDiamond);
            Leaf goToVan = new Leaf("Go to Van", GoToVan);
            Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
            Selector opendoor = new Selector("Open Door");

            
            opendoor.AddChild(gotoFrontdoor);
            opendoor.AddChild(goTobackDoor);

            steal.AddChild(hasGotMoney);
            steal.AddChild(opendoor);
            steal.AddChild(goToDiamond);
            
            steal.AddChild(goToVan);
            tree.AddChild(steal);

            tree.PrintTree();

        }

        public Node.Status HasMoney()
        {
            if (money >= 500)
            {
                return Node.Status.FAILURE;
            }
            return Node.Status.SUCCESS;
        }

        public Node.Status GoToBackdoor()
        {
            return GoToDoor(backdoor);
        }
        public Node.Status GoToFrontdoor()
        {
            return GoToDoor(frontdoor);
        }

        public Node.Status GoToDiamond()
        {
            Node.Status status = GoToLocation(diamond.transform.position);
            if(status == Node.Status.SUCCESS)
            {
                diamond.transform.parent = gameObject.transform;
            }
            return status;
        }

        public Node.Status GoToVan()
        {
            Node.Status status = GoToLocation(van.transform.position);
            if(status == Node.Status.SUCCESS)
            {
                money += 300;
                diamond.SetActive(false);
            }
            return status;
        }

        public Node.Status GoToDoor(GameObject door)
        {
            Node.Status status = GoToLocation(door.transform.position);
            if (status == Node.Status.SUCCESS)
            {
                if (!door.GetComponent<Lock>().isLocked)
                {
                    door.SetActive(false);
                    return Node.Status.SUCCESS;
                }
                return Node.Status.FAILURE;
            }
            else
            {
                return status;
            }
        }


        Node.Status GoToLocation(Vector3 destination)
        {
            float distancetoTarget = Vector3.Distance(destination, transform.position);
            if(state == ActionState.IDLE)
            {
                agent.SetDestination(destination);
                state = ActionState.WORKING;
            }
            else if(Vector3.Distance(agent.pathEndPosition, destination)>= 2)
            {
                state = ActionState.IDLE;
                return Node.Status.FAILURE;
            }
            else if(distancetoTarget < 2)
            {
                state = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }
            return Node.Status.RUNNING;
        }

        // Update is called once per frame
        void Update()
        {
            if(treeStatus != Node.Status.SUCCESS)
            {
                treeStatus = tree.Process();
            }
        }
    }

}
