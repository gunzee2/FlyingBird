using UnityEngine;
using System.Collections;

// 見切れているブロックの削除用クラス.
public class BlockController : MonoBehaviour {

	public MapCreator map_creator = null;	// MapCreatorを保管するための変数.

	// Use this for initialization
	void Start () {
		// MapCreatorを取得して、メンバー変数map_creatorに保管.
		map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
	}
	
	// Update is called once per frame
	void Update () {

		// 見切れている時.
		if(this.map_creator.isDelete(this.gameObject)) {
			// 自分自身を削除.
			GameObject.Destroy(this.gameObject);
		}
	
	}
}
