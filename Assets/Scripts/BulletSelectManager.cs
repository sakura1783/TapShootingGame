using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSelectManager : MonoBehaviour
{
    [SerializeField] private BulletSelectDetail bulletSelectDetailPrefab;

    [SerializeField] private Transform bulletButtonTran;

    private const int maxBulletButtonCount = 4;

    public List<BulletSelectDetail> bulletSelectDetailList = new();

    [SerializeField] private BulletDataSO bulletDataSO;

    private GameManager gameManager;

    
    //void Start()
    //{
    //    StartCoroutine(GenerateBulletSelectDetail());
    //}

    /// <summary>
    /// バレット選択用ボタンの生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateBulletSelectDetail(GameManager gameManager)
    {
        this.gameManager = gameManager;

        for (int i = 0; i < maxBulletButtonCount; i++)
        {
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletButtonTran, false);

            //TODO あとで引数を変更
            bulletSelectDetail.SetUpBulletSelectDetail(this, bulletDataSO.bulletDataList[i]);

            bulletSelectDetailList.Add(bulletSelectDetail);

            //0.25秒だけ処理を中断(順番にボタンが生成されるようにする演出)
            yield return new WaitForSeconds(0.25f);
        }

        //TODO 使用するバレットの情報を初期設定。後で、引数を変更する
        GameData.instance.SetBulletData(bulletDataSO.bulletDataList[0]);
    }

    /// <summary>
    /// 全てのバレット選択ボタンに対してNoで照合を行い、装填中と未装填の状態を変更
    /// </summary>
    /// <param name="bulletNo"></param>
    public void ChangeLoadingBulletSettings(int bulletNo)
    {
        for (int i = 0; i < bulletSelectDetailList.Count; i++)
        {
            //この要素のバレットのNoが選択したバレット(bulletNo)と同じであるなら
            if (bulletSelectDetailList[i].bulletData.bulletNo == bulletNo)
            {
                //装填中にする
                bulletSelectDetailList[i].ChangeLoadingBullet(true);

                Debug.Log($"装填中のバレットのNo : {bulletNo}");
            }
            else
            {
                //未装填中にする
                bulletSelectDetailList[i].ChangeLoadingBullet(false);
            }
        }
    }

    /// <summary>
    /// 初期バレットを装填中のバレットとして設定
    /// </summary>
    public void ActivateDefaultBullet()
    {
        foreach (BulletSelectDetail bulletSelectDetail in bulletSelectDetailList)
        {
            if (bulletSelectDetail.isDefaultBullet)
            {
                bulletSelectDetail.OnClickButtonBulletSelect();

                Debug.Log("初期バレットを装填中のバレットとして設定");

                return;
            }
        }
    }

    /// <summary>
    /// 使用可能バレット選択ボタンの確認と更新
    /// </summary>
    public void JudgeOpenBullets()
    {
        int totalExp = GameData.instance.GetTotalExp();

        //バレットごとに必要なExpを超えているか確認
        foreach (BulletSelectDetail bulletSelectDetail in bulletSelectDetailList)
        {
            if (gameManager.isGameUp)
            {
                bulletSelectDetail.SwitchActivateBulletButton(false, 0.5f);

                continue;  //if(gameManager.isGameUp)がtrueの場合、以下の処理がスキップされる。    
            }

            //持っているExpの値がバレットに設定されているコストの値以上であれば、そのバレット選択ボタンをアクティブにしてタップできるようにする
            if (bulletSelectDetail.bulletData.needExp <= totalExp)
            {
                bulletSelectDetail.SwitchActivateBulletButton(true, 1f);
            }
            //持っているExpの値がバレットに設定されているコストの値以下であれば、そのバレット選択ボタンを非アクティブにしてタップできなくする
            else
            {
                bulletSelectDetail.SwitchActivateBulletButton(false, 0.5f);
            }
        }
    }
}
