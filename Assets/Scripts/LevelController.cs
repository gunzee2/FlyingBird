using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

	public LevelData current_level = null;

	private List<LevelData> level_datas = new List<LevelData>();
	private int last_levelNo = -1;		// 前の難易度。難易度が切り替わった瞬間を検知するために使用.

	public void loadLevelData(TextAsset level_data_text) {
		
		// テキストデータを、文字列として取り込む.
		string level_texts = level_data_text.text;

		// 改行コード'\n'ごとに分割し、文字列の配列に入れる.
		string[] lines = level_texts.Split('\n');

		// lines内の各行に対して、順番に処理していくループ.
		foreach(var line in lines) {
			if(line == "") continue;	// 空行なら読み飛ばす.
			Debug.Log(line);
			string[] words = line.Split(); // 行内のワードを配列に格納.

			// LevelData型の変数を作成.
			// ここに、現在処理している行のデータを入れていく.
			LevelData level_data = new LevelData();

			int n = 0;
			// words内の各ワードに対して、順番に処理していくループ.
			foreach(var word in words) {
				if(word.StartsWith("#")) break;	// ワードの先頭文字が#なら、ループを抜ける(コメント行として読み飛ばす).
				if(word == "") continue;		// ワードが空なら、1ワード読み飛ばす(ループの先頭に行く).

				// n の値を0,1,2,...7と変化させていくことで、8項目を処理.
				// 各ワードをfloat値に変換し、level_dataに格納する.
				switch(n) {
					case 0: level_data.end_time = float.Parse(word);
							break;
					case 1: level_data.speed = float.Parse(word);
							break;
					case 2: level_data.distance.min = float.Parse(word);
							break;
					case 3: level_data.distance.max = float.Parse(word);
							break;
					case 4: level_data.min_distance = float.Parse(word);
							break;
					case 5: level_data.movement.min = float.Parse(word);
							break;
					case 6: level_data.movement.max = float.Parse(word);
							break;
				}
				n++;
			}

			if(n >= 7) {	// 7項目(以上)がきちんと処理されたなら.
				// List構造のlevel_datasにlevel_dataを追加.
				this.level_datas.Add(level_data);
			} else {		// そうでないなら(エラーの可能性あり).
				if(n == 0) {	// 1ワードも処理していない場合はコメントなので問題なし。何もしない.
				
				} else {	// それ以外ならエラー.
					// データの個数が合っていないことを示すエラーメッセージを表示.
					Debug.LogError("[LevelData] Out of parameter.\n");
				}
			}
		}

	}
	public void update_level(float passage_time) {

		// 経過時間から現在の難易度番号を取得.
		int levelNo = getCurrentLevel(passage_time, this.level_datas);
		// 難易度番号から現在のレベル情報を取得.
		this.current_level = level_datas[levelNo];

		// 難易度番号が過去の難易度番号と違うとき、難易度が切り替わったとして過去の難易度番号を記録.
		if(levelNo != this.last_levelNo) {
			Debug.Log("Level Changed:" + this.last_levelNo.ToString() + " -> " + levelNo.ToString());
			this.last_levelNo = levelNo;
		}
		
	}

	// 現在のレベルを求める.
	private int getCurrentLevel(float currentTime, List<LevelData> ldatas) {
		int i;
		for(i = 0; i < ldatas.Count - 1; i++){
			// 現在時間が、各レベルデータの終了時間を下回る際、レベル決定としてbreakする.
			if(currentTime <= ldatas[i].end_time) break;
		}
		return i;
	}
}
