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


    void Start()
    {
        StartCoroutine(GenerateBulletSelectDetail());
    }

    /// <summary>
    /// 弾選択用ボタンの生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateBulletSelectDetail()
    {
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
}
