using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AITutorial.GoalDrivenBehaviour
{
    public sealed class GWorld
    {
        static readonly GWorld instance = new GWorld();
        static WorldStates world;
        //
        static Queue<GameObject> patients;
        static Queue<GameObject> cubicles; 

        static GWorld()
        {
            world = new WorldStates();
            patients = new Queue<GameObject>();
            cubicles = new Queue<GameObject>();

            GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cubicle");
            foreach(GameObject cub in cubes)
            {
                cubicles.Enqueue(cub);
            }
            if (cubes.Length > 0)
            {
                world.ModifyState("FreeCubicle", cubes.Length);
            }
        }
        GWorld()
        {

        }
        public void AddCubile(GameObject c)
        {
            cubicles.Enqueue(c);
        }
        public GameObject RemoveCubicle()
        {
            if (cubicles.Count == 0)
            {
                return null;
            }
            return cubicles.Dequeue();
        }



        //환자 더하기
        public void AddPatient(GameObject p)
        {
            patients.Enqueue(p);
        }
        //환자 빼기
        public GameObject RemovePatient()
        {
            if(patients.Count == 0)
            {
                return null;
            }
            return patients.Dequeue();
        }
        public static GWorld Instance
        {
            get => instance;
        }

        public WorldStates GetWorld()
        {
            return world;
        }
    }

}
