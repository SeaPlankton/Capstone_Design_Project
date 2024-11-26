using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���� ���� ��ƼƼ�� �ִ� ��Ȳ���� 
/// �ּ��� �ѹ��� ��ȸ�� ��� �۾��� �������ؾ� �Ѵ�.
/// </summary>
public class ZombieController : MonoBehaviour
{
    // ���� �� ���ٰ� ����
    // ����ִ� ���� �ִ°� �ƴ�, isDead�� ���͸� �ʿ�
    public List<ZombieChaseAI> Zombies;
    // ���� ����� ����, Nullable Value.
    public ZombieChaseAI ClosestZombie;
    // �÷��̾� ���� ���� ī�޶�
    public CinemachineVirtualCamera VirtualCamera;
    // �ʹ� ���� ���� ����� ���� ã���� ������ ����� �߻��ϹǷ� 2�ʸ��� ����
    private float waitRefreshTime;

    //�Ͻ�����, ��� �� ���� �ӵ� �����ϱ����� ����
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
            Debug.LogError("�ó׸ӽ� ���� ī�޶� �Ҵ���� �ʾҽ��ϴ�.");
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
        // ���� ����� ���� ������ų�, ���ٸ� ����
        if (ClosestZombie == null)
        {
            FindClosestZombie();
        }
        else
        {
            // 0.2�ʸ��� ����
            if (waitRefreshTime > 0.2f)
            {
                waitRefreshTime = 0f;
                FindClosestZombie();
            }
        }
    }
    /// <summary>
    /// ���� ����� ���� ã��
    /// ���⼭ sqrDistance�� distance�� ������ �ǹ�. a^2 + b^2
    /// ��Ʈ ������ ��α� ����.
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
                // ����Ʈ ��ǥ�� ȭ�� ���� �ִٸ� �Ÿ� ������ �ݿ��մϴ�.
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
    /// �޴� ������ ������ ���� �ӵ� 0
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

    //���� �����ӵ��� ����
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
