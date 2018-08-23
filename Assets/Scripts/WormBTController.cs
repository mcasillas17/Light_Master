using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class WormBTController : MonoBehaviour {

    double timeOnLevel;
    int treeIndex;
    public int currentLife;
    public int maxLife;
    private bool active;
    int totalHits;
    int timesHit;
    int totalBulletsConnected;

    public int coinsInRangeCount;
    double timeNearbyCoins;
    public int checkpointsInRangeCount;
    double timeNerbyCheckPoints;

    public GameObject wormDamageParticles;
    public GameObject wormDeathParticles;
    public GameObject bullet;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public bool grounded;

    public bool notAtEdge;
    public Transform edgeCheck;
    public Transform wallCheck;
    public bool hittingWall;

    public bool canShoot;
    public double timeHittingWall;
    public double timeGrounded;

    public bool playerInRange;

    void Start () {
        timeOnLevel = 0.0f;
        active = true;
        maxLife = currentLife = 90;
        totalBulletsConnected = 0;
        timesHit = 0;
        totalHits = 0;
        canShoot = true;
        timeHittingWall = 0.0f;
        timeGrounded = 0.0f;
	}
    
    void Update () {

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, groundCheckRadius, whatIsGround);
        notAtEdge = Physics2D.OverlapCircle(edgeCheck.position, groundCheckRadius, whatIsGround);

        if (active){
            timeOnLevel += Time.deltaTime;
            if(hittingWall){
                timeHittingWall += Time.deltaTime;
            }
            if(grounded){
                timeGrounded += Time.deltaTime;
            }
            if(checkpointsInRangeCount>0){
                timeNerbyCheckPoints += Time.deltaTime;
            }
            if(coinsInRangeCount>0){
                timeNearbyCoins += Time.deltaTime;
            }
        }
	}

    public void getDamageFromFallingTile(int x){
        currentLife -= x;
    }

    public int getTreeIndex(){
        return treeIndex;
    }

    public void GetAttackFromPlayer(int damage){
        currentLife -= damage;
        timesHit++;
        if (currentLife > 0){
            Instantiate(wormDamageParticles, transform.position, Quaternion.identity);
        }
        else{
            Instantiate(wormDeathParticles, transform.position, Quaternion.identity);
            disableWorm();
        }
    }

    public void disableWorm(){
        gameObject.SetActive(false);
        GetComponent<Renderer>().enabled = false;
        active = false;

    }

    public double getFinalFitness(){
        return (timeOnLevel * 0.2f) + (totalHits * 3.0f) - (timesHit * 3.2f) + (totalBulletsConnected * 3.5f) - (timeHittingWall * 0.1f)
            + (timeGrounded * 0.09);
    }

    private Task getBTfromGenotype(BTNode subTree){
        string taskName = subTree.taskName;
        if (subTree.isLeafNode()){
            // Actions
            if (taskName.Equals("Jump")){
                Jump temp = new Jump();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }
            else if (taskName.Equals("MoveLeftForSeconds")){
                MoveLeftForSeconds temp = new MoveLeftForSeconds();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }
            else if(taskName.Equals("MoveRightForSeconds")){
                MoveRightForSeconds temp = new MoveRightForSeconds();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }else if(taskName.Equals("MoveRightForDistance")){
                MoveRightForDistance temp = new MoveRightForDistance();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }else if (taskName.Equals("ShootBullet")){
                ShootBullet temp = new ShootBullet();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }else if(taskName.Equals("MoveLeftForDistance")){
                MoveLeftForDistance temp = new MoveLeftForDistance();
                #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }
            // TO DO add conditionals
            else if (taskName.Equals("AtEdge")){
                AtEdge temp = new AtEdge();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }else if (taskName.Equals("HittingWall")){
                HittingWall temp = new HittingWall();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }
            else if (taskName.Equals("LowLife")){
                LowLife temp = new LowLife();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }
            else if (taskName.Equals("PlayerInRange")){
                PlayerInRange temp = new PlayerInRange();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }
            else {
                Grounded temp = new Grounded();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                return temp;
            }
        }
        else{
            if (taskName.Equals("Selector")){
                Selector temp = new Selector();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                List<BTNode> children = subTree.children;
                for(int i = 0; i < children.Count; i++){
                    BTNode child = children[i];
                    temp.AddChild(getBTfromGenotype(child),i);
                }
                return temp;
            } else if (taskName.Equals("Sequence")){
                Sequence temp = new Sequence();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                List<BTNode> children = subTree.children;
                for (int i = 0; i < children.Count; i++){
                    BTNode child = children[i];
                    temp.AddChild(getBTfromGenotype(child), i);
                }
                return temp;
            } else if (taskName.Equals("RandomSelector")){
                RandomSelector temp = new RandomSelector();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                List<BTNode> children = subTree.children;
                for (int i = 0; i < children.Count; i++){
                    BTNode child = children[i];
                    temp.AddChild(getBTfromGenotype(child), i);
                }
               return temp;
            } else if (taskName.Equals("RandomSequence")){
                RandomSequence temp = new RandomSequence();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                List<BTNode> children = subTree.children;
                for (int i = 0; i < children.Count; i++){
                    BTNode child = children[i];
                    temp.AddChild(getBTfromGenotype(child), i);
                }
                return temp;
            } else{
                Parallel temp = new Parallel();
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
                temp.NodeData = new NodeData();
#endif
                List<BTNode> children = subTree.children;
                for (int i = 0; i < children.Count; i++){
                    BTNode child = children[i];
                    temp.AddChild(getBTfromGenotype(child), i);
                }
                return temp;
            }
        }
    }

    public void setBT(BTGenotype btGenotype, int _treeIndex){
        treeIndex = _treeIndex;
        BehaviorTree tree = gameObject.AddComponent<BehaviorTree>();
        if (tree != null){
            //Debug.Log("Creando árbol con index " + _treeIndex);
            tree.StartWhenEnabled = false;
            tree.GetBehaviorSource().HasSerialized = false;
            EntryTask entryTask = new EntryTask();
            entryTask.NodeData = new NodeData();
            tree.GetBehaviorSource().EntryTask = entryTask;
            //Debug.Log("Árbol con index " + _treeIndex + ": " + btGenotype.ToString());
            Task rootTask = getBTfromGenotype(btGenotype.getRoot());
            rootTask.NodeData = new NodeData();
            tree.GetBehaviorSource().RootTask = rootTask;
            tree.GetBehaviorSource().HasSerialized = true;
            tree.RestartWhenComplete = true;
            tree.EnableBehavior();
        }
        else{
            Debug.Log("No se pudo crear el árbol con index " + _treeIndex);
        }
    }

    public void incrementHits(){
        totalHits++;
    }

    public void incrementBulletsConnected(){
        totalBulletsConnected++;
    }

    public void enableBT(){
        BehaviorTree tree = GetComponent<BehaviorTree>();
        tree.EnableBehavior();
    }
    public void shootBullet(){
        StartCoroutine("shootCorutine");
    }
    public IEnumerator shootCorutine(){
        canShoot = false;
        yield return new WaitForSeconds(3.5f);

        GameObject temp = Instantiate(bullet, this.gameObject.transform.position, Quaternion.identity);
        WormBulletController bulletController = temp.GetComponent<WormBulletController>();
        bulletController.worm = this;
        if (gameObject.transform.localScale.x > 0){
            //facing left
            bulletController.shoot_speed_horizontal *= -1;
        }
        canShoot = true;
    }
}
