using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.BehaviourTrees
{
    //리프는 실제 행동이 있는 곳... :  BehaviourTree에서
    public class Leaf : Node
    {
        public System.Func<Status> ProcessMethod;

        public Leaf()
        {

        }

        public Leaf(string name, Func<Status> processMethod)
        {
            this.name = name;
            ProcessMethod = processMethod;
        }

        public override Status Process()
        {
            if(ProcessMethod != null)
            {
                return ProcessMethod();
            }
            return Status.FAILURE;
        }
    }

}
