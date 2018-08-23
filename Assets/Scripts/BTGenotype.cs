using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BTGenotype {

    int n;
    BTNode root;
    double fitness;
    int maxDepthAllowed;

    public BTGenotype(int _n, string[] rep, int _maxDepthAllowed) {
        n = _n;
        fitness = double.Parse(rep[0],System.Globalization.NumberStyles.AllowDecimalPoint,System.Globalization.NumberFormatInfo.InvariantInfo);
        string[] enconded = new string[rep.Length - 1];
        for (int i = 1; i < rep.Length; i++) {
            enconded[i - 1] = rep[i];
        }
        maxDepthAllowed = _maxDepthAllowed;
        constructFromArray(enconded);
        //Debug.Log(this.ToString());
    }

    public BTGenotype(BTNode _root) {
        root = _root;
        n = _root.n;
        fitness = 0.0f;
    }

    public void setRoot(BTNode _root){
        root = _root;
    }

    // Method that returns a the BTNode wiht the specified index
    // Uses a BFS to check all the nodes in the tree
    // Returns null if the node does not exist
    public BTNode getNodeWithIndex(int index) {
        Queue<BTNode> q = new Queue<BTNode>();
        q.Enqueue(root);
        while (q.Count > 0) {
            BTNode current = q.Dequeue();
            if (current.index == index) {
                return current;
            }
            List<BTNode> currentChildren = current.children;
            for (int i = 0; i < currentChildren.Count; i++) {
                q.Enqueue(currentChildren[i]);
            }
        }
        return null;
    }

    // Method that builds the genotype using an array in the format
    // i_index i_task
    public void constructFromArray(string[] rep) {
        /*Debug.Log("****Construyendo BT****");
        string s = "";
        for (int i = 0; i < rep.Length; i++){
            s += rep[i]+"|";
        }
        Debug.Log(s);*/
        string nodeStr = rep[0];
        string[] nodeArr = Regex.Split(nodeStr," ");
        int index = int.Parse(nodeArr[0]);
        string taskName = nodeArr[1];
        root = new BTNode(n, 0, taskName);
        //Debug.Log("RootIndex: " + root.index + " RootTaskName: " + root.taskName);
        for (int i = 1; i < rep.Length; i++) {
            nodeStr = rep[i];
            nodeArr = Regex.Split(nodeStr, " ");
            index = int.Parse(nodeArr[0]);
            taskName = nodeArr[1];
            BTNode currentNode = new BTNode(n, index, taskName);
            int parentIndex = (int)Mathf.Floor((index - 1) / (float)n);
            //Debug.Log("Index: " + index + " TaskName: " + taskName+" ParentIndex: "+parentIndex);
            BTNode parent = getNodeWithIndex(parentIndex);
            if (parent != null) {
                parent.addChild(currentNode);
            }
        }
    }

    public void normaliseFitness(double mean, double stdDev) {
        fitness = (fitness - mean) / stdDev;
    }

    private void restructureIndexes(BTNode currentNode, int index) {
        currentNode.index = index;
        List<BTNode> children = currentNode.children;
        for (int i = 1; i <= children.Count; i++) {
            int childIndex = index * n + i;
            restructureIndexes(children[i-1], childIndex);
        }
    }

    public void restructureBTIndexes() {
        restructureIndexes(root, 0);
    }

    public double getFitness() {
        return fitness;
    }

    public int CompareTo(BTGenotype other) {
        if (other == null) return 1;
        if (fitness > other.getFitness()) return 1;
        if (fitness < other.getFitness()) return -1;
        return 0;
    }

    public override string ToString() {
        string ans = "";
        if (fitness < 0) fitness *= -1;
        if (double.IsNaN(fitness)) fitness = 0;
        ans += fitness.ToString("N4", System.Globalization.NumberFormatInfo.InvariantInfo);
        Queue<BTNode> q = new Queue<BTNode>();
        q.Enqueue(root);
        while (q.Count > 0) {
            BTNode currentNode = q.Dequeue();
            string temp = "_" + currentNode.index + " " + currentNode.taskName;
            ans += temp;
            List<BTNode> children = currentNode.children;
            for (int i = 0; i < children.Count; i++) {
                q.Enqueue(children[i]);
            }
        }
        return ans;
    }

    private BTNode getCopyAux(BTNode currentNode) {
        BTNode subTree = new BTNode(n, currentNode.index, currentNode.taskName);
        for (int i = 0; i < currentNode.children.Count; i++) {
            BTNode child = currentNode.children[i];
            BTNode newChild = getCopyAux(child);
            subTree.addChild(newChild);
        }
        return subTree;
    }

    public BTGenotype getCopy() {
        BTNode newRoot = getCopyAux(root);
        BTGenotype ans = new BTGenotype(newRoot);
        ans.maxDepthAllowed = maxDepthAllowed;
        return ans;
    }

    private List<int> getIndexesList() {
        List<int> ans = new List<int>();
        Queue<BTNode> q = new Queue<BTNode>();
        q.Enqueue(root);
        while (q.Count > 0) {
            BTNode currentNode = q.Dequeue();
            ans.Add(currentNode.index);
            List<BTNode> children = currentNode.children;
            for (int i = 0; i < children.Count; i++) {
                q.Enqueue(children[i]);
            }
        }
        return ans;
    }

    public BTNode getRandomSubTree(){
        List<int> indexes = getIndexesList();
        int randIndex = indexes[Random.Range(0, indexes.Count)];
        return getNodeWithIndex(randIndex);
    }

    public void incrementFitness(double increment) {
        fitness += increment;
    }

    public void divideFitness(int numberOfTrees){
        fitness /= numberOfTrees;
    }

    private void pruneAux(BTNode subTree, int currentHeight, string[] leafTasks){
        if (subTree.isLeafNode()) return;
        if (currentHeight==maxDepthAllowed){
            int r = Random.Range(0, leafTasks.Length);
            subTree.turnToLeafNode(leafTasks[r]);
            return;
        }
        List<BTNode> children = subTree.children;
        for(int i = 0; i < children.Count; i++){
            BTNode child = children[i];
            if(child!=null)
                pruneAux(child, currentHeight + 1, leafTasks);
        }
    }

    public void pruneGenotype(string[] conditionals, string[] actions){
        string[] leafTasks = new string[conditionals.Length+actions.Length];
        int index = 0;
        for(int i = 0; i < conditionals.Length; i++){
            leafTasks[index++] = conditionals[i];
        }
        for(int i = 0; i < actions.Length; i++){
            leafTasks[index++] = actions[i];
        }
        for (int k = 0; k < Random.Range(4, 10); k++){
            for (int i = leafTasks.Length - 1; i > 0; i--){
                int r = Random.Range(0, i + 1);
                string temp = leafTasks[i];
                leafTasks[i] = leafTasks[r];
                leafTasks[r] = temp;
            }
        }
        pruneAux(root, 1, leafTasks);
    }

    public BTNode getRoot(){
        return root;
    }

    public void replaceSubTreeWithRoot(int subTreeIndex, BTNode newSubtreeRoot){
        int parentIndex = (int)Mathf.Floor((subTreeIndex - 1) / (float)n);
        BTNode parent = getNodeWithIndex(parentIndex);
        if(parent!=null){
            List<BTNode> children = parent.children;
            for (int i = 0; i < children.Count; i++){
                if (children[i].index == subTreeIndex){
                    children[i] = newSubtreeRoot;
                }
            }
        }else{
                root = newSubtreeRoot;
        }
    }

    public int numberOfNodes(){
        int ans = 0;
        Queue<BTNode> q = new Queue<BTNode>();
        q.Enqueue(root);
        while (q.Count > 0){
            BTNode current = q.Dequeue();
            ans++;
            List<BTNode> currentChildren = current.children;
            for (int i = 0; i < currentChildren.Count; i++){
                q.Enqueue(currentChildren[i]);
            }
        }
        return ans;
    }

}
