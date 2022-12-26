using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.GoalDrivenBehaviour
{
    public class Spawn : MonoBehaviour
    {
        public GameObject patientPrefab;
        public int numPatient;
        // Start is called before the first frame update
        void Start()
        {
            for(int i = 0; i< numPatient; i++)
            {
                Instantiate(patientPrefab, transform.position, Quaternion.identity);
            }
            Invoke(nameof(SpawnPatient), 5);
        }

        void SpawnPatient()
        {
            Instantiate(patientPrefab, transform.position, Quaternion.identity);
            Invoke(nameof(SpawnPatient), Random.Range(2, 10));
        }
        // Update is called once per frame
        void Update()
        {

        }
    }

}
