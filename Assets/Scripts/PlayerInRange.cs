using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class PlayerInRange : Conditional{

    public override TaskStatus OnUpdate(){
        if (!this.gameObject.GetComponent<WormBTController>().playerInRange){
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

}