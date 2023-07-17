using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    public int hp;
    private int maxHp;

    public int attackPower;

    [SerializeField] private Slider slider;

    [SerializeField] private GameObject bulletEfectPrefab;

    private bool isBoss;

    private EnemyGenerator enemyGenerator;


    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (!isBoss)
        {
            transform.Translate(0, -0.01f, 0);
        }

        //if (transform.localPosition.y < -1300)
        //{
        //    Destroy(gameObject);
        //}
    }

    public void SetUpEnemyController(bool isBoss = false)
    {
        this.isBoss = isBoss;

        if (!this.isBoss)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-400, 400), transform.localPosition.y, 0);
        }
        else
        {
            //徐々に下方向に移動
            transform.DOLocalMoveY(transform.position.y - 500, 3);

            //大きさを2倍にする
            transform.localScale = Vector3.one * 2;

            //HPゲージを高い位置にする
            //slider.transform.position = new Vector3(0, 150, 0);

            hp *= 3;
        }

        maxHp = hp;

        DisplayHpGauge();
    }

    /// <summary>
    /// EnemyControllerの追加設定
    /// </summary>
    /// <param name="enemyGenerator"></param>
    public void AdditionalSetUpEnemyController(EnemyGenerator enemyGenerator)
    {
        this.enemyGenerator = enemyGenerator;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log($"ぶつかったゲームオブジェクト：{col.gameObject.name}");

        if (col.CompareTag("Bullet"))
        {
            DestroyBullet(col);

            if (col.TryGetComponent(out Bullet bullet))
            {
                UpdateHp(bullet);

                GenerateBulletEfect(col.gameObject.transform);
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

        hp = Mathf.Clamp(hp, 0, maxHp);

        DisplayHpGauge();

        if (hp <= 0)
        {
            if (isBoss)
            {
                enemyGenerator.SwitchBossDestroyed(true);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"残りHP : {hp}");
        }
    }

    /// <summary>
    /// HPゲージの表示更新
    /// </summary>
    private void DisplayHpGauge()
    {
        slider.DOValue((float)hp / maxHp, 0.25f);
    }

    /// <summary>
    /// 被弾時のヒット演出用のパーティクル生成
    /// </summary>
    /// <param name="bulletTran"></param>
    private void GenerateBulletEfect(Transform bulletTran)
    {
        GameObject particle = Instantiate(bulletEfectPrefab, bulletTran, false);

        particle.transform.SetParent(transform);

        Destroy(particle, 3);
    }
}
