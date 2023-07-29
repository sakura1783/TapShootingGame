using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BulletSelectManager : MonoBehaviour
{
    [SerializeField] private BulletSelectDetail bulletSelectDetailPrefab;

    [SerializeField] private Transform bulletButtonTran;

    private const int maxBulletButtonCount = 4;

    public List<BulletSelectDetail> bulletSelectDetailList = new();

    [SerializeField] private BulletDataSO bulletDataSO;

    private GameManager gameManager;

    //[SerializeField] private ElementDataSO elementDataSO;

    
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

        //BulletDataSOスクリプタブル・オブジェクトのデータから、利用者の種類がPlayerのバレットの情報だけを代入するListを用意する(プレイヤーと敵のバレットの情報が混在する状態を防ぐ)
        List<BulletDataSO.BulletData> playerBulletDatas = new();

        //バレットの利用者の種類がPlayerのバレットの情報だけを抽出してListを作成
        playerBulletDatas = bulletDataSO.bulletDataList.Where(x => x.userType == BulletDataSO.UserType.Player).ToList();

        for (int i = 0; i < maxBulletButtonCount; i++)
        {
            BulletSelectDetail bulletSelectDetail = Instantiate(bulletSelectDetailPrefab, bulletButtonTran, false);

            bulletSelectDetail.SetUpBulletSelectDetail(this, playerBulletDatas[i]);

            bulletSelectDetailList.Add(bulletSelectDetail);

            //0.25秒だけ処理を中断(順番にボタンが生成されるようにする演出)
            yield return new WaitForSeconds(0.25f);
        }

        //使用するバレットの情報を初期設定
        GameData.instance.SetBulletData(playerBulletDatas[0]);
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

                //bulletSelectDetailList[i].ChangeColorToBulletButton(new Color(0.65f, 0.65f, 0.65f));
            }
            else
            {
                //未装填中にする
                bulletSelectDetailList[i].ChangeLoadingBullet(false);

                //bulletSelectDetailList[i].ChangeColorToBulletButton(new Color(1f, 1f, 1f));
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

                continue;  //if(gameManager.isGameUp)がtrueの場合、以下の処理がスキップされて、次の要素のforeachの処理に入る
            }

            //コストを支払っているかどうか、戻り値を持つメソッドを利用して確認する
            //if (bulletSelectDetail.GetStateBulletCostPayment())
            if (bulletSelectDetail.IsCostPaid)
            {
                bulletSelectDetail.SwitchActivateBulletButton(true, 1f);

                continue;
            }

            //持っているExpの値がバレットに設定されているコストの値以上であれば、そのバレット選択ボタンをアクティブにしてタップできるようにする
            if (bulletSelectDetail.bulletData.needExp <= totalExp)
            {
                bulletSelectDetail.SwitchActivateBulletButton(true, 1f);

                if (!bulletSelectDetail.IsAnimation)
                {
                    bulletSelectDetail.OpenBulletAnimation(true);
                }
            }
            //持っているExpの値がバレットに設定されているコストの値以下であれば、そのバレット選択ボタンを非アクティブにしてタップできなくする
            else
            {
                bulletSelectDetail.SwitchActivateBulletButton(false, 0.5f);

                if (bulletSelectDetail.IsAnimation)
                {
                    bulletSelectDetail.OpenBulletAnimation(false);
                }
            }
        }
    }

    /// <summary>
    /// 選択したバレットのコスト支払いとそれに関連する処理
    /// </summary>
    /// <param name="costExp"></param>
    public void SelectedBulletCostPayment(int costExp)
    {
        //画面のExp表示の更新
        gameManager.uiManager.UpdateDisplayTotalExp(-costExp);

        //フロート表示
        gameManager.uiManager.CreateFloatingMessageToExp(-costExp, FloatingMessage.FloatingMessageType.BulletCost);

        //コストの支払い
        GameData.instance.UpdateTotalExp(-costExp);

        //使用可能バレットの確認と更新
        JudgeOpenBullets();
    }

    /// <summary>
    /// ElementTypeからボタンの背景画像(Sprite)を取得
    /// </summary>
    /// <param name="elementType"></param>
    /// <returns></returns>
    //public Sprite GetElementTypeSprite(ElementType elementType)
    //{
    //    foreach (ElementDataSO.ElementData elementData in elementDataSO.elementDataList)
    //    {
    //        if (elementData.elementType == elementType)
    //        {
    //            return elementData.elementSprite;
    //        }
    //    }

    //    //どれも一致しない場合はnullを戻す
    //    return null;
    //}

    /// <summary>
    /// BulletTypeよりBulletDataを検索して取得
    /// </summary>
    /// <param name="bulletType"></param>
    /// <returns></returns>
    public BulletDataSO.BulletData GetBulletData(BulletDataSO.BulletType bulletType)
    {
        foreach (BulletDataSO.BulletData bulletData in bulletDataSO.bulletDataList.Where(x => x.bulletType == bulletType))
        {
            return bulletData;
        }

        return null;
    }
}
