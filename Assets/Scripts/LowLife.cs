using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class LowLife : Conditional {
	
    public override TaskStatus OnUpdate () {
        int maxLife = this.gameObject.GetComponent<WormBTController>().maxLife;
        int currentLife = this.gameObject.GetComponent<WormBTController>().currentLife;
        if (((double)currentLife) / maxLife <= 0.5) return TaskStatus.Success;
        return TaskStatus.Failure;
	}

}
