using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttackTrigger : MonoBehaviour {

	public int attackDamage;
	public float knifeRepelForce;

	void Start () {
		attackDamage = 20;
		knifeRepelForce = 500f;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("BreakableBlock")) {
            other.gameObject.GetComponent<BreakableBlockController>().GetAttackFromPlayer(attackDamage);
            //other.SendMessageUpwards ("GetAttackFromPlayer",attackDamage);
		}
		if (other.CompareTag ("Knife")) {
			Rigidbody2D rbKnife = other.GetComponent<Rigidbody2D> ();
			Vector2 direction = other.gameObject.transform.position - transform.position;
			other.gameObject.GetComponent<ArrowController> ().state = 1;
			rbKnife.isKinematic = false;
			rbKnife.freezeRotation = false;
			rbKnife.velocity = Vector2.zero;
			int yDirection = Random.Range (0, 10) < 5 ? 1 : -1;
			if (direction.x > 0) {
				if (yDirection == -1)
					other.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 45));
				else
					other.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 270));
				rbKnife.AddForce (new Vector2 (knifeRepelForce, yDirection*knifeRepelForce));
			} else {
				if (yDirection == -1)
					other.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 135));
				else
					other.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 225));
				rbKnife.AddForce (new Vector2 (-knifeRepelForce, yDirection*knifeRepelForce));
			}
		}
		if (other.CompareTag ("Enemy")) {
            other.gameObject.GetComponent<WormBTController>().GetAttackFromPlayer(attackDamage);
			//other.SendMessageUpwards ("GetAttackFromPlayer",attackDamage);
			Rigidbody2D rbEnemie = other.GetComponent<Rigidbody2D> ();
			Vector2 direction = other.gameObject.transform.position - transform.position;
			if (direction.x > 0) {
				rbEnemie.AddForce (new Vector2 (180, 180));
			} else {
				rbEnemie.AddForce (new Vector2 (-180, 180));
			}
		}
	}
}
