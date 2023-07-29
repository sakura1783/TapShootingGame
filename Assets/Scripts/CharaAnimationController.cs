using UnityEngine;

public class CharaAnimationController : MonoBehaviour
{
    [SerializeField] private Animator charaAnim;

    public static string attackParameter = "Attack";
    public static string hitParameter = "Hit";
    public static string downParameter = "Down";


    /// <summary>
    /// キャラのアニメーションの制御
    /// </summary>
    /// <param name="playAnimationParameter"></param>
    public void PlayAnimation(string playAnimationParameter)
    {
        if (playAnimationParameter == downParameter)
        {
            //AttackとHitのアニメをリセットしてDownのアニメを行う
            charaAnim.ResetTrigger(attackParameter);
            charaAnim.ResetTrigger(hitParameter);

            charaAnim.SetBool(downParameter, true);
            //bool isDowned = true;

            //if (isDowned)
            //{
            //    charaAnim.SetBool(downParameter, false);
            //}
        }
        else
        {
            //TODO なぜAttackだけをリセット？
            charaAnim.ResetTrigger(attackParameter);
            charaAnim.SetTrigger(playAnimationParameter);
        }
    }
}
