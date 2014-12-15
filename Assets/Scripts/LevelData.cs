using UnityEngine;
using System.Collections;

public class LevelData {

	public struct Range {	// 範囲を表す構造体.
		public float min;		// 範囲の最小値.
		public float max;		// 範囲の最大値.
	};

	public Range distance;	// 上下ブロック間の距離.
	public Range movement;	// 上下ブロックの移動量.
	public float min_distance;	// 最小距離(どこまで狭くするか).
	public float end_time;	// 終了時間.
	public float speed;		// プレイヤーの移動速度.

	public LevelData() {
		this.distance.min = -0.2f;
		this.distance.max = 0.1f;
		this.min_distance = 5.0f;
		this.movement.min = -0.1f;
		this.movement.max = 0.1f;
		this.end_time = 0.0f;
		this.speed = 20.0f;
	}
}
