using UnityEngine;
using UnityEngine.UI;

public class BossHpBarView : MonoBehaviour
{
    public Image FilledHpImg;
    public Text HpText;

    public void SetHp(int presentHp, int maxHp)
    {
        FilledHpImg.fillAmount = presentHp * 1f / maxHp;
        HpText.text = $"{presentHp}/{maxHp}";
    }
}
