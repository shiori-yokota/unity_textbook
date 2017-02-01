using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	// 動く速度を指定する
	public float speed = 3;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		// 左右キー・上下キーのキーボード入力を取得する
		float speed_x = Input.GetAxis ("Horizontal") * speed;
		float speed_y = Input.GetAxis ("Vertical") * speed;

		//自分に力を与えて動かす（転がす）
		this.GetComponent<Rigidbody>().AddForce(speed_x,0,speed_y);
	}
}
