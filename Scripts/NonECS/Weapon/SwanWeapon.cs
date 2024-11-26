using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwanWeapon : MonoBehaviour
{
    // ���� ������
    public float Damage = 100;
    // �߻� ���͹�
    public float FireInterval = 0.2f;
    // ���⸶�� �ٸ� �ѱ� ��ġ
    public Transform[] FirePosition;
    // �߻� ��Ÿ�� ī��Ʈ
    private float fireCount = 0f;
    // ���� ���� ǥ����
    private bool isGameStop;

    private void Start()
    {
        //�˵��� ���ư��°� �Ⱥ��̰�
        this.transform.GetChild(0).gameObject.SetActive(false);
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

    private void Update()
    {
        if (isGameStop) return;

        fireCount += Time.deltaTime;

        // �߻� ��Ÿ�� ������ �߻�
        if (fireCount > FireInterval)
        {
            fireCount = 0f;
            FireKnife();
        }
    }

    //��ô���� �߻�
    private void FireKnife()
    {
        GameObject bullet = Managers.Instance.ObjectPool.KnifeBulletPool.Get();
        bullet.transform.position = FirePosition[0].position;
        KnifeBullet bulletComponent = bullet.GetComponent<KnifeBullet>();
        bulletComponent.Damage = Damage;
        bulletComponent.FireDirection = transform.rotation.eulerAngles.y;
        bulletComponent.Init();
    }
}
