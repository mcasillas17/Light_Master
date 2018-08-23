using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class TreeEvolverController : MonoBehaviour {

    public int maxChildren;
    public int maxDepth;
    public string populationName;
    public int populationSize;

    public int sizeOfGenotype;

    public string[] actions;
    public string[] conditionals;
    public string[] composites;

    public TextAsset generationNumber;
    public TextAsset txtPopulation;
    public TextAsset txtFitness;

    public int currentGenNumber;
    public List<GenotypeDescriptor> currentGeneration;


    int pow(int a, int b){
        if (a == 0) return 0;
        if (b == 0) return 1;
        if (a == 1) return 1;
        if (b == 1) return a;
        int mid = pow(a, b / 2);
        mid *= mid;
        if(b%2!=0){
            mid *= a;
        }
        return mid;
    }

    int getTreeHeightAux(string[] tree, int i){
        if (i >= sizeOfGenotype) return 0;
        if (tree[i].Equals("_")) return 0;
        int maxi = 0;
        for (int j = 0; j < maxChildren; j++){
            int childPosition = maxChildren * i + j;
            maxi = Mathf.Max(maxi, getTreeHeightAux(tree, childPosition));
        }
        return maxi+1;
    }

    int getTreeHeight(string[] tree){
        return getTreeHeightAux(tree, 0);
    }

    string getRandomTerminal(){
        int rand = Random.Range(0, 100);
        if(rand<30){
            int indexCond = Random.Range(0, conditionals.Length);
            return conditionals[indexCond];
        }
        int indexAct = Random.Range(0, actions.Length);
        return actions[indexAct];
    }

    string getRandomComposite(){
        int index = Random.Range(0, composites.Length);
        return composites[index];
    }

    string[] getEmptyTreeGenotype(){
        string[] res = new string[sizeOfGenotype];
        for (int i = 0; i < sizeOfGenotype; i++){
            res[i] = "_";
        }
        return res;
    }

    void getInitialGenotypeByFullMethodAux(string[] res, int position, int currentDepth, int depth){
        if (position >= sizeOfGenotype) return;
        if(currentDepth==depth){
            res[position] = getRandomTerminal();
        }else{
            res[position] = getRandomComposite();
            int numberOfChildren = Random.Range(2, maxChildren+1);
            for (int i = 1; i <= numberOfChildren; i++){
                int childPosition = maxChildren * position + i;
                getInitialGenotypeByFullMethodAux(res,childPosition,currentDepth+1,depth);
            }
        }
    }

    string[] getInitialGenotypeByFullMethod(int depth){
        string[] res = new string[sizeOfGenotype];
        for (int i = 0; i < sizeOfGenotype;i++){
            res[i] = "_";
        }
        getInitialGenotypeByFullMethodAux(res,0,1,depth);
        return res;
    }

    void getInitialGenotypeByGrowMethodAux(string[] res, int position, int currentDepth, int maxD){
        if (position >= sizeOfGenotype) return;
        if(currentDepth==maxD){
            res[position] = getRandomTerminal();
        }else{
            int rand = Random.Range(0, 100);
            // Terminal
            if(rand<20){
                res[position] = getRandomTerminal();
            }else{ // Composite
                res[position] = getRandomComposite();
                int numberOfChildren = Random.Range(2, maxChildren+1);
                for (int i = 1; i <= numberOfChildren; i++){
                    int childPosition = maxChildren * position + i;
                    getInitialGenotypeByFullMethodAux(res, childPosition, currentDepth + 1, maxD);
                }
            }
        }
    }

    string[] getInitialGenotypeByGrowMethod(int maxD){
        string[] res = new string[sizeOfGenotype];
        for (int i = 0; i < sizeOfGenotype; i++){
            res[i] = "_";
        }
        getInitialGenotypeByGrowMethodAux(res, 0, 1, maxD);
        return res;
    }

    string genotypeToStr(string[] genotype){
        string str = "";
        for (int i = 0; i < genotype.Length - 1 ;i++){
            str += genotype[i] + "-";
        }
        str += genotype[genotype.Length - 1];
        return str;
    }

    // Ramped-half-and-half method that creates the random initial population
    // half the population is created with the full method, starting with
    // a depth of 2 and incrementing
    // the other half is created with the grow method, starting with a max depth
    // of 2 and incrementing
    void setInitialPopulation(){
        for (int i = 0; i < populationSize/2;i++){
            string[] currentGeno = getInitialGenotypeByFullMethod(maxDepth);
            currentGeneration.Add(new GenotypeDescriptor(0.0f, currentGeno));
        }
        for (int i = 0; i < populationSize / 2;i++){
            string[] currentGeno = getInitialGenotypeByGrowMethod(maxDepth);
            currentGeneration.Add(new GenotypeDescriptor(0.0f, currentGeno));
        }
        Debug.Log("Random initial population! :D");
        printCurrentGeneration();
    }

    void printCurrentGeneration(){
        for (int i = 0; i < currentGeneration.Count; i++){
            Debug.Log(genotypeToStr(currentGeneration[i].tree) + " " + currentGeneration[i].fitness);
        }
    }

    // Method that reads the info stored in the TextAsset objects
    // and creates new txt files to store the info
    // that way the information about the previous generation isn't lost
    void storeLastGeneration(){
        int lastGeneration = int.Parse(generationNumber.text);
        string populationStr = txtPopulation.text;
        string fitnessStr = txtFitness.text;

        string path = "Assets/Resources/EvolvedGenerations/" + populationName + lastGeneration + ".txt";
        StreamWriter writer = new StreamWriter(path);
        writer.Write(populationStr);
        writer.Close();

        path = "Assets/Resources/EvolvedGenerations/" + populationName + lastGeneration + "Fitness.txt";
        writer = new StreamWriter(path);
        writer.Write(fitnessStr);
        writer.Close();
    }

    // Method that reads the population and fitness stored in the TextAsset objects
    // and stores the info in the currentGeneration list
    void getLastGeneration(){
        string populationStr = txtPopulation.text;
        string[] trees = Regex.Split(populationStr, "\n");
        string fitnessStr = txtFitness.text;
        string[] fitnessScores = Regex.Split(fitnessStr, "\n");
        for (int i = 0; i < trees.Length; i++){
            string[] currentTree = Regex.Split(trees[i], "-");
            float currentFitness = float.Parse(fitnessScores[i], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            GenotypeDescriptor gd = new GenotypeDescriptor(currentFitness, currentTree);
            currentGeneration.Add(gd);
        }
    }

    // Method that fills a treeToFill from a subtree starting at index i, using the 
    // subtree at position j in the refTree
    void fillTreeFromIndex(string[] treeToFill, string[] refTree, int i, int j){
        if (i >= sizeOfGenotype || j >= sizeOfGenotype) return;
        treeToFill[i] = refTree[j];
        for (int k = 1; k <= maxChildren; k++){
            int childPosition1 = maxChildren * i + k;
            int childPosition2 = maxChildren * j + k;
            fillTreeFromIndex(treeToFill, refTree, childPosition1, childPosition2);
        }
    }

    private int[] getRandomIndexesAtLevel(int level){
        int[] res = new int[pow(maxChildren, level-1)];
        res[0] = pow(maxChildren, level - 1);
        for (int i = 1; i < res.Length; i++){
            res[i] = res[i - 1] + 1;
        }
        for (int k = 0; k < 10; k++){
            for (int i = 0; i < res.Length - 1; i++){
                int temp = res[i];
                int randInd = Random.Range(i, res.Length);
                res[i] = res[randInd];
                res[randInd] = temp;
            }
        }
        return res;
    }

    // Method that returns the corssover result from the
    // genotypes at position i and position j
    List<GenotypeDescriptor> getCrossover(int i, int j){
        List<GenotypeDescriptor> ans = new List<GenotypeDescriptor>();
        string[] elem1 = getEmptyTreeGenotype();
        string[] elem2 = getEmptyTreeGenotype();
        string[] parent1 = currentGeneration[i].tree;
        string[] parent2 = currentGeneration[i].tree;

        bool canCross = false;
        int h1 = getTreeHeight(parent1);
        int h2 = getTreeHeight(parent2);
        int startIndex1 = -1;
        int startIndex2 = -1;

        while(!canCross){
            int mini = Mathf.Min(h1, h2);
            int targetHeight = Random.Range(1, mini+1);
            int[] ind1 = getRandomIndexesAtLevel(targetHeight);
            int[] ind2 = getRandomIndexesAtLevel(targetHeight);
            for (int k = 0; k< ind1.Length; k++){
                if(!parent1[ind1[k]].Equals("_")){
                    startIndex1 = ind1[k];
                    break;
                }
            }
            for (int k = 0; k < ind2.Length; k++){
                if (!parent2[ind2[k]].Equals("_")){
                    startIndex2 = ind2[k];
                    break;
                }
            }
            canCross = startIndex1 != -1 && startIndex2 != -1;
        }

        for (int k = 0; k < parent1.Length; k++){
            elem1[i] = parent1[i];
            elem2[i] = parent2[i];
        }

        fillTreeFromIndex(elem1,parent2,startIndex1,startIndex2);
        fillTreeFromIndex(elem2,parent1,startIndex1,startIndex2);

        ans.Add(new GenotypeDescriptor(0.0f, elem1));
        ans.Add(new GenotypeDescriptor(0.0f, elem2));
        return ans;
    }

    void evolveLastGeneration(){
        Debug.Log("Evolving generation! :D");
    }

    public void storeCurrentGeneration(){
        string path = "Assets/Resources/" + populationName + ".txt";
        string content = "";
        StreamWriter writer = new StreamWriter(path);
        for (int i = 0; i < currentGeneration.Count; i++){
            content += genotypeToStr(currentGeneration[i].tree) + "\n";
        }
        writer.Write(content);
        writer.Close();

        content = "";
        path = "Assets/Resources/" + populationName + "Fitness.txt";
        writer = new StreamWriter(path);
        for (int i = 0; i < currentGeneration.Count; i++){
            string fit = currentGeneration[i].fitness.ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            content += fit + "\n";
        }
        writer.Write(content);
        writer.Close();

        int currGen = int.Parse(generationNumber.text) + 1;
        path = "Assets/Resources/" + populationName + "GenerationNumber.txt";
        writer = new StreamWriter(path);
        writer.Write(""+currGen);
        writer.Close();
        Debug.Log("Storing current generation to txt files");
    }

    public void setPopulationReady(){
        int lastGeneration = int.Parse(generationNumber.text);
        //Debug.Log(lastGeneration);
        if(lastGeneration==-1){
            setInitialPopulation();
        }else{ //evolve last Generation
            storeLastGeneration();
            getLastGeneration();
            evolveLastGeneration();
        }
    }

    void setInitialValues(){
        sizeOfGenotype = 0;
        for (int i = 0; i < maxDepth; i++){
            sizeOfGenotype += pow(maxChildren, i);
        }
        currentGeneration = new List<GenotypeDescriptor>();
    }

    // 1.- Read the file with the previous population
    //     a) if there is no previous population, create the initial one
    // 2.- Evolve the previous population and store the previous one
	void Start () {
        setInitialValues();
        setPopulationReady();
        storeCurrentGeneration();
	}
	
}