using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    public int hp;


    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        transform.Translate(0, -0.01f, 0);

        if (transform.localPosition.y < -1300)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            DestroyBullet(col);

            if (col.TryGetComponent(out Bullet bullet))
            {
                UpdateHp(bullet);
            }
        }
    }

    /// <summary>
    /// 弾と敵の破壊処理
    /// </summary>
    /// <param name="col"></param>
    private void DestroyBullet(Collider2D col)
    {
        Destroy(col.gameObject);
    }

    /// <summary>
    /// HPの更新処理と敵の破壊確認処理
    /// </summary>
    private void UpdateHp(Bullet bullet)
    {
        hp -= bullet.bulletPower;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"残りHP : {hp}");
        }
    }
}
