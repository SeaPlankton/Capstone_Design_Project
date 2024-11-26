using UnityEngine;
using UnityEngine.Pool;
/// <summary>
/// �Ѿ� ���� ��ũ��Ʈ
/// �Ѿ˿� �پ ȸ���� �ӷ�, �����ֱ� ��� ����
/// </summary>
public class SniperBullet : MonoBehaviour
{
    // �ݳ��ϱ� ���� ������Ʈ Ǯ ���۷���
    public IObjectPool<GameObject> BulletPool;
    public GameObject Trail;
    public BoxCollider boxCollider;
    // �Ѿ˼ӵ�
    public float Speed = 10f;
    // �Ѿ˵�����
    public float Damage = 0;
    // �Ѿ� �����ֱ�
    public float LifeTime = 4f;

    // ���� �ٶ󺸴� ����
    public float FireDirection;

    //�������� �Ѿ˿� ������ ���ο� �ɸ��� �׼����� �����
    public bool isSlow = false;

    //���� ���� ����
    public bool isPenetration = true;

    private Rigidbody rb;
    private float count;
    private bool isGameStop;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
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
    // �Ѿ��� ���� �ٶ󺸱⿡ ���� ����
    // ������ �ܼ��� ���� �ٶ󺸴� �������� �Ѿ��� ������ ����� ��ũ��Ʈ��.
    public void Init()
    {
        count = 0f;
        transform.rotation = Quaternion.Euler(90f, 0f, -FireDirection);
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameStop) return;
        // ������ �Ѿ��� �ڱ�ġ�� �� ����
        float y = rb.position.y;
        rb.position += transform.up * Speed * Time.deltaTime;
        rb.position = new Vector3(rb.position.x, y, rb.position.z);

        count += Time.deltaTime;
        // Ʈ���� ���� ����
        if (!Trail.activeSelf)
        {
            if (count > 0.1f) Trail.SetActive(true);
        }
        // �Ѿ� ���� ���� ���
        if (count > LifeTime)
        {
            count = 0f;
            // �Ѿ� ������Ʈ �ݳ�
            BulletPool.Release(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isGameStop) return;
        // �ݳ��Ǿ �۵��Ǵ� ���� ����
        if (!gameObject.activeSelf) return;
        if (collision.transform.CompareTag("Enemy"))
        {
            // �ǰ� ����
            collision.gameObject.GetComponent<ZombieCombat>().Hit(Damage);
            //���� ���ο� �׼������� ȹ��������
            if (isSlow)
            {
                collision.gameObject.GetComponent<ZombieChaseAI>().SetZomBieSlow(1f);
            }
            if (!isPenetration)
            {
                // �Ѿ� ������Ʈ �ݳ�
                BulletPool.Release(this.gameObject);
            }
        } 
        else if (collision.transform.CompareTag("Boss"))
        {
            if (!collision.gameObject.activeSelf) return;
            // �ǰ� ����
            collision.gameObject.GetComponent<BossHitBox>().Hit(Damage);
            if (!isPenetration)
            {
                // �Ѿ� ������Ʈ �ݳ�
                BulletPool.Release(this.gameObject);
            }
        }
    }
}
