using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.BehaviourTrees
{
    public class Sequence : Node
    {
        public Sequence(string name)
        {
            this.name = name;
        }
        public override Status Process()
        {
            Status childstatus = children[currentChild].Process();
            if(childstatus != Status.SUCCESS)
            {
                return childstatus;
            }
            currentChild++;
            if(currentChild >= children.Count)
            {
                currentChild = 0;
                return Status.SUCCESS;
            }
            return Status.RUNNING;
        }
    }

}
