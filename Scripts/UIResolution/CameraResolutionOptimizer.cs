using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraResolutionOptimizer : MonoBehaviour
{
    public UIArea UIArea;

    private Camera cam;
    private Rect _originalRect;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        _originalRect = cam.rect;
        Rect rect = cam.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로비 / 세로비)       
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        cam.rect = rect;
    }
    private void Start()
    {
        //해상도 설정 함수 이벤트 등록
        Managers.Instance.Game.OptionPresenter.OnChangeUIAreaResolution += ResetCameraRect;
        //UIArea.SetUIArea(cam);
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game.OptionPresenter == null) return;
        Managers.Instance.Game.OptionPresenter.OnChangeUIAreaResolution -= ResetCameraRect;
    }

    //옵션에서 해상도 설정 변경시 카메라 Rect 다시 설정1
    private void ResetCameraRect()
    {
        Rect rect = _originalRect;

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)Managers.Instance.Game.OptionPresenter.xRatio / Managers.Instance.Game.OptionPresenter.yRatio); // (가로비 / 세로비)
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        cam.rect = rect;
        UIArea.SetUIArea(cam);
    }

    private void OnEnable()
    {
#if !UNITY_EDITOR
        RenderPipelineManager.beginCameraRendering += RenderPipelineManager_endCameraRendering;
#endif
    }

    private void OnDisable()
    {
#if !UNITY_EDITOR
        RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_endCameraRendering;
#endif
    }

    //URP 모드에서 화면 잔상 지우기
    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)

    {
        GL.Clear(true, true, Color.black);
    }

    /// <summary>
    /// 카메라 외부 영역의 화면을 검게 칠함
    /// </summary>
    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }
}
