using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;  //UnityActionを使用する際には宣言が必要

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    public EnemyDataSO.EnemyData enemyData;

    [SerializeField] private Image imgEnemy;

    private int hp;
    private int maxHp;

    //public int attackPower;

    [SerializeField] private Slider slider;

    [SerializeField] private GameObject bulletEfectPrefab;

    [SerializeField] private GameObject enemyBulletPrefab;

    //private bool isBoss;

    private EnemyGenerator enemyGenerator;

    private UnityAction<Transform, float> moveEvent;  //ここに敵の移動方法に応じた移動用のメソッドを登録する

    [SerializeField] private Transform floatingTran;

    [SerializeField] private FloatingMessage floatingMessagePrefab;


    void Start()
    {
        Application.targetFrameRate = 60;
    }

    //void Update()
    //{
    //    if (enemyData.enemyType != EnemyType.Boss)
    //    {
    //        transform.Translate(0, -0.01f, 0);
    //    }

    //    //if (transform.localPosition.y < -1300)
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //}

    public void SetUpEnemyController(EnemyDataSO.EnemyData enemyData, EnemyGenerator enemyGenerator)
    {
        this.enemyData = enemyData;
        this.enemyGenerator = enemyGenerator;

        //this.isBoss = isBoss;

        if (this.enemyData.enemyType != EnemyType.Boss)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-400, 400), transform.localPosition.y, 0);
        }
        else
        {
            //徐々に下方向に移動
            //transform.DOLocalMoveY(transform.position.y - 500, 3);

            //大きさを2倍にする
            transform.localScale = Vector3.one * 1.5f;

            //HPゲージを高い位置にする
            slider.transform.localPosition = new Vector3(0, 170, 0);

            //hp *= 3;
        }

        imgEnemy.sprite = this.enemyData.enemySprite;

        maxHp = this.enemyData.hp;

        hp = maxHp;

        DisplayHpGauge();

        //SetMoveByMoveType();

        //MoveEventSOスクリプタブル・オブジェクトのGetMoveEventメソッドを実行し、戻り値で移動用のメソッドを受け取る。ここで移動方法を決定
        moveEvent = this.enemyGenerator.moveEventSO.GetMoveEvent(enemyData.moveType);

        //Invokeメソッドを実行すると、moveEvent変数に登録されたメソッド(今回は移動用のメソッド)を実行する
        moveEvent.Invoke(transform, enemyData.moveDuration);

        if (enemyData.moveType == MoveType.Straight || enemyData.moveType == MoveType.Boss_Horizontal)
        {
            StartCoroutine(EnemyShot());
        }
    }

    /// <summary>
    /// EnemyControllerの追加設定
    /// </summary>
    /// <param name="enemyGenerator"></param>
    //public void AdditionalSetUpEnemyController(EnemyGenerator enemyGenerator)
    //{
    //    this.enemyGenerator = enemyGenerator;
    //}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            //DestroyBullet(col);

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
        CreateFloatingMessageToBulletPower(bullet.bulletData.bulletPower);

        hp -= bullet.bulletData.bulletPower;

        hp = Mathf.Clamp(hp, 0, maxHp);

        DisplayHpGauge();

        if (hp <= 0)
        {
            if (enemyData.enemyType == EnemyType.Boss)
            {
                enemyGenerator.SwitchBossDestroyed(true);
            }

            GameData.instance.UpdateTotalExp(enemyData.exp);
            enemyGenerator.PrepareDisplayTotalExp(enemyData.exp);

            Destroy(gameObject);
        }
        //else
        //{
        //    Debug.Log($"残りHP : {hp}");
        //}

        //氷のバレット以外の場合、そのバレットを破壊(氷のバレットは貫通する)
        if (bullet.bulletData.bulletType != BulletDataSO.BulletType.Player_Ice)
        {
            Destroy(bullet.gameObject);
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyShot()
    {
        while (true)  //<= 条件にtrueを指定すると無制限のループ処理になる
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, transform);
            bullet.GetComponent<Bullet>().ShotBullet(enemyGenerator.PrepareGetPlayerDirection(transform.position));

            if (enemyData.moveType == MoveType.Boss_Horizontal)
            {
                bullet.transform.SetParent(TransformHelper.GetTemporaryObjectContainerTran());
            }

            yield return new WaitForSeconds(5);
        }
    }

    /// <summary>
    /// 弾の攻撃力値分のフロート表示の生成
    /// </summary>
    private void CreateFloatingMessageToBulletPower(int bulletPower)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingTran, false);

        floatingMessage.DisplayFloatingMessage(bulletPower, FloatingMessage.FloatingMessageType.EnemyDamage);
    }

    /// <summary>
    /// 移動タイプに応じた移動方法を選択して実行
    /// </summary>
    //private void SetMoveByMoveType()
    //{
    //    switch (enemyData.moveType)
    //    {
    //        case MoveType.Straight:
    //            MoveStraight();
    //            break;
    //        case MoveType.Meandering:
    //            MoveMeandering();
    //            break;
    //        case MoveType.Boss_Horizontal:
    //            MoveBossHorizontal();
    //            break;
    //    }
    //}

    /// <summary>
    /// 直進移動
    /// </summary>
    //private void MoveStraight()
    //{
    //    Debug.Log("直進");

    //    transform.DOLocalMoveY(-3000, enemyData.moveDuration).SetLink(gameObject);
    //}

    /// <summary>
    /// 蛇行移動
    /// </summary>
    //private void MoveMeandering()
    //{
    //    Debug.Log("蛇行");

    //    transform.DOLocalMoveX(transform.position.x + Random.Range(150, 200), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetLink(gameObject);
    //    transform.DOLocalMoveY(-3000, enemyData.moveDuration).SetLink(gameObject);
    //}

    /// <summary>
    /// ボス・水平移動
    /// </summary>
    //private void MoveBossHorizontal()
    //{
    //    transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);

    //    transform.DOLocalMoveY(-500, 3).OnComplete(() =>
    //    {
    //        Sequence sequence = DOTween.Sequence().SetLink(gameObject);
    //        sequence.Append(transform.DOLocalMoveX(transform.localPosition.x + 400, 2.5f).SetEase(Ease.Linear));
    //        sequence.Append(transform.DOLocalMoveX(transform.localPosition.x - 400, 5f).SetEase(Ease.Linear));
    //        sequence.Append(transform.DOLocalMoveX(transform.localPosition.x, 2.5f).SetEase(Ease.Linear));
    //        sequence.AppendInterval(1).SetLoops(-1, LoopType.Restart);  //LoopType.Restartは最初からやり直す
    //    });
    //}
}
