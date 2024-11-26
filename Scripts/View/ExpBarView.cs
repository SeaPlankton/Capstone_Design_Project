using UnityEngine;
using UnityEngine.UI;

public class ExpBarView : MonoBehaviour
{
    public Image FilledExpImg;
    
    public void SetExp(int presentExp, int maxExp)
    {
        FilledExpImg.fillAmount = presentExp * 1f / maxExp;
    }
}
