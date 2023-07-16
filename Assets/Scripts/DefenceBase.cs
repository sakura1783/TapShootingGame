using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//侵入判定のコライダーは子オブジェクトにアタッチされている。その場合、Rigidbodyをつけることにより、親子関係のあるゲームオブジェクトのコライダーの情報を利用することができるようになる
[RequireComponent(typeof(Rigidbody2D))]
public class DefenceBase : MonoBehaviour
{
    public int durability;


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (col.TryGetComponent(out EnemyController enemy))
            {
                UpdateDurability(enemy);
            }

            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// 耐久力の更新
    /// </summary>
    /// <param name="enemy"></param>
    private void UpdateDurability(EnemyController enemy)
    {
        durability -= enemy.attackPower;

        Debug.Log($"残りの耐久力：{durability}");

        //TODO 耐久力が0以下になっていないか確認。0以下の場合、ゲームオーバー判定を行う
    }
}
