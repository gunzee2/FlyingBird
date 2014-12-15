using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ScoreCounter : MonoBehaviour {

	public int score;					// 最後のスコア.
	public int highScore;				// ハイスコア.

	public Text score_text;				// スコア表示用Textオブジェクト.
	public Text highscore_text;			// ハイスコア表示用Textオブジェクト.

	private string highScoreKey = "highScore";	// PlayerPrefsでハイスコアを保存するためのキー.


	// Use this for initialization
	void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		// スコア表示をアップデート.
		this.print_value(this.score_text, "SCORE:", this.score);
		this.print_value(this.highscore_text, "HIGH-SCORE:", this.highScore);
	}

	// 初期化処理.
	private void Initialize () {
		
		this.score = 0;
		// ハイスコア読み込み(保存されていなければ0を読む).
		this.highScore = PlayerPrefs.GetInt(this.highScoreKey, 0);
		Debug.Log("highScore Load:" + this.highScore);
	}

	public void print_value(Text txtObj, string label, int value) {

		txtObj.text = label + value.ToString();
	}

	public void update_score(int addPoint) {
		this.score = this.score + addPoint;
		// 現在スコアがハイスコアを上回ったら、現在スコアをハイスコアにする.
		if(this.score > this.highScore) {
			this.highScore = this.score;
		}
	}

	public void Save() {
		Debug.Log("highScore Save:" + this.highScore);
		PlayerPrefs.SetInt(this.highScoreKey, this.highScore);
		PlayerPrefs.Save();
	}


}
