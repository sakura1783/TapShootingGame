using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup gameClearSetCanvasGroup;
    [SerializeField] private CanvasGroup gameOverSetCanvasGroup;

    [SerializeField] private Text txtGameOver;
    [SerializeField] private Text txtDurability;
    [SerializeField] private Text txtExp;

    [SerializeField] private Slider slider;


    /// <summary>
    /// ゲームクリア表示を隠す
    /// </summary>
    public void HideGameClearSet()
    {
        gameClearSetCanvasGroup.alpha = 0;
    }

    /// <summary>
    /// ゲームクリア表示を行う
    /// </summary>
    public void DisplayGameClearSet()
    {
        gameClearSetCanvasGroup.DOFade(1, 0.25f);
    }

    /// <summary>
    /// ゲームオーバー表示を隠す
    /// </summary>
    public void HideGameOverSet()
    {
        gameOverSetCanvasGroup.alpha = 0;
    }

    /// <summary>
    /// ゲームオーバー表示を行う
    /// </summary>
    public void DisplayGameOverSet()
    {
        gameOverSetCanvasGroup.DOFade(1, 1);

        string text = "Game Over";

        txtGameOver.DOText(text, 1.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 耐久力の表示更新
    /// </summary>
    /// <param name="durability"></param>
    /// <param name="maxDurability"></param>
    public void DisplayDurability(int durability, int maxDurability)
    {
        txtDurability.text = durability + "/" + maxDurability;

        //ゲージの表示を耐久力の値に合わせて更新(最初はdurability / maxDurabilityの結果が1になるので、ゲージは最大値になる)
        slider.DOValue((float)durability / maxDurability, 0.25f);
    }

    /// <summary>
    /// TotalExpの更新
    /// </summary>
    /// <param name="totalExp"></param>
    public void UpdateDisplayTotalExp(int totalExp)
    {
        txtExp.text = totalExp.ToString();
    }
}
