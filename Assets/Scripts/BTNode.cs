using System.Collections.Generic;

public class BTNode {
    
    public int n;
    public int index;
    public string taskName;
    public List<BTNode> children;

    public BTNode(int _n, int _index, string _taskName){
        n = _n;
        index = _index;
        taskName = _taskName;
        children = new List<BTNode>();
    }

    public bool addChild(BTNode child){
        if (children.Count < n){
            children.Add(child);
            return true;
        }
        return false;
    }

    public bool isLeafNode(){
        return children.Count == 0;
    }

    public bool isInternalNode(){
        return children.Count > 0;
    }

    public void setTaskName(string newTaskName){
        taskName = newTaskName;
    }

    public void turnToLeafNode(string newTaskName){
        taskName = newTaskName;
        children = new List<BTNode>();
    }

    public override string ToString(){
        return "" + index + " " + taskName;
    }
}
