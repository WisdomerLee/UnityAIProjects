using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.GoalDrivenBehaviour
{
    public class GoToWaitingRoom : GAction
{
        public override bool PrePerform()
        {
            return true;
        }
        public override bool PostPerform()
        {
            GWorld.Instance.GetWorld().ModifyState("Waiting", 1);
            GWorld.Instance.AddPatient(gameObject);
            beliefs.ModifyState("atHospital", 1);
            return true;
        }
    }

}
