using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{

    GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        Transform();
    }

    void Transform()
    {
        //座標の更新
        transform.position = _player.transform.position;

        //スクリーン座標を計算する
        var screenPos = Camera.main.WorldToScreenPoint(transform.position);

        //このオブジェクトから見たマウスカーソルの方向を計算する
        var direction = Input.mousePosition - screenPos;

        // マウスカーソルが存在する方向の角度を取得する
        var angle = GetAngle(Vector3.zero, direction);

        //このオブジェクトマウスカーソルの方向を見るようにする
        //(子オブジェクトの魔法陣が移動する)
        var angles = transform.localEulerAngles;
        angles.z = angle;
        transform.localEulerAngles = angles;
    }
    public static float GetAngle(Vector2 from, Vector2 to)
    {   //二つの点から角度を求める
        var dx = to.x - from.x;
        var dy = to.y - from.y;
        var rad = Mathf.Atan2(dy, dx);
        return rad * Mathf.Rad2Deg;
    }

}
