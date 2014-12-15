using UnityEngine;
using System.Collections;

public class GameRoot : MonoBehaviour {

	public float step_timer = 0.0f;	// 経過時間を保持.
	public float interval_time = 0.0f;	// 0.5秒ごとに処理を実行する用タイマー.

	private PlayerController player = null;
	private ScoreCounter score_counter = null;

	private bool isGameOver = false;	// ゲームオーバーフラグ.

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		this.score_counter = this.gameObject.GetComponent<ScoreCounter>();

		// ターゲットフレームレートを60fpsに.
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {
		this.step_timer += Time.deltaTime; // 刻々と経過時間を足していく.
		interval_time += Time.deltaTime;

		// interval_timerが0.5秒を上回った時、スコアカウント.
		if(this.interval_time >= 0.5f) {
			if(!player.isHitBlock) {
				this.score_counter.update_score(5);	// 0.5秒ごとに5点加算.
			}
			this.interval_time = 0;	// タイマーリセット.
		}

		// プレイヤーがブロックにぶつかった場合、ゲームオーバーとする.
		if(player.isHitBlock && !this.isGameOver) {
			// プレイヤーのゲームオーバー処理を実行.
			player.GameOver();
			// GameOverコルーチンの実行.
			StartCoroutine("GameOver");
			// コルーチン実行中に再度この処理が流れないよう、ゲームオーバーフラグをONにする.
			this.isGameOver = true;
		}


		// Androidで戻るボタンが押されたらゲームを終了する.
		if (Application.platform == RuntimePlatform.Android) {
			if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
			{
				Application.Quit();
				return;
			}
		}

	}
	
	private IEnumerator GameOver() {


		// ハイスコアのセーブ.
		this.score_counter.Save();

		// 3秒待つ
		yield return new WaitForSeconds (3.0f);

		// タイトル画面へ戻る.
		Application.LoadLevel("TitleScene");

	
	}

	// 経過時間を取得(外部スクリプト用).
	public float getPlayTime()
	{
		float time;
		time = this.step_timer;
		return time;
	}

	// プレイヤーに移動速度変更の指示をする.
	public void update_player_speed(float speed) {
		// プレイヤーがブロックに衝突していなければ.
		if(!player.isHitBlock) {
			// 移動速度を更新する.
			player.update_speed(speed);
		}
	}
}
