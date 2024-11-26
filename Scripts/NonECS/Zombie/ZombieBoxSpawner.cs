using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ZombieChaseAI;
/// <summary>
/// 박스형 형태의 스포너
/// 반드시 딸려있는 mesh가 plane이어야 한다.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class ZombieBoxSpawner : MonoBehaviour
{
    [Header("소환 영역")]
    public MeshFilter MeshFilter;
    // 4개의 가장 모서리
    [SerializeField]
    private Vector3 bottomLeft;
    [SerializeField]
    private Vector3 bottomRight;
    [SerializeField]
    private Vector3 topLeft;
    [SerializeField]
    private Vector3 topRight;
    [Header("소환 높이")]
    public float SpawnHeight = 0f;

    //좀비 속도 줄이는 액세서리
    private bool _isSlow = false;
    //좀비 속도들
    private float _normalSpeed = 2.5f;
    private float _tankSpeed = 1.5f;
    void Awake()
    {
        SetVertices();
    }
    /// <summary>
    /// 좀비 소환
    /// </summary>
    /// <param name="zombieIndex">소환할 좀비 번호</param>
    /// <param name="zombieCount">소환할 좀비 수</param>
    public void SpawnZombie(int zombieIndex, int zombieCount)
    {
        for (int i = 0; i < zombieCount; i++) { SpawnZombie(zombieIndex); }
    }
    /// <summary>
    /// 좀비 소환
    /// </summary>
    /// <param name="zombieIndex">소환할 좀비 번호</param>
    public void SpawnZombie(int zombieIndex)
    {
        // 1. 오브젝트 풀에서 좀비 오브젝트 대출
        GameObject gameObject = Managers.Instance.ObjectPool.ZombiePool.Get();

        if(zombieIndex == 2)
        {
            gameObject.layer = 10;
        }
        //탱커 좀비일 경우 크기 3으로 조정
        else if(zombieIndex == 3)
        {
            gameObject.transform.localScale = new Vector3(3.4f, 3.4f, 3.4f);
        }

        // #2. 트랜스폼 설정하기
        gameObject.transform.position = GetZombieTransform();
        // #3. 스탯 불러오기 및 저장
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
        // #4. 초기 작업, 스크립트가 알아서 진행
        zombie.Init(zombieStats);
    }
    private Vector3 GetZombieTransform()
    {
        int side = Random.Range(0, 4);
        Vector3 ret = topLeft;
        switch (side)
        {
            // 위쪽, tr과 tl 사이, x축만 사이로 랜덤
            case 0:
                ret = new Vector3(
                    x: Random.Range(topLeft.x, topRight.x),
                    y: SpawnHeight,
                    z: topLeft.z);
                break;
            // 오른쪽, tr과 br 사이, z축만 사이로 랜덤
            case 1:
                ret = new Vector3(
                    x: topRight.x,
                    y: SpawnHeight,
                    z: Random.Range(topRight.z, bottomRight.z));
                break;
            // 아래쪽, bl과 br 사이, x축만 사이로 랜덤
            case 2:
                ret = new Vector3(
                    x: Random.Range(bottomLeft.x, bottomRight.x),
                    y: SpawnHeight,
                    z: bottomLeft.z);
                break;
            // 왼쪽, tl과 b1 사이, z축만 사이로 랜덤
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
        // 모서리 지정 코드
        Vector3[] localVertices = MeshFilter.mesh.vertices;

        /*
         *  Plane의 버텍스 형태는 다음과 같다.
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

