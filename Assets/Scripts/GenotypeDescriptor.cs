public class GenotypeDescriptor {

    public float fitness;
    public string[] tree;
    public int instantiatedTrees;

    public GenotypeDescriptor(float _fitness, string [] _tree){
        fitness = _fitness;
        tree = _tree;
    }

    public GenotypeDescriptor(string[] _tree){
        tree = new string[_tree.Length];
        for (int i = 0; i < tree.Length; i++){
            tree[i] = _tree[i];
        }
    }

	
}