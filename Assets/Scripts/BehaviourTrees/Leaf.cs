using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.BehaviourTrees
{
    //������ ���� �ൿ�� �ִ� ��... :  BehaviourTree����
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
