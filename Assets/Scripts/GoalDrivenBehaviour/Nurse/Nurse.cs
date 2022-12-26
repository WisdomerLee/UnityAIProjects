using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.GoalDrivenBehaviour
{
    public class Nurse : GAgent
    {
        protected override void Start()
        {
            base.Start();
            SubGoal s1 = new SubGoal("treatPatient", 1, true);
            goals.Add(s1, 3);

        }
    }

}
