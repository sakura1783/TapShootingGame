using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 属性間の相性の確認
/// </summary>
public static class ElementCompatibilityHelper
{
    /// <summary>
    /// 攻撃側と防御側の属性の種類によって弱点かどうかを判定。trueは弱点、falseは弱点じゃない
    /// </summary>
    /// <param name="attackElementType">攻撃側の属性の種類</param>
    /// <param name="defenceElementType">防御側の属性の種類</param>
    /// <returns></returns>
    public static bool GetElementCompatibility(ElementType attackElementType, ElementType defenceElementType)
    {
        //火 vs 氷or水
        if (attackElementType == ElementType.Fire)
        {
            if (defenceElementType == ElementType.Ice || defenceElementType == ElementType.Water)
            {
                return true;
            }
        }
        //氷 vs 火
        else if (attackElementType == ElementType.Ice)
        {
            if (defenceElementType == ElementType.Fire)
            {
                return true;
            }
        }
        //雷 vs 闇
        else if (attackElementType == ElementType.Thunder)
        {
            if (defenceElementType == ElementType.Darkness)
            {
                return true;
            }
        }
        //水 vs 火
        else if (attackElementType == ElementType.Water)
        {
            if (defenceElementType == ElementType.Fire)
            {
                return true;
            }
        }
        //
        else if (attackElementType == ElementType.Darkness)
        {
            if (defenceElementType == ElementType.Thunder)
            {
                return true;
            }
        }

        return false;
    }
}
