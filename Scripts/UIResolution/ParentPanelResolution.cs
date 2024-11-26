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

    //�ٲ� �ػ󵵿� �°� �θ� �г� ������ ����
    private void Start()
    {
        Managers.Instance.Game.OptionPresenter.OnChangeParentResolution += OnUpdateParentRectSizeDelta;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game.OptionPresenter == null) return;
        Managers.Instance.Game.OptionPresenter.OnChangeParentResolution -= OnUpdateParentRectSizeDelta;
    }

    //�θ� �г� ������ ����
    //1920 / 1000 = 1.92 -> 300 / 1.92 = 156.25
    //_originalRect�� ���� �ȵǼ� ���� -> �ν����Ϳ� �г� ������ ���� �����༭ �ʱ�ȭ���ִ� ���?
    //gameObject False ���ִ� �гε��� ������ �ȵ� UIControl�� ��� �г� �� �Ѵ¹��?
    public void OnUpdateParentRectSizeDelta()
    {
        _rectTransform.sizeDelta = _originalRectTransform;

        float changedRatioX = (float)1920 / Managers.Instance.Game.OptionPresenter.UIAreaRect.x;
        float changedRatioY = (float)1080 / Managers.Instance.Game.OptionPresenter.UIAreaRect.y;

        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x / changedRatioX, _rectTransform.sizeDelta.y / changedRatioY);
    }
}
