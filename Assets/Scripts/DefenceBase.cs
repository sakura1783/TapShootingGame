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

    [SerializeField] private CharaAnimationController charaAnimationController;


    public void SetUpDefenceBase(GameManager gameManager)
    {
        this.gameManager = gameManager;

        durability = GameData.instance.GetDurability();
        maxDurability = durability;

        this.gameManager.uiManager.DisplayDurability(durability, maxDurability);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") || col.CompareTag("EnemyBullet"))
        {
            //int damage = 0;  //<= 下の処理に変更

            //タプル型で変数を宣言。2つの型を1つの変数内に用意できる。valueは最終的なダメージ値、isWeaknessは弱点かどうか
            (int value, bool isWeakness) damage = (0, false);

            //侵入してきたコライダーをオフにする(重複判定を防ぐため)
            col.GetComponent<CapsuleCollider2D>().enabled = false;

            //侵入してきたゲームオブジェクトにBulletクラスがアタッチされていたら
            if (col.TryGetComponent(out Bullet bullet))
            {
                damage = JudgeDamageToElementType(bullet.bulletData.bulletPower, bullet.bulletData.elementType);
            }

            //上のif文が処理されず、侵入してきたゲームオブジェクトにEnemyControllerクラスがアタッチされていたら
            else if (col.TryGetComponent(out EnemyController enemy))
            {
                damage = JudgeDamageToElementType(enemy.enemyData.attackPoint, enemy.enemyData.elementType);
            }

            UpdateDurability(damage.value);

            CreateFloatingMessageToDamage(damage.value, damage.isWeakness);

            GenerateEnemyAttackEffect(col.gameObject.transform);

            Destroy(col.gameObject);

            SoundManager.instance.PlaySE(SoundDataSO.SEType.Damage);
            SoundManager.instance.PlayVoice(SoundDataSO.VoiceType.Damage);
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

            //ダウンアニメ再生
            charaAnimationController.PlayAnimation(CharaAnimationController.downParameter);
        }

        //ヒットアニメ再生
        charaAnimationController.PlayAnimation(CharaAnimationController.hitParameter);
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
    private void CreateFloatingMessageToDamage(int damage, bool isWeakness)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingTran, false);

        floatingMessage.DisplayFloatingMessage(damage, FloatingMessage.FloatingMessageType.PlayerDamage, isWeakness);
    }

    /// <summary>
    /// 属性の相性判定を行って、ダメージの最終値と弱点かどうかを判定する
    /// </summary>
    /// <param name="attackPower"></param>
    /// <param name="attackElementType"></param>
    /// <returns></returns>
    private (int, bool) JudgeDamageToElementType(int attackPower, ElementType attackElementType)  //<= 戻り値をタプル型に変更
    {
        //タプル型用に別々の変数を用意する(この変数もタプル型にすれば、1つでも実装可能)
        int lastDamage = attackPower;
        bool isWeakness = false;
        //(int lastDamage, bool isWeakness) returnValue = (attackPower, false);

        if (ElementCompatibilityHelper.GetElementCompatibility(attackElementType, GameData.instance.GetCurrentBulletData().elementType))
        {
            lastDamage = Mathf.FloorToInt(attackPower * GameData.instance.DamageRatio);

            isWeakness = true;

            Debug.Log("弱点です");
        }

        return (lastDamage, isWeakness);
    }
}
