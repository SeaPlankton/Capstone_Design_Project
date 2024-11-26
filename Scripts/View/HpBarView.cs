using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 임시 테스트용, presenter 생략
/// </summary>
public class HpBarView : MonoBehaviour
{
    public Image FilledHpImg;
    public Text HpText;

    public void SetHp(int presentHp, int maxHp)
    {
        FilledHpImg.fillAmount = presentHp * 1f / maxHp;
        HpText.text = $"{presentHp}/{maxHp}";
    }

}
