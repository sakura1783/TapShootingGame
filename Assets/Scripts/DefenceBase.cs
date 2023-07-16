using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//侵入判定のコライダーは子オブジェクトにアタッチされている。その場合、Rigidbodyをつけることにより、親子関係のあるゲームオブジェクトのコライダーの情報を利用することができるようになる
[RequireComponent(typeof(Rigidbody2D))]
public class DefenceBase : MonoBehaviour
{
    public int durability;

    [SerializeField] private Text txtDurability;

    [SerializeField] private Slider slider;

    private int maxDurability;


    void Start()
    {
        maxDurability = durability;

        DisplayDurability();
    }

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

        durability = Mathf.Clamp(durability, 0, maxDurability);

        Debug.Log($"残りの耐久力：{durability}");

        DisplayDurability();

        //TODO 耐久力が0以下になっていないか確認。0以下の場合、ゲームオーバー判定を行う
    }

    /// <summary>
    /// 耐久力の表示更新
    /// </summary>
    private void DisplayDurability()
    {
        txtDurability.text = durability + "/" + maxDurability;

        //ゲージの表示を耐久力の値に合わせて更新(最初はdurability / maxDurabilityの結果が1になるので、ゲージは最大値になる)
        slider.DOValue((float)durability / maxDurability, 0.25f);
    }
}
