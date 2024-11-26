using Cinemachine;
using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class CinemachineController : MonoBehaviour
{
    //플레이어 뒷 모습 게임 화면 카메라
    public CinemachineVirtualCamera GameCamera;
    //아이템 메뉴 클릭했을 때 보여줄 카메라
    public CinemachineVirtualCamera ItemMenuCamera;
    //감정표현 메뉴 클릭했을 때 보여줄 카메라
    public CinemachineVirtualCamera EmotionMenuCamera;

    //게임 카메라 컴포넌트
    private CinemachineComponentBase GameCameraComponentBase;
    private CinemachineBasicMultiChannelPerlin Perlin;
    [SerializeField]
    private GameObject MainCameraTarget;
    [SerializeField]
    private GameObject MenuCameraTarget;

    // 최대 카메라 사거리
    private readonly float MAX_CAMERA_DISTANCE = 10F;
    // 최소 카메라 사거리
    private readonly float MIN_CAMERA_DISTANCE = 1F;

    private void Awake()
    {
        Managers.Instance.Game.OnGridSceneLoaded += Init;
        GameCameraComponentBase = GameCamera.GetCinemachineComponent<CinemachineComponentBase>();
        Perlin = GameCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Perlin.m_AmplitudeGain = 0f;
        Perlin.m_FrequencyGain = 0f;
    }
    private void Init()
    {
        Managers.Instance.Game.CinemachineController = this;
    }

    private void OnDestroy()
    {
        if (Managers.Instance.Game == null) return;
        Managers.Instance.Game.OnGridSceneLoaded -= Init;
    }
    public void ScrollMainCamera(float scroll)
    {
        if (GameCameraComponentBase is Cinemachine3rdPersonFollow cinemachine3RdPersonFollow)
        {
            float distance = cinemachine3RdPersonFollow.CameraDistance + scroll;
            print($"distance : {distance}");
            cinemachine3RdPersonFollow.CameraDistance = Mathf.Clamp(distance, MIN_CAMERA_DISTANCE, MAX_CAMERA_DISTANCE);
        }
    }

    public void ShakeMainCamera(float time)
    {
        StartCoroutine(DoShake(time));
    }
    public void StartShakeMainCamera()
    {
        Perlin.m_AmplitudeGain = 1f;
        Perlin.m_FrequencyGain = 1f;
    }
    public void StopShakeMainCamera()
    {
        Perlin.m_AmplitudeGain = 0f;
        Perlin.m_FrequencyGain = 0f;
    }
    IEnumerator DoShake(float time)
    {
        float timer = 0f;
        Perlin.m_AmplitudeGain = 1f;
        Perlin.m_FrequencyGain = 1f;
        while (true)
        {
            yield return new WaitUntil(() => !Managers.Instance.TimeManager.IsPause);
            if (timer > time) break;
            timer += Time.deltaTime;
            Debug.Log($"timer : {timer}");
            yield return null;
        }
        Perlin.m_AmplitudeGain = 0f;
        Perlin.m_FrequencyGain = 0f;
        Debug.Log("!!!");
    }

    public void MainCamera(bool cameraOn)
    {
        GameCamera.gameObject.SetActive(cameraOn);
        /*
        if (!cameraOn)
        {
            MainCameraTarget.gameObject.transform.position =
                new Vector3(MainCameraTarget.gameObject.transform.position.x,
                MainCameraTarget.gameObject.transform.position.y, 0.3f);
            GameCamera.gameObject.SetActive(cameraOn);
        }
        else if (cameraOn)
        {
            GameCamera.gameObject.SetActive(cameraOn);
            await UniTask.Delay(200);
            MainCameraTarget.gameObject.transform.position =
                new Vector3(MainCameraTarget.gameObject.transform.position.x,
                MainCameraTarget.gameObject.transform.position.y, 0f);
        }
        */
    }

    public void ItemCamera(bool cameraOn)
    {
        ItemMenuCamera.gameObject.SetActive(cameraOn);
    }

    public void EmotionCamera(bool cameraOn)
    {
        EmotionMenuCamera.gameObject.SetActive(cameraOn);
    }
}
