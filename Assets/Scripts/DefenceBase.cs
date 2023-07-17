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

    //[SerializeField] private Text txtDurability;

    //[SerializeField] private Slider slider;

    private int maxDurability;

    private GameManager gameManager;

    [SerializeField] private GameObject enemyAttackEffectPrefab;


    public void SetUpDefenceBase(GameManager gameManager)
    {
        this.gameManager = gameManager;

        maxDurability = durability;

        this.gameManager.uiManager.DisplayDurability(durability, maxDurability);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            if (col.TryGetComponent(out EnemyController enemy))
            {
                UpdateDurability(enemy);
            }

            GenerateEnemyAttackEffect(col.gameObject.transform);

            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// 耐久力の更新
    /// </summary>
    /// <param name="enemy"></param>
    private void UpdateDurability(EnemyController enemy)
    {
        durability -= enemy.enemyData.attackPoint;

        durability = Mathf.Clamp(durability, 0, maxDurability);

        Debug.Log($"残りの耐久力：{durability}");

        gameManager.uiManager.DisplayDurability(durability, maxDurability);

        if (durability <= 0 && gameManager.isGameUp == false)
        {
            Debug.Log("ゲームオーバー");

            gameManager.SwitchGameUp(true);

            gameManager.PrepareGameOver();
        }
    }

    /// <summary>
    /// 耐久力の表示更新
    /// </summary>
    //private void DisplayDurability()
    //{
    //    txtDurability.text = durability + "/" + maxDurability;

    //    //ゲージの表示を耐久力の値に合わせて更新(最初はdurability / maxDurabilityの結果が1になるので、ゲージは最大値になる)
    //    slider.DOValue((float)durability / maxDurability, 0.25f);
    //}

    /// <summary>
    /// 敵が拠点に侵入した際の攻撃演出用のパーティクル生成
    /// </summary>
    private void GenerateEnemyAttackEffect(Transform enemyTran)
    {
        GameObject particle = Instantiate(enemyAttackEffectPrefab, enemyTran, false);

        //particle.transform.SetParent(TransformHelper.GetTemporaryObjectContainerTran());
        particle.transform.SetParent(TransformHelper.TemporaryObjectContainerTran);

        Destroy(particle, 3);
    }
}
