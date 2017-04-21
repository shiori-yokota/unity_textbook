using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    // ターゲットへの参照
    public Transform target;
    private Vector3 offset;

    void Start() {
        offset = GetComponent<Transform>().position - target.position;
    }
	
	// Update is called once per frame
	void Update () {
        // 自分の座標にtargetの座標を代入する
        GetComponent<Transform>().position = target.position + offset;
        GetComponent<Transform>().rotation = target.rotation;


    }
}
