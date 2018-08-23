using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class HittingWall : Conditional{

    public override TaskStatus OnUpdate(){
        if(this.gameObject.GetComponent<WormBTController>().hittingWall){
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

}
