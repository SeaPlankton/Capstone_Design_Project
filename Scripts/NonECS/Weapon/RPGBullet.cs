using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class RPGBullet : MonoBehaviour
{
    // �ݳ��ϱ� ���� ������Ʈ Ǯ ���۷���
    public IObjectPool<GameObject> BulletPool;
    public GameObject BulletMesh;
    public GameObject Trail;
    public Explosion Explosion;
    public RPGAOE RPGAOE;
    
    // �Ѿ˼ӵ�
    public float Speed = 10f;
    // �Ѿ˵�����
    public float Damage = 0;
    // �Ѿ� �����ֱ�
    public float LifeTime = 4f;

    // ���� �ٶ󺸴� ����
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
    // �Ѿ��� ���� �ٶ󺸱⿡ ���� ����
    // ������ �ܼ��� ���� �ٶ󺸴� �������� �Ѿ��� ������ ����� ��ũ��Ʈ��.
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
        // ������ �Ѿ��� �ڱ�ġ�� �� ����
        float y = rb.position.y;
        rb.position += transform.forward * Speed * Time.deltaTime;
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf || isExploded || isGameStop) return;
        if (other.transform.CompareTag("Enemy"))
        {
            isExploded = true;
            
            // �޽� ����
            BulletMesh.SetActive(false);

            Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.RPGBoom);
            Explosion.gameObject.SetActive(true);
            RPGAOE.gameObject.SetActive(true);
        }
        else if (other.transform.CompareTag("Boss"))
        {
            if (!other.gameObject.activeSelf) return;

            // �ǰ� ����
            isExploded = true;

            // �޽� ����
            BulletMesh.SetActive(false);

            Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.RPGBoom);
            Explosion.gameObject.SetActive(true);
            RPGAOE.gameObject.SetActive(true);
        }
    }
}
