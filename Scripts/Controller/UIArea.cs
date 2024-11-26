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

    //UIArea의 크기를 정해진 해상도에 맞게 변경
    public void SetUIArea(Camera cam)
    {
        CamFollowCanvasGO.SetActive(true);
        Vector2 camFollowSizeDelta = CamFollowCanvasGO.GetComponent<RectTransform>().sizeDelta;
        _rectTransform.sizeDelta = camFollowSizeDelta * cam.rect.width;
        Managers.Instance.Game.OptionPresenter.UIAreaRect = _rectTransform.sizeDelta;
        CamFollowCanvasGO.SetActive(false);
    }
}
