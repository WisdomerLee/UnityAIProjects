using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.BehaviourTrees
{
    public class BehaviourTree : Node
    {
        public BehaviourTree()
        {
            name = "Tree";
        }

        public BehaviourTree(string name)
        {
            this.name = name;
        }

        struct NodeLevel
        {
            public int level;
            public Node node;
        }

        public override Status Process()
        {
            return children[currentChild].Process();
        }

        //해당 노드의 트리구조 출력
        public void PrintTree()
        {
            string treePrintout = "";
            Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
            Node currentNode = this;
            nodeStack.Push(new NodeLevel { level = 0, node = currentNode});

            while(nodeStack.Count != 0)
            {
                NodeLevel nextNode = nodeStack.Pop();
                treePrintout += new string('-', nextNode.level) + nextNode.node.name + "\n";
                for(int i = nextNode.node.children.Count -1 ; i>=0; i--)
                {
                    nodeStack.Push(new NodeLevel { level = nextNode.level + 1, node = nextNode.node.children[i] });
                }
            }
            Debug.Log(treePrintout);
        }
    }

}
