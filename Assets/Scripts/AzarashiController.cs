using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    Rigidbody2D rb2d;

    public float maxHeight;
    public float flapVelocity;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // 最高高度に達していない場合に限り、タップの入力を受け付ける
        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
        {
            Flap();
        }
    }


    public void Flap()
    {
        // Velocityを直接書き換えて上方向に加速
        rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }
}
