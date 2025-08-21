using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator;
    float angle;
    bool isDead;

    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public GameObject sprite;

    // 外部から呼ばれるためにgetterとして作成
    public bool IsDead()
    {
        return isDead;
    }

    // Start()より先に動く
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = sprite.GetComponent<Animator>();
    }


    void Update()
    {
        // 最高高度に達していない場合に限り、タップの入力を受け付ける
        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
        {
            Flap();
        }

        // 角度を反映
        ApplyAngle();

        // angleが水平だったら、アニメーターのflapフラグをtrueにする
        animator.SetBool("flap", angle >= 0.0f && !isDead);
    }


    public void Flap()
    {
        // 死んでいるときは戻る
        if (isDead) return;

        // 重力が効いていないときは操作しない
        if (rb2d.isKinematic) return;

        // Velocityを直接書き換えて上方向に加速
        rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }


    void ApplyAngle()
    {
        // A現在の速度、B相対速度から進んでいる角度を求める
        // Arctangent逆三角比 AB2辺の比を用意すると、角度が求められる
        // 上昇していると、上向きの角度を
        // 下降していると、下向きの角度を算出する
        float targetAngle;

        // 死亡したら常にひっくり返る
        if (isDead)
        {
            targetAngle = 180.0f;
        }
        else
        {
            targetAngle = Mathf.Atan2(rb2d.velocity.y,
            relativeVelocityX) * Mathf.Rad2Deg;
        }

        // 回転アニメをスムージング
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

        // z回転でアザラシの向きを変更
        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        // クラッシュエフェクト
        Camera.main.SendMessage("Clash");

        // 何かにぶつかったら、死亡フラグをたてる
        isDead = true;
    }

    public void SetSteerActive(bool active)
    {
        // Rigidbodyのオン、オフを切り替える
        rb2d.isKinematic = !active;
    }
}
