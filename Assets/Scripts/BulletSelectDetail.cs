using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BulletSelectDetail : MonoBehaviour
{
    [SerializeField] private Button btnBulletSelect;

    private BulletSelectManager bulletSelectManager;

    [SerializeField] private Image imgBulletButton;
    [SerializeField] private Image imgTimeGauge;

    public BulletDataSO.BulletData bulletData;

    private float duration;  //弾を発射できる残り時間

    private float initialDuration;  //弾を発射できる時間の初期値

    private bool isLoading;  //選択した弾を発射できる状態かどうか。trueなら装填中で発射できる。durationも減る

    public bool isDefaultBullet;


    void Update()
    {
        //初期バレットは時間制限なし
        if (isDefaultBullet)
        {
            return;
        }

        //装填しているバレットでなければ何もしない
        if (!isLoading)
        {
            return;
        }

        duration -= Time.deltaTime;

        imgTimeGauge.DOFillAmount(duration / initialDuration, 0.25f);

        if (duration <= 0)
        {
            duration = 0;

            //TODO 初期バレット以外のバレットを初期状態に戻す
        }
    }

    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager, BulletDataSO.BulletData bulletData)
    {
        this.bulletSelectManager = bulletSelectManager;
        this.bulletData = bulletData;

        imgBulletButton.sprite = this.bulletData.btnSprite;

        btnBulletSelect.onClick.AddListener(OnClickButtonBulletSelect);

        //TODO 弾を選択できない(ボタンを押せない)状態に切り替える

        initialDuration = this.bulletData.duration;
        duration = initialDuration;

        //弾を発射できる残り時間のゲージ表示を0にして見えなくする
        imgTimeGauge.fillAmount = 0;

        //初期バレット確認
        if (this.bulletData.needExp == 0)
        {
            isDefaultBullet = true;

            ChangeLoadingBullet(true);

            //TODO そのほかに設定する処理を追加
        }
    }

    /// <summary>
    /// btnBulletSelectを押した際の処理
    /// </summary>
    public void OnClickButtonBulletSelect()
    {
        Debug.Log("バレット選択");

        //このバレット選択ボタンに設定されているBulletDataをGameDataのcurrentBulletData変数に登録し、現在使用中のバレットとする
        GameData.instance.SetBulletData(bulletData);

        //TODO 装填中の弾を切り替え。あとで処理を変更
        ChangeLoadingBullet(true);

        //初期バレットではなくて、かつ、まだ選択していない場合(重複タップ防止)
        if (!isDefaultBullet && imgTimeGauge.fillAmount == 0)
        {
            imgTimeGauge.fillAmount = 1;

            //TODO そのほかに設定する処理を追加
        }
    }

    /// <summary>
    /// 装填中の弾を切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void ChangeLoadingBullet(bool isSwitch)
    {
        isLoading = isSwitch;
    }
}
