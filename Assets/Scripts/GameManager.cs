using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isGameUp;

    [SerializeField] private DefenceBase defenceBase;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private EnemyGenerator enemyGenerator;


    void Start()
    {
        SwitchGameUp(false);

        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpPlayerController(this);

        enemyGenerator.SetUpEnemyGenerator(this);
    }

    /// <summary>
    /// ゲーム終了状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;

        //TODO ゲーム終了時には画面に残っている敵を全て破壊する
    }
}
