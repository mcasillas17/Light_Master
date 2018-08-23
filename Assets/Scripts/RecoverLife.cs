using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverLife : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag=="Player"){
            other.gameObject.GetComponent<LightMasterController>().incrementLife(7);
            Destroy(gameObject);
        }
    }
}
