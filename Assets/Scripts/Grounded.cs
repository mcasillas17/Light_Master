using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Grounded : Conditional{

    public override TaskStatus OnUpdate(){
        if(this.gameObject.GetComponent<WormBTController>().grounded){
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

}
