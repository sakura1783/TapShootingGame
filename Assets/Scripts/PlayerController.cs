using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    private GameManager gameManager;

    [SerializeField] private CharaAnimationController charaAnimationController;


    void Update()
    {
        if (!gameManager.isSetUpEnd)
        {
            return;
        }

        if (gameManager.isGameUp)
        {
            return;
        }

        if (gameManager.uiManager.isAlerting)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Vector3 direction = tapPos - transform.position;
            ////Vector3.Scaleで第一引数の各成分と第二引数の同じ成分を乗算する。この処理で、不要なZ成分の除去ができる
            //direction = Vector3.Scale(direction, new Vector3(1, 1, 0));
            //direction = direction.normalized;

            //上の処理を1行で記述
            Vector3 direction = Vector3.Scale(tapPos - transform.position, new Vector3(1, 1, 0)).normalized;

            //GenerateBullet(direction);
            PrepareGenerateBullet(direction);

            //攻撃アニメ再生
            charaAnimationController.PlayAnimation(CharaAnimationController.attackParameter);

            SoundManager.instance.PlayVoice(SoundDataSO.VoiceType.Attack);
        }
    }

    public void SetUpPlayerController(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private void PrepareGenerateBullet(Vector3 direction)
    {
        BulletDataSO.BulletData bulletData = GameData.instance.GetCurrentBulletData();

        switch (bulletData.bulletType)
        {
            case BulletDataSO.BulletType.Player_Leaf:
            case BulletDataSO.BulletType.Player_Fire:
                GenerateBullet(direction, bulletData);
                break;

            case BulletDataSO.BulletType.Player_Ice:
                for (float i = -0.25f; i <= 0.25f; i += 0.5f)
                {
                    //2方向に扇状に発射する
                    GenerateBullet(new Vector3((direction.x + i), direction.y, direction.z), bulletData);
                }
                break;

            case BulletDataSO.BulletType.Player_Thunder:
                for (int i = -1; i < 2; i++)
                {
                    //3方向に扇状に発射する
                    GenerateBullet(new Vector3((direction.x + (0.5f * i)), direction.y, direction.z), bulletData);
                }
                break;
        }
    }

    /// <summary>
    /// 弾の生成
    /// </summary>
    private void GenerateBullet(Vector3 direction, BulletDataSO.BulletData bulletData)
    {
        Instantiate(bulletPrefab, transform).ShotBullet(direction, bulletData);
    }
}
