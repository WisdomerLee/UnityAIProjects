using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.GoalDrivenBehaviour
{
    public class Register : GAction
    {
        public override bool PrePerform()
        {
            return true;
        }
        public override bool PostPerform()
        {
            return true;
        }
    }

}
