using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScript : MonoBehaviour {

	private float interval_time = 0.0f;	// 1秒ごとに処理を実行する用タイマー.

	private string highScoreKey = "highScore";	// ハイスコア取得用のキー(ScoreCounterで同様のものを使用しているので名前変更注意).
	private int highScore = 0;

	public Text flashing_text;	// 点滅表示させるテキストオブジェクト.
	public Text highscore_text;	// ハイスコアを表示させるテキストオブジェクト.
	// Use this for initialization
	void Start () {
		GetHighScore();
		UpdateHighScore();
	}

	
	// Update is called once per frame
	void Update () {

		interval_time += Time.deltaTime;

		if(interval_time > 0.5f) {
			if(flashing_text.enabled) {
				this.flashing_text.enabled = false;
			} else {
				this.flashing_text.enabled = true;
			}
			interval_time = 0;
		}
		
		if(Input.GetMouseButtonDown(0)) {
			Application.LoadLevel("GameScene");
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

	void GetHighScore() {
		this.highScore = PlayerPrefs.GetInt(this.highScoreKey, 0);	
	}

	void UpdateHighScore() {
		this.highscore_text.text = "HIGH-SCORE:" + this.highScore.ToString();
	}
}
