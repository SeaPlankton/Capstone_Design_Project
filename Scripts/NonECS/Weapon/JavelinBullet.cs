using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class JavelinBullet : MonoBehaviour
{
    // 반납하기 위한 오브젝트 풀 레퍼런스
    public IObjectPool<GameObject> BulletPool;
    public GameObject Trail;
    // 총알속도
    public float Speed = 10f;
    // 총알데미지
    public float Damage = 0;
    // 총알 생명주기
    public float LifeTime = 2f;

    // 총이 바라보는 방향
    public float FireDirection;

    private Rigidbody rb;
    private float count;
    private bool isGameStop;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Managers.Instance.Game.TimeController.GameStop += OnGameStop;
        Managers.Instance.Game.TimeController.GameResume += OnGameResume;
    }
    public void OnGameStop()
    {
        isGameStop = true;
    }
    public void OnGameResume()
    {
        isGameStop = false;
    }
    // 총알이 위로 바라보기에 생긴 문제
    // 실제론 단순히 총이 바라보는 방향으로 총알이 나가게 설계된 스크립트임.
    public void Init()
    {
        count = 0f;
        transform.rotation = Quaternion.Euler(90f, 0f, -FireDirection);
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameStop) return;
        // 점프시 총알이 솟구치는 거 방지
        float y = rb.position.y;
        rb.position += transform.up * Speed * Time.deltaTime;
        rb.position = new Vector3(rb.position.x, y, rb.position.z);

        count += Time.deltaTime;
        // 트레일 생성 지연
        if (!Trail.activeSelf)
        {
            if (count > 0.1f) Trail.SetActive(true);
        }
        // 총알 생명 다할 경우
        if (count > LifeTime)
        {
            count = 0f;
            // 총알 오브젝트 반납
            BulletPool.Release(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isGameStop) return;
        // 반납되어도 작동되는 버그 수정
        if (!gameObject.activeSelf) return;
        if (collision.transform.CompareTag("Enemy"))
        {
            // 피격 판정
            collision.gameObject.GetComponent<ZombieCombat>().Hit(Damage);
        }
        else if (collision.transform.CompareTag("Boss"))
        {
            if (!collision.gameObject.activeSelf) return;

            // 피격 판정
            collision.gameObject.GetComponent<BossHitBox>().Hit(Damage);
        }
    }
}
