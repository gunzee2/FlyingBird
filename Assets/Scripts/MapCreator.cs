using UnityEngine;
using System.Collections;

public class MapCreator : MonoBehaviour {

	// 定数.
	public static float FIRST_BLOCK_POS_Y = -3;	// ブロックの初期高さ.
	public static float BLOCK_WIDTH = 1.0f;		// ブロックの幅.
	public static float BLOCK_HEIGHT = 1.0f;	// ブロックの高さ.
	public static float MAX_DISTANCE = 7.0f;	// 上下ブロック間の最大距離.
	public static int BLOCK_NUM_IN_SCREEN = 26;	// 画面内に収まるブロックの数(横).
	public static float SCREEN_HEIGHT = 8.0f;	// 画面の高さ.
	public static int UPPER_LIMIT_Y = -2;			// ブロックを配置出来る位置(Y)の上限.
	public static int LOWER_LIMIT_Y = -4;			// ブロックを配置出来る位置(Y)の下限.

	// ブロックに関する情報をまとめて管理するための構造体.
	public struct FloorBlock {
		public bool is_created;						// ブロックが作成済みか否か.
		public Vector3 position;					// ブロックの位置(下).
		public Vector3 position_top;				// ブロックの位置(上).
	}

	// パブリックプロパティ.
	public GameObject blockPrefab;				// ブロックのプレハブを保管.
	public TextAsset level_data_text = null;	// 難易度設定のテキストデータ.
	public float distance_top;					// 上ブロックとの距離.

	// プライベートプロパティ.
	private FloorBlock last_block;						// 最後に作成したブロックの情報(下).
	private GameObject player = null;					// シーン上のPlayerを保管.
	private LevelController level_controller = null;	// 難易度操作用クラスを保管.
	private GameRoot game_root = null;					// GameRootオブジェクトを保持.


	void Start () {
		// プレイヤーオブジェクトを取得.
		this.player = GameObject.FindGameObjectWithTag("Player");
		// 直近のブロックは未作成.
		this.last_block.is_created = false;

		// 難易度操作用クラスを作成し、テキストデータから難易度設定を読み込む.
		this.level_controller = new LevelController();
		this.level_controller.loadLevelData(this.level_data_text);

		// GameRootオブジェクトを取得(経過時間計測用).
		this.game_root = this.gameObject.GetComponent<GameRoot>();
	}

	void Update () {

		// GameRootから経過時間を取得し、難易度情報を更新.
		this.level_controller.update_level(this.game_root.getPlayTime());
		this.game_root.update_player_speed(this.level_controller.current_level.speed);

		// プレイヤーのX位置を取得.
		float block_generate_x = this.player.transform.position.x;
		// そこから、およそ半画面分、右へ移動.
		// この位置が、ブロックを生み出すしきい値になる.
		block_generate_x += BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN + 1) / 2.0f;

		// 最後に作ったブロックの位置がしきい値より小さい間.
		while(this.last_block.position.x < block_generate_x) {
			// ブロックを作る.
			create_floor_block();
		}
	
	}

	// ブロックを作る場所を決定し、ブロックを作成する命令を出す.
	private void create_floor_block() {

		Vector3 block_position;

		// 前のブロックが未作成の時.
		if(!this.last_block.is_created) {
			// ブロックの位置を、とりあえずPlayerと同じにする.
			block_position = this.player.transform.position;
			// それから、ブロックのX位置を半画面分、左に移動.
			block_position.x -= BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);
			// ブロックのY位置は初期高さに.
			block_position.y = FIRST_BLOCK_POS_Y;
		} else { // last_blockが作成済みの場合.
			// 今回作るブロックの位置を、前回作ったブロックと同じに.
			block_position = this.last_block.position;
		}

		// ブロックを1ブロック分、右に移動.
		block_position.x += BLOCK_WIDTH;

		// 現在の難易度データをLevelControllerから取得.
		LevelData level = this.level_controller.current_level;

		// 上ブロックとの距離を調整する.
		distance_top = change_distance(distance_top, level.distance.min, level.distance.max, level.min_distance);

		// ブロックのY座標を決める.
		// 画面の高さの半分(中心点が0のため)から上下ブロックの間隔を引いた数を下ブロックを置ける座標の上限とする.
		float block_upper_limit = (SCREEN_HEIGHT / 2) - distance_top;
		block_position.y = move_block_position_y(block_position.y, level.movement.min, level.movement.max, LOWER_LIMIT_Y, block_upper_limit);

		// 規定距離離れた上側にもブロックを生成する.
		Vector3 block_position_top = new Vector3(block_position.x, block_position.y + distance_top, block_position.z);

		// ブロックを実際に生成する(下側、上側).
		GameObject bot_block = create_block(block_position);
		GameObject top_block = create_block(block_position_top);

		// 線を引く(前のブロックが存在しない場合、読み飛ばす).
		if(this.last_block.position != Vector3.zero) {
			DrawLine(bot_block, block_position, this.last_block.position, Color.white);
			DrawLine(top_block, block_position_top, this.last_block.position_top, Color.white);
		}

		// last_blockの位置を、今回の位置に更新.
		this.last_block.position = block_position;
		this.last_block.position_top = block_position_top;
		// ブロック作成済みなので、last_blockのis_createdをtrueに.
		this.last_block.is_created = true;

	}

	
	private void DrawLine(GameObject block, Vector3 pos1, Vector3 pos2, Color color) {

		// ブロックオブジェクトからLineRendererを取り出し、pos1からpos2へ線を引く.
		LineRenderer line = block.gameObject.GetComponent<LineRenderer>();	
		line.SetVertexCount(2);
		line.SetPosition(0, pos1);
		line.SetPosition(1, pos2);

		// 引いた線に合わせて当たり判定を設定
		block.transform.position = pos1 + (pos2 - pos1) / 2;
		block.transform.LookAt(pos1);
		//CapsuleCollider capsule = block.gameObject.GetComponent<CapsuleCollider>();	
		//capsule.transform.position = pos1 + (pos2 - pos1) / 2;
		//capsule.transform.LookAt(pos1);
		//capsule.height = (pos2 - pos1).magnitude;
	
	}

	// ランダムに上下位置を動かす(事前に決めた上限、下限に制限).
	private float move_block_position_y(float pos, float range_low, float range_up, float lower_limit, float upper_limit) {

		return Mathf.Clamp(pos + Random.Range(range_low, range_up), lower_limit, upper_limit);
	}

	// ランダムに上下ブロックの感覚を動かす(事前に決めた上限、下限に制限).
	private float change_distance(float distance, float range_sub, float range_add, float min_distance) {
		
		return Mathf.Clamp(distance + Random.Range(range_sub, range_add), min_distance, MAX_DISTANCE);
	}

	// ブロックを実際に生成する.
	private GameObject create_block(Vector3 block_pos) {
		// ブロックを生成し、goに保管.
		GameObject go = GameObject.Instantiate(this.blockPrefab) as GameObject;

		go.transform.position = block_pos;	// ブロックの位置を移動.
		//Debug.Log(aim);
		
		return go;
	}

	// 画面からブロックが見切れているか判定するパブリックメソッド.
	// BlockControllerクラスから使用.
	public bool isDelete(GameObject block_object) {
		bool ret = false;	// 戻り値.

		// Playerから、半画面分左の位置。これが、消えるべきか否かを決める閾値となる.
		float left_limit = this.player.transform.position.x - BLOCK_WIDTH * ((float)BLOCK_NUM_IN_SCREEN / 2.0f);

		// ブロックの位置がしきい値より小さい(左)なら.
		if(block_object.transform.position.x < left_limit) {
			ret = true;		// 戻り値をtrueに.
		}

		return ret;
	}
}
