using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class AtEdge : Conditional{

    public override TaskStatus OnUpdate(){
        if (!this.gameObject.GetComponent<WormBTController>().notAtEdge){
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

}
