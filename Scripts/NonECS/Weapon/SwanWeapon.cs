using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwanWeapon : MonoBehaviour
{
    // 무기 데미지
    public float Damage = 100;
    // 발사 인터벌
    public float FireInterval = 0.2f;
    // 무기마다 다른 총구 위치
    public Transform[] FirePosition;
    // 발사 쿨타임 카운트
    private float fireCount = 0f;
    // 게임 정지 표시자
    private bool isGameStop;

    private void Start()
    {
        //궤도에 돌아가는게 안보이게
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

        // 발사 쿨타임 끝나면 발사
        if (fireCount > FireInterval)
        {
            fireCount = 0f;
            FireKnife();
        }
    }

    //투척무기 발사
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
