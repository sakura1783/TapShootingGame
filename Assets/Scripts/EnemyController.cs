using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
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
            DestroyObjects(col);
        }
    }

    /// <summary>
    /// 弾と敵の破壊処理
    /// </summary>
    /// <param name="col"></param>
    private void DestroyObjects(Collider2D col)
    {
        Destroy(col.gameObject);

        Destroy(gameObject);
    }
}