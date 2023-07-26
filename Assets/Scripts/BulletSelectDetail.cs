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

    [SerializeField] private Text txtExpValue;


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

            //初期バレット以外のバレットを初期状態に戻す
            InitializeBulletState();
        }
    }

    public void SetUpBulletSelectDetail(BulletSelectManager bulletSelectManager, BulletDataSO.BulletData bulletData)
    {
        this.bulletSelectManager = bulletSelectManager;
        this.bulletData = bulletData;

        imgBulletButton.sprite = this.bulletData.btnSprite;

        btnBulletSelect.onClick.AddListener(OnClickButtonBulletSelect);

        //弾を選択できない(ボタンを押せない)状態に切り替える
        SwitchActivateBulletButton(false, 0.5f);

        initialDuration = this.bulletData.duration;
        duration = initialDuration;

        //弾を発射できる残り時間のゲージ表示を0にして見えなくする
        imgTimeGauge.fillAmount = 0;

        txtExpValue.text = this.bulletData.needExp.ToString();

        //初期バレット確認
        if (this.bulletData.needExp == 0)
        {
            isDefaultBullet = true;

            ChangeLoadingBullet(true);

            SwitchActivateDisplayBulletExp(false);

            SwitchActivateBulletButton(true, 1f);

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

        //このバレット選択ボタンを装填中に切り替えて、それ以外のバレット選択ボタンを未装填に変更
        bulletSelectManager.ChangeLoadingBulletSettings(bulletData.bulletNo);

        //初期バレットではなく、まだ選択していない場合(重複タップ防止)
        if (!isDefaultBullet && imgTimeGauge.fillAmount == 0)
        {
            imgTimeGauge.fillAmount = 1;

            //装填中のバレット選択ボタンのExp表示を非表示にする
            SwitchActivateDisplayBulletExp(false);

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

    /// <summary>
    /// 初期バレット以外のバレットを初期状態に戻す
    /// </summary>
    private void InitializeBulletState()
    {
        isLoading = false;

        //初期バレットを装填中のバレットとして設定
        bulletSelectManager.ActivateDefaultBullet();

        duration = initialDuration;

        SwitchActivateDisplayBulletExp(true);
    }

    /// <summary>
    /// バレット解放に必要なExp表示の表示切り替え
    /// </summary>
    private void SwitchActivateDisplayBulletExp(bool isSwitch)
    {
        txtExpValue.gameObject.SetActive(isSwitch);
    }

    /// <summary>
    /// バレット選択ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateBulletButton(bool isSwitch, float alphaValue)
    {
        btnBulletSelect.interactable = isSwitch;

        //ボタンの画像のAlpha値を変更
        SetAlpha(alphaValue);
    }

    /// <summary>
    /// ボタンの画像のAlpha値を変更
    /// </summary>
    /// <param name="alphaValue"></param>
    private void SetAlpha(float alphaValue)
    {
        Color color = imgBulletButton.color;
        color.a = alphaValue;
        imgBulletButton.color = color;
    }
}
