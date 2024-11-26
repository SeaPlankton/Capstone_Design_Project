using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 많은 좀비 엔티티가 있는 상황에서 
/// 최소한 한번의 순회로 모든 작업을 마무리해야 한다.
/// </summary>
public class ZombieController : MonoBehaviour
{
    // ㄹㅇ 개 많다고 가정
    // 살아있는 좀비만 있는게 아님, isDead로 필터링 필요
    public List<ZombieChaseAI> Zombies;
    // 가장 가까운 좀비, Nullable Value.
    public ZombieChaseAI ClosestZombie;
    // 플레이어 추적 가상 카메라
    public CinemachineVirtualCamera VirtualCamera;
    // 너무 자주 가장 가까운 좀비를 찾으면 프레임 드랍이 발생하므로 2초마다 갱신
    private float waitRefreshTime;

    //일시정지, 재생 시 좀비 속도 세팅하기위한 변수
    private bool isGameStop;
    private Camera mainCamera;

    public void OnGameStop()
    {
        isGameStop = true;
    }
    public void OnGameResume()
    {
        isGameStop = false;
    }

    private void Awake()
    {
        waitRefreshTime = 0f;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found.");
        }
        if (VirtualCamera == null)
        {
            Debug.LogError("시네머신 가상 카메라가 할당되지 않았습니다.");
            return;
        }
        Managers.Instance.Game.TimeController.GameStop += SetZombieSpeedZero;
        Managers.Instance.Game.TimeController.GameResume += SetZombieSpeedBack;
        Managers.Instance.Game.TimeController.GameStop += OnGameStop;
        Managers.Instance.Game.TimeController.GameResume += OnGameResume;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStop) return;
        if (Zombies.Count < 1) return;

        waitRefreshTime += Time.deltaTime;
        // 가장 가까운 좀비가 사라지거나, 없다면 갱신
        if (ClosestZombie == null)
        {
            FindClosestZombie();
        }
        else
        {
            // 0.2초마다 갱신
            if (waitRefreshTime > 0.2f)
            {
                waitRefreshTime = 0f;
                FindClosestZombie();
            }
        }
    }
    /// <summary>
    /// 가장 가까운 좀비 찾기
    /// 여기서 sqrDistance는 distance의 제곱을 의미. a^2 + b^2
    /// 루트 연산은 비싸기 때문.
    /// </summary>
    void FindClosestZombie()
    {
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < Zombies.Count; i++)
        {
            if (Zombies[i] != null)
            {
                if (Zombies[i].IsDead)
                {
                    continue;
                }
                Vector3 viewportPoint = mainCamera.WorldToViewportPoint(Zombies[i].Zombie.BoxCollider.transform.position);
                // 뷰포트 좌표가 화면 내에 있다면 거리 측정에 반영합니다.
                if (viewportPoint.z > 0 && viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
                {
                    if (Zombies[i].SqrDistance < minDistance)
                    {
                        if (ClosestZombie != null)
                        {
                            ClosestZombie.IsClosest = false;
                        }
                        minDistance = Zombies[i].SqrDistance;
                        ClosestZombie = Zombies[i];
                        Zombies[i].IsClosest = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 메뉴 열리고 닫히면 좀비 속도 0
    /// </summary>
    public void SetZombieSpeedZero()
    {
        for (int i = 0; i < Zombies.Count; i++)
        {
            Zombies[i].PreviousSpeed = Zombies[i].Speed;
            if (Zombies[i].Zombie.Rigidbody != null)
            {
                Zombies[i].Zombie.Rigidbody.velocity = Vector3.zero;
                Zombies[i].Zombie.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
            Zombies[i].Speed = 0f;
            Zombies[i].Zombie.ZombieChaseAI.IsGameStop = true;
        }
    }

    //좀비 원래속도로 변경
    private void SetZombieSpeedBack()
    {
        for (int i = 0; i < Zombies.Count; i++)
        {
            if (Zombies[i].Zombie.Rigidbody != null)
            {
                Zombies[i].Zombie.Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            }
            Zombies[i].Speed = Zombies[i].PreviousSpeed;
            Zombies[i].Zombie.ZombieChaseAI.IsGameStop = false;
        }
    }

    public void SetZombieSpeedForAccessory()
    {
        for(int i = 0; i < Zombies.Count; i++)
        {
            if(Zombies[i].ZombieID == 3)
            {
                Zombies[i].Speed = 1f;
            }
            else
            {
                Zombies[i].Speed = 2f;
            }
        }
    }

}
