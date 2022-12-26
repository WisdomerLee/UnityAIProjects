using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.AutonomouslyMovingAgent
{
    public sealed class World 
    {
        static readonly World instance = new World();
        static GameObject[] hidingSpots;

        static World()
        {
            hidingSpots = GameObject.FindGameObjectsWithTag("hide");
        }
        World()
        {

        }
        public static World Instance
        {
            get => instance;
        }
        public GameObject[] GetHidingSpots()
        {
            return hidingSpots;
        }
    }

}
