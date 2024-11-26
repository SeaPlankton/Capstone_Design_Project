using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArea : MonoBehaviour
{
    public GameObject CamFollowCanvasGO;
    private RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        CamFollowCanvasGO.SetActive(false);
    }

    //UIArea�� ũ�⸦ ������ �ػ󵵿� �°� ����
    public void SetUIArea(Camera cam)
    {
        CamFollowCanvasGO.SetActive(true);
        Vector2 camFollowSizeDelta = CamFollowCanvasGO.GetComponent<RectTransform>().sizeDelta;
        _rectTransform.sizeDelta = camFollowSizeDelta * cam.rect.width;
        Managers.Instance.Game.OptionPresenter.UIAreaRect = _rectTransform.sizeDelta;
        CamFollowCanvasGO.SetActive(false);
    }
}
