using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPanelResolution : MonoBehaviour
{
    private RectTransform _rectTransform;
    public Vector2 _originalRectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    //바뀐 해상도에 맞게 부모 패널 사이즈 변경
    private void Start()
    {
        Managers.Instance.Game.OptionPresenter.OnChangeParentResolution += OnUpdateParentRectSizeDelta;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game.OptionPresenter == null) return;
        Managers.Instance.Game.OptionPresenter.OnChangeParentResolution -= OnUpdateParentRectSizeDelta;
    }

    //부모 패널 사이즈 변경
    //1920 / 1000 = 1.92 -> 300 / 1.92 = 156.25
    //_originalRect가 저장 안되서 오류 -> 인스펙터에 패널 사이즈 직접 적어줘서 초기화해주는 방식?
    //gameObject False 되있는 패널들은 적용이 안됨 UIControl로 모든 패널 다 켜는방식?
    public void OnUpdateParentRectSizeDelta()
    {
        _rectTransform.sizeDelta = _originalRectTransform;

        float changedRatioX = (float)1920 / Managers.Instance.Game.OptionPresenter.UIAreaRect.x;
        float changedRatioY = (float)1080 / Managers.Instance.Game.OptionPresenter.UIAreaRect.y;

        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x / changedRatioX, _rectTransform.sizeDelta.y / changedRatioY);
    }
}
