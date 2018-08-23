using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeCheckController : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other){
        WormBTController controller = this.gameObject.transform.parent.gameObject.GetComponent<WormBTController>();
        if(other.gameObject.tag=="Checkpoint"){
            controller.checkpointsInRangeCount++;
        }else if(other.gameObject.tag=="Coin"){
            controller.coinsInRangeCount++;
        }
        else if(other.gameObject.tag=="Player"){
            controller.playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        WormBTController controller = this.gameObject.transform.parent.gameObject.GetComponent<WormBTController>();
        if(other.gameObject.tag=="Checkpoint"){
            controller.checkpointsInRangeCount--;
        }
        else if(other.gameObject.tag=="Coin"){
            controller.coinsInRangeCount--;
        }
        else if(other.gameObject.tag=="Player"){
            controller.playerInRange = false;
        }
    }


}
