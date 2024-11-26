using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ZombieChaseAI;
/// <summary>
/// �ڽ��� ������ ������
/// �ݵ�� �����ִ� mesh�� plane�̾�� �Ѵ�.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class ZombieBoxSpawner : MonoBehaviour
{
    [Header("��ȯ ����")]
    public MeshFilter MeshFilter;
    // 4���� ���� �𼭸�
    [SerializeField]
    private Vector3 bottomLeft;
    [SerializeField]
    private Vector3 bottomRight;
    [SerializeField]
    private Vector3 topLeft;
    [SerializeField]
    private Vector3 topRight;
    [Header("��ȯ ����")]
    public float SpawnHeight = 0f;

    //���� �ӵ� ���̴� �׼�����
    private bool _isSlow = false;
    //���� �ӵ���
    private float _normalSpeed = 2.5f;
    private float _tankSpeed = 1.5f;
    void Awake()
    {
        SetVertices();
    }
    /// <summary>
    /// ���� ��ȯ
    /// </summary>
    /// <param name="zombieIndex">��ȯ�� ���� ��ȣ</param>
    /// <param name="zombieCount">��ȯ�� ���� ��</param>
    public void SpawnZombie(int zombieIndex, int zombieCount)
    {
        for (int i = 0; i < zombieCount; i++) { SpawnZombie(zombieIndex); }
    }
    /// <summary>
    /// ���� ��ȯ
    /// </summary>
    /// <param name="zombieIndex">��ȯ�� ���� ��ȣ</param>
    public void SpawnZombie(int zombieIndex)
    {
        // 1. ������Ʈ Ǯ���� ���� ������Ʈ ����
        GameObject gameObject = Managers.Instance.ObjectPool.ZombiePool.Get();

        if(zombieIndex == 2)
        {
            gameObject.layer = 10;
        }
        //��Ŀ ������ ��� ũ�� 3���� ����
        else if(zombieIndex == 3)
        {
            gameObject.transform.localScale = new Vector3(3.4f, 3.4f, 3.4f);
        }

        // #2. Ʈ������ �����ϱ�
        gameObject.transform.position = GetZombieTransform();
        // #3. ���� �ҷ����� �� ����
        ZombieStats zombieStats = Managers.Instance.Game.ZombieStatsManager.ZombieStatsArray[zombieIndex - 1];

        if (_isSlow)
        {
            if(zombieIndex == 3)
            {
                zombieStats.speed = _tankSpeed;
            }
            else
            {
                zombieStats.speed = _normalSpeed;
            }
        }

        Zombie zombie = gameObject.GetComponent<Zombie>();
        // #4. �ʱ� �۾�, ��ũ��Ʈ�� �˾Ƽ� ����
        zombie.Init(zombieStats);
    }
    private Vector3 GetZombieTransform()
    {
        int side = Random.Range(0, 4);
        Vector3 ret = topLeft;
        switch (side)
        {
            // ����, tr�� tl ����, x�ุ ���̷� ����
            case 0:
                ret = new Vector3(
                    x: Random.Range(topLeft.x, topRight.x),
                    y: SpawnHeight,
                    z: topLeft.z);
                break;
            // ������, tr�� br ����, z�ุ ���̷� ����
            case 1:
                ret = new Vector3(
                    x: topRight.x,
                    y: SpawnHeight,
                    z: Random.Range(topRight.z, bottomRight.z));
                break;
            // �Ʒ���, bl�� br ����, x�ุ ���̷� ����
            case 2:
                ret = new Vector3(
                    x: Random.Range(bottomLeft.x, bottomRight.x),
                    y: SpawnHeight,
                    z: bottomLeft.z);
                break;
            // ����, tl�� b1 ����, z�ุ ���̷� ����
            case 3:
                ret = new Vector3(
                    x: topLeft.x,
                    y: SpawnHeight,
                    z: Random.Range(topLeft.z, bottomLeft.z));
                break;
            default:
                break;
        }
        return ret;
    }

    public void SetVertices()
    {
        // �𼭸� ���� �ڵ�
        Vector3[] localVertices = MeshFilter.mesh.vertices;

        /*
         *  Plane�� ���ؽ� ���´� ������ ����.
         *  (0)     (1)     (2)     ...     (10)
         *  (11)    (12)    (13)    ...     (21)
         *  (22)    (23)    (24)    ...     (32)
         *  ...      ...     ...    ...      ...
         *  (110)   (111)   (112)   ...     (120)
         */
        topLeft = transform.TransformPoint(localVertices[0]);
        topRight = transform.TransformPoint(localVertices[10]);
        bottomLeft = transform.TransformPoint(localVertices[110]);
        bottomRight = transform.TransformPoint(localVertices[120]);
    }

    public void SetZombieSpeedForAccessory()
    {
        _isSlow = true;
        _normalSpeed = 2f;
        _tankSpeed = 1f;
    }
}

