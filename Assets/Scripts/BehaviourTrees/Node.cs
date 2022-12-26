using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.BehaviourTrees
{
    //행동의 기본이 되는 노드 : 모든 노드는 자식을 가짐 : 행동 등 null을 가지면 leafNode가 됨
    public class Node
    {
        public enum Status
        {
            SUCCESS, RUNNING, FAILURE
        };
        public Status status;

        public List<Node> children = new List<Node>();
        public int currentChild = 0;
        public string name;
        public Node()
        {

        }
        public Node(string name)
        {
            this.name = name;
        }

        public virtual Status Process()
        {
            return children[currentChild].Process();
        }

        //자식 노드 추가
        public void AddChild(Node child)
        {
            children.Add(child);
        }
    }

}
