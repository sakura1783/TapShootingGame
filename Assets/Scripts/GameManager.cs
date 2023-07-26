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


    IEnumerator Start()
    {
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

        bulletSelectManager.JudgeOpenBullets();
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
}
