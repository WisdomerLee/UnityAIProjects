using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace AITutorial.GoalDrivenBehaviour
{
    public class UpdateWorld : MonoBehaviour
    {
        public Text states;
        
        // Update is called once per frame
        void LateUpdate()
        {
            Dictionary<string, int> worldstates = GWorld.Instance.GetWorld().GetStates();
            states.text = "";
            foreach(var s in worldstates)
            {
                states.text += s.Key + ", " + s.Value + "\n";
            }
        }
    }

}
