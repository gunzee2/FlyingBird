using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private GameObject player = null;
	private Vector3 position_offset = Vector3.zero;

	// Use this for initialization
	void Start () {

		// Playerオブジェクトを探し、カメラの位置とプレイヤーの位置の差分を保存する.
		this.player = GameObject.FindGameObjectWithTag("Player");
		this.position_offset = this.transform.position - this.player.transform.position;
	
	}
	
	// Update is called once per frame
	// LateUpdate()は全てのGameObjectのUpdate処理が終わった後に実行される.
	void LateUpdate () {
		// カメラの現在位置をnew_positionに取得.
		Vector3 new_position = this.transform.position;

		// プレイヤーのX座標に差分を足して、変数new_positionのXに代入する.
		new_position.x = this.player.transform.position.x + this.position_offset.x;

		// カメラの位置を、新しい位置(new_position)に更新.
		this.transform.position = new_position;
	}
}
