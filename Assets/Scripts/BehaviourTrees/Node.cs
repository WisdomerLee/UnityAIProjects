using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AITutorial.BehaviourTrees
{
    //�ൿ�� �⺻�� �Ǵ� ��� : ��� ���� �ڽ��� ���� : �ൿ �� null�� ������ leafNode�� ��
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

        //�ڽ� ��� �߰�
        public void AddChild(Node child)
        {
            children.Add(child);
        }
    }

}
