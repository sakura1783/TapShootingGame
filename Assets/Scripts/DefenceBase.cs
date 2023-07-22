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

    [SerializeField] private FloatingMessage floatingMessagePrefab;

    [SerializeField] private Transform floatingTran;


    public void SetUpDefenceBase(GameManager gameManager)
    {
        this.gameManager = gameManager;

        durability = GameData.instance.GetDurability();
        maxDurability = durability;

        this.gameManager.uiManager.DisplayDurability(durability, maxDurability);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            int damage = 0;

            //侵入してきたコライダーをオフにする(重複判定を防ぐため)
            col.GetComponent<CapsuleCollider2D>().enabled = false;

            //侵入してきたゲームオブジェクトにBulletクラスがアタッチされていたら
            if (col.TryGetComponent(out Bullet bullet))
            {
                damage = bullet.bulletPower;
            }

            //上のif文が処理されず、侵入してきたゲームオブジェクトにEnemyControllerクラスがアタッチされていたら
            else if (col.TryGetComponent(out EnemyController enemy))
            {
                damage = enemy.enemyData.attackPoint;
            }

            UpdateDurability(damage);

            CreateFloatingMessageToDamage(damage);

            GenerateEnemyAttackEffect(col.gameObject.transform);

            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// 耐久力の更新
    /// </summary>
    /// <param name="enemy"></param>
    private void UpdateDurability(int damage)
    {
        durability -= damage;

        durability = Mathf.Clamp(durability, 0, maxDurability);

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

    /// <summary>
    /// エネミーからのダメージ値用のフロート表示の生成
    /// </summary>
    /// <param name="damage"></param>
    private void CreateFloatingMessageToDamage(int damage)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingTran, false);

        floatingMessage.DisplayFloatingMessage(damage, FloatingMessage.FloatingMessageType.PlayerDamage);
    }
}
