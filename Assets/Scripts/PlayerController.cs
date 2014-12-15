using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// Public メソッド.
	public int up_power = 10;	// 上昇力.

	public bool isHitBlock = false;	// 壁にぶつかったかどうか判定するフラグ.

	private Vector3 prevPosition;

	// Use this for initialization
	void Start () {
		update_speed(1.0f);

		// 前回の位置を保存(移動方向を取得するため).
		prevPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// マウスの左クリック(または画面タップ)を押している間かつブロックに当たっていない場合.
		if((Input.touchCount > 0 || Input.GetMouseButton(0)) && !this.isHitBlock) {
			// 上昇する.
			this.rigidbody.AddForce((Vector2.up * up_power) * 100 * Time.deltaTime);
		}

		// 進行方向を取得し、傾きを設定する(進行方向を向くようにする).
		Vector3 diff = (this.transform.position - prevPosition).normalized;
		if(diff.magnitude > 0.01) {
			//Debug.Log("diff:" + diff);
			this.transform.rotation = Quaternion.FromToRotation(Vector3.right, diff);
		}

		// 前回の位置を保存(移動方向を取得するため).
		prevPosition = this.transform.position;

		
	}

	void OnCollisionEnter(Collision other) {
		if(other.gameObject.CompareTag("Wall") && !this.isHitBlock) {
			// 壁にぶつかったフラグをONにする.
			isHitBlock = true;
		}
	}

	public void GameOver() {
			// 爆発エフェクトを再生.
			this.gameObject.GetComponent<Detonator>().Explode();
			// オブジェクトを非表示にする.
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			// オブジェクトを固定(非表示なだけで存在はするため).
			this.rigidbody.constraints = RigidbodyConstraints.FreezeAll;

	}

	public void update_speed(float speed) {
		Vector3 velocity = this.rigidbody.velocity;
		this.rigidbody.velocity = new Vector3(speed, velocity.y, velocity.z);
	}
}
