using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.BehaviourTrees
{
    public class Selector : Node
    {
        public Selector(string name)
        {
            this.name = name;
        }

        public override Status Process()
        {
            Status childstatus = children[currentChild].Process();
            if(childstatus == Status.RUNNING)
            {
                return childstatus;
            }
            if (childstatus == Status.SUCCESS)
            {
                currentChild = 0;
                return Status.SUCCESS;
            }
            currentChild++;
            if (currentChild >= children.Count)
            {
                currentChild = 0;
                return Status.FAILURE;
            }
            return Status.RUNNING;
        }
    }

}
