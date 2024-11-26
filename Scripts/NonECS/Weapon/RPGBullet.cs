using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RPGBullet : MonoBehaviour
{
    // 반납하기 위한 오브젝트 풀 레퍼런스
    public IObjectPool<GameObject> BulletPool;
    public GameObject BulletMesh;
    public GameObject Trail;
    public Explosion Explosion;
    public RPGAOE RPGAOE;
    
    // 총알속도
    public float Speed = 10f;
    // 총알데미지
    public float Damage = 0;
    // 총알 생명주기
    public float LifeTime = 4f;

    // 총이 바라보는 방향
    public float FireDirection;

    private Rigidbody rb;
    private float count;
    private bool isExploded;
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
        isExploded = false;
        transform.rotation = Quaternion.Euler(new Vector3(0f, FireDirection, 0f));
    }
    // Update is called once per frame
    void Update()
    {
        if (isExploded || isGameStop) return;
        // 점프시 총알이 솟구치는 거 방지
        float y = rb.position.y;
        rb.position += transform.forward * Speed * Time.deltaTime;
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf || isExploded || isGameStop) return;
        if (other.transform.CompareTag("Enemy"))
        {
            isExploded = true;
            
            // 메쉬 해제
            BulletMesh.SetActive(false);

            Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.RPGBoom);
            Explosion.gameObject.SetActive(true);
            RPGAOE.gameObject.SetActive(true);
        }
        else if (other.transform.CompareTag("Boss"))
        {
            if (!other.gameObject.activeSelf) return;

            // 피격 판정
            isExploded = true;

            // 메쉬 해제
            BulletMesh.SetActive(false);

            Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.RPGBoom);
            Explosion.gameObject.SetActive(true);
            RPGAOE.gameObject.SetActive(true);
        }
    }
}
