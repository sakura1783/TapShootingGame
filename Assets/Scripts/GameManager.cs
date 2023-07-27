using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameUp;

    [SerializeField] private DefenceBase defenceBase;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private EnemyGenerator enemyGenerator;

    [SerializeField] private Transform temporaryObjectContainerTran;

    public UIManager uiManager;

    public BulletSelectManager bulletSelectManager;

    [SerializeField] private GameObject fireworkPrefab;

    [SerializeField] private Transform canvasTran;

    public bool isSetUpEnd;


    IEnumerator Start()
    {
        isSetUpEnd = false;

        SwitchGameUp(false);

        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpPlayerController(this);

        enemyGenerator.SetUpEnemyGenerator(this);

        //TransformHelper.SetTemporaryObjectContainerTran(temporaryObjectContainerTran);
        TransformHelper.TemporaryObjectContainerTran = temporaryObjectContainerTran;

        uiManager.HideGameClearSet();
        uiManager.HideGameOverSet();

        //バレット選択ボタンの生成。この処理が終了するまで、次の処理は動かない
        yield return StartCoroutine(bulletSelectManager.GenerateBulletSelectDetail(this));

        yield return StartCoroutine(uiManager.PlayOpening());

        bulletSelectManager.JudgeOpenBullets();

        isSetUpEnd = true;
    }

    /// <summary>
    /// ゲーム終了状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        //ゲーム終了時には画面に残っている敵と一時的に存在しているゲームオブジェクトを全て破壊する
        if (isGameUp)
        {
            enemyGenerator.ClearEnemiesList();

            enemyGenerator.DestroyTemporaryObjectContainer();
        }
    }

    /// <summary>
    /// ゲームクリアの準備
    /// </summary>
    public void PrepareGameClear()
    {
        uiManager.DisplayGameClearSet();

        StartCoroutine(GenerateFireworks());
    }

    /// <summary>
    /// ゲームオーバーの準備
    /// </summary>
    public void PrepareGameOver()
    {
        uiManager.DisplayGameOverSet();
    }

    /// <summary>
    /// プレイヤーと敵との位置から方向を判定
    /// </summary>
    /// <param name="enemyPos"></param>
    /// <returns></returns>
    public Vector3 GetPlayerDirection(Vector3 enemyPos)
    {
        return (playerController.transform.position - enemyPos).normalized;
    }

    /// <summary>
    /// 花火の生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateFireworks()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < Random.Range(5, 8); i++)
        {
            GameObject firework = Instantiate(fireworkPrefab, canvasTran, false);

            //花火の色を変更するために、花火のゲームオブジェクトにアタッチされているパーティクルシステムのメインの情報(DurationやStartColorの情報がある部分)を取得
            ParticleSystem.MainModule main = firework.GetComponent<ParticleSystem>().main;

            //パーティクルの色をランダムな2色に変更
            main.startColor = GetNewTwoRandomColors();

            firework.transform.localPosition = new Vector3(firework.transform.position.x + Random.Range(-400, 400), firework.transform.position.y + Random.Range(500, 800));

            Destroy(firework, 3f);

            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// パーティクルの色をランダムで設定
    /// </summary>
    /// <returns></returns>
    private ParticleSystem.MinMaxGradient GetNewTwoRandomColors()
    {
        //パーティクルシステム用の色の設定を行うためのインスタンスを作成して、コンストラクタを利用して色を2色設定して初期化(パーティクルシステムのStartColorを変更する場合、ParticleSystem.MinMaxGradientの構造体のインスタンスを作成し、それを利用することで色を変更できるようになっている)
        return new ParticleSystem.MinMaxGradient(GetRandomColor(), GetRandomColor());
    }

    /// <summary>
    /// ランダムな色を取得
    /// </summary>
    /// <returns></returns>
    private Color GetRandomColor()
    {
        //Color32はbyte型(0~255)で色の指定が可能なので、色の各成分用の値をRandom.Rangeメソッドを利用してint型で取得し、byte型にキャストして指定
        return new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }
}
