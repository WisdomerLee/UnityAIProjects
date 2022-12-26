using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AITutorial.FiniteStateMachine
{
    public sealed class GameEnvironment
    {
        static GameEnvironment instance;
        List<GameObject> checkPoints = new List<GameObject>();
        public List<GameObject> CheckPoints
        {
            get => checkPoints;
        }
        public static GameEnvironment Singleton
        {
            get
            {
                if(instance == null)
                {
                    instance = new GameEnvironment();
                    instance.CheckPoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                    instance.checkPoints = instance.checkPoints.OrderBy(waypoint => waypoint.name).ToList();
                }
                return instance;
            }
        }
    }

}
