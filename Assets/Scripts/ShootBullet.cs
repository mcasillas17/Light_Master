using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class ShootBullet : Action {
	
    public override TaskStatus OnUpdate () {
        if (this.gameObject.GetComponent<WormBTController>().canShoot){
            this.gameObject.GetComponent<WormBTController>().shootBullet();

            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
	}

}
