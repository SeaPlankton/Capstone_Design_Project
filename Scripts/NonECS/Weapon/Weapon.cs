using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    None,
    Pistol,
    Rifle,
    Shotgun,
    Sniper,
    RPG,
    RevolverShotgun,
    Knife,
    Javelin
}

public class Weapon : MonoBehaviour
{
    [HideInInspector]
    //무기 이름
    public string Name;
    [HideInInspector]
    //무기 번호
    public int Number;
    //무기 레벨
    public int LV = 1;
    [HideInInspector]
    //무기 설명
    public string Description;
    // 무기 종류
    public WeaponType Type;
    // 무기 데미지
    public float Damage = 100;
    //RPG 장판 데미지
    public float RPGAOEDamage;
    // 발사 인터벌
    public float FireInterval = 0.2f;
    // 무기 사거리
    public float FireDistance = 100f;
    // 무기마다 다른 총구 위치
    public Transform[] FirePosition;
    // 총기 화염
    public ParticleSystem MuzzleFlash;
    // 바라보는 방향
    public Vector3 direction;
    // 발사 쿨타임 카운트
    private float fireCount = 0f;
    // 최대 제곱 거리 (사거리)
    private float maxSprDistance;
    // 게임 정지 표시자
    private bool isGameStop;

    //샷건
    private int pelletCount = 8; // 총알의 개수
    private float spreadAngle = 15f; // 퍼지 각도

    //스나이퍼 총알에 슬로우 적용할지
    private bool isSlow = false;

    //각각의 총들 관통 여부
    private bool isPistolPenetrate = false;
    //private bool isSniperPenetrate = false;
    private bool isRiflePenetrate = false;
    private bool isShotgunPenetrate = false;

    //권총, 돌격 소총 총알 2줄기 여부
    private bool isTwoBullets = false;

    //바주카포 폭발범위 증가
    private bool rpgRangeUpgrade = false;

    //스나이퍼 폭발범위
    private bool sniperRangeUpgrade1 = false;
    private bool sniperRangeUpgrade2 = false;

    private void Start()
    {
        if (MuzzleFlash != null)
        {
            var mainModule = MuzzleFlash.main;
            mainModule.duration = FireInterval;
        }
        if (this.Name == "투척용 나이프" || this.Name == "Javelin")
        {
            //궤도에 돌아가는게 안보이게
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
        maxSprDistance = FireDistance * FireDistance;
        Managers.Instance.Game.TimeController.GameStop += OnGameStop;
        Managers.Instance.Game.TimeController.GameResume += OnGameResume;
    }

    public void OnGameStop()
    {
        isGameStop = true;
        if (MuzzleFlash == null)
            return;
        MuzzleFlash.Pause();
    }
    public void OnGameResume()
    {
        isGameStop = false;
        if (MuzzleFlash == null)
            return;
        if (MuzzleFlash.isPaused)
        {
            MuzzleFlash.Play();
        }
    }
    private void Update()
    {
        if (isGameStop) return;

        // 두번 실행되면 안되기 때문에 return문 달기
        if (Managers.Instance.Game.Player.PlayerController.IsShootBoss)
        {
            if (Managers.Instance.Game.ZombieController.ClosestZombie == null)
            {
                // 좀비가 없다면 보스만 공격 시도
                TryFireBoss();
                return;
            } else
            {
                if (Mathf.Pow(Managers.Instance.Game.Boss.BossAI.PlayerDistance,2f) > Managers.Instance.Game.ZombieController.ClosestZombie.SqrDistance)
                {
                    // 좀비가 더 가깝다면 좀비 공격 시도
                    TryFireZombie();
                    return;
                } else
                {
                    // 보스가 더 가깝다면 보스 공격 시도
                    TryFireBoss();
                    return;
                }
            }
        } else
        {
            TryFireZombie();
        }
    }

    /// <summary>
    /// 좀비 사격 시도
    /// </summary>
    public void TryFireZombie()
    {
        fireCount += Time.deltaTime;
        if (Managers.Instance.Game.ZombieController.ClosestZombie == null) return;
        // 가장 가까운 적을 바라봄
        LookClosestEnemy();
        direction = Managers.Instance.Game.ZombieController.ClosestZombie.transform.position;
        direction = new Vector3(direction.x - transform.position.x, direction.y, direction.z - transform.position.z);
        Debug.DrawRay(transform.position, direction, Color.blue);      
        // 발사 쿨타임 끝나면 발사, 그리고 가장 가까운 좀비가 거리에 들어오면 사격.
        float sqrDistanceToEnemy = Managers.Instance.Game.ZombieController.ClosestZombie.SqrDistance;

        if (fireCount > FireInterval && sqrDistanceToEnemy < maxSprDistance)
        {
            fireCount = 0f;
            Fire();
            if (MuzzleFlash != null)
                MuzzleFlash.Play();
        }
    }
    /// <summary>
    /// 보스 사격 시도
    /// </summary>
    public void TryFireBoss()
    {
        if (Managers.Instance.Game.isVictory) return;
        if (!Managers.Instance.Game.Boss.gameObject.activeSelf) return;
        direction = Managers.Instance.Game.Boss.transform.position;
        if (direction.y > 10f) return;
        LookBoss();
        direction = new Vector3(direction.x - transform.position.x, direction.y, direction.z - transform.position.z);
        Debug.DrawRay(transform.position, direction, Color.blue);
        fireCount += Time.deltaTime;
        // 발사 쿨타임 끝나면 발사, 그리고 가장 가까운 좀비가 거리에 들어오면 사격.
        float sqrDistanceToEnemy = Mathf.Pow(Managers.Instance.Game.Boss.BossAI.PlayerDistance, 2);

        if (fireCount > FireInterval && sqrDistanceToEnemy < maxSprDistance)
        {
            fireCount = 0f;
            Fire();
            if (MuzzleFlash != null)
                MuzzleFlash.Play();
        }
    }
    public void Fire()
    {
        switch (Type)
        {
            case WeaponType.Pistol:
                FirePistol();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Pistol);
                break;
            case WeaponType.Rifle:
                FireRifle();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Rifle);
                break;
            case WeaponType.Shotgun:
                FireShotgun();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Shotgun);
                break;
            case WeaponType.Sniper:
                FireSniper();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Sniper);
                break;
            case WeaponType.RPG:
                FireRPG();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.RPG);
                break;
            case WeaponType.RevolverShotgun:
                FireRevolver();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Shotgun);
                break;
            case WeaponType.Knife:
                FireKnife();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Knife);
                break;
            case WeaponType.Javelin:
                FireJavelin();
                Managers.Instance.Game.GameAudio.PlaySfx(GameAudio.Sfx.Javelin);
                break;
        }
    }

    private void FirePistol()
    {
        //총알이 2줄기일 경우 한개 더 생성
        if (isTwoBullets)
        {
            GameObject bullet1 = Managers.Instance.ObjectPool.PistolBulletPool.Get();
            GameObject bullet2 = Managers.Instance.ObjectPool.PistolBulletPool.Get();
            bullet1.transform.position = FirePosition[1].position;
            bullet2.transform.position = FirePosition[2].position;
            PistolBullet bulletComponent1 = bullet1.GetComponent<PistolBullet>();
            PistolBullet bulletComponent2 = bullet2.GetComponent<PistolBullet>();
            bulletComponent1.Damage = Damage;
            bulletComponent2.Damage = Damage;
            bulletComponent1.FireDirection = transform.rotation.eulerAngles.y;
            bulletComponent2.FireDirection = transform.rotation.eulerAngles.y;
            //총알에 관통효과 적용
            bulletComponent1.isPenetration = isPistolPenetrate;
            bulletComponent2.isPenetration = isPistolPenetrate;
            bulletComponent1.Init();
            bulletComponent2.Init();
        }
        else
        {
            GameObject bullet = Managers.Instance.ObjectPool.PistolBulletPool.Get();
            bullet.transform.position = FirePosition[0].position;
            PistolBullet bulletComponent = bullet.GetComponent<PistolBullet>();
            bulletComponent.Damage = Damage;
            bulletComponent.FireDirection = transform.rotation.eulerAngles.y;
            //총알에 관통효과 적용
            bulletComponent.isPenetration = isPistolPenetrate;
            bulletComponent.Init();
        }
    }

    private void FireRifle()
    {
        //총알이 2줄기일 경우 한개 더 생성
        if (isTwoBullets)
        {
            GameObject bullet1 = Managers.Instance.ObjectPool.RifleBulletPool.Get();
            GameObject bullet2 = Managers.Instance.ObjectPool.RifleBulletPool.Get();
            bullet1.transform.position = FirePosition[1].position;
            bullet2.transform.position = FirePosition[2].position;
            RifleBullet bulletComponent1 = bullet1.GetComponent<RifleBullet>();
            RifleBullet bulletComponent2 = bullet2.GetComponent<RifleBullet>();
            bulletComponent1.Damage = Damage;
            bulletComponent2.Damage = Damage;
            bulletComponent1.FireDirection = transform.rotation.eulerAngles.y;
            bulletComponent2.FireDirection = transform.rotation.eulerAngles.y;
            //총알에 관통효과 적용
            bulletComponent1.isPenetration = isRiflePenetrate;
            bulletComponent2.isPenetration = isRiflePenetrate;
            bulletComponent1.Init();
            bulletComponent2.Init();
        }
        else
        {
            GameObject bullet = Managers.Instance.ObjectPool.RifleBulletPool.Get();
            bullet.transform.position = FirePosition[0].position;
            RifleBullet bulletComponent = bullet.GetComponent<RifleBullet>();
            bulletComponent.Damage = Damage;
            bulletComponent.FireDirection = transform.rotation.eulerAngles.y;
            //총알에 관통효과 적용
            bulletComponent.isPenetration = isRiflePenetrate;
            bulletComponent.Init();
        }
    }

    private void FireShotgun()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            GameObject bullet = Managers.Instance.ObjectPool.ShotgunBulletPool.Get();
            bullet.transform.position = FirePosition[0].position;
            ShotgunBullet bulletComponent = bullet.GetComponent<ShotgunBullet>();
            bulletComponent.Damage = Damage;
            float angle = Random.Range(-spreadAngle, spreadAngle);
            bulletComponent.FireDirection = transform.rotation.eulerAngles.y + angle;
            //총알에 관통효과 적용
            bulletComponent.isPenetration = isShotgunPenetrate;
            bulletComponent.Init();
        }
    }

    private void FireSniper()
    {
        GameObject bullet = Managers.Instance.ObjectPool.SniperBulletPool.Get();
        bullet.transform.position = FirePosition[0].position;
        SniperBullet bulletComponent = bullet.GetComponent<SniperBullet>();
        bulletComponent.Damage = Damage;
        bulletComponent.FireDirection = transform.rotation.eulerAngles.y;
        //총알에 슬로우효과 적용
        if (isSlow)
        {
            bulletComponent.isSlow = true;
        }

        //총알 피격범위 증가
        if (sniperRangeUpgrade1)
        {
            bulletComponent.boxCollider.size = new Vector3(
                0.15f,
                bulletComponent.boxCollider.size.y,
                bulletComponent.boxCollider.size.z
                );
        }
        else if (sniperRangeUpgrade2)
        {
            bulletComponent.boxCollider.size = new Vector3(
                0.5f,
                bulletComponent.boxCollider.size.y,
                bulletComponent.boxCollider.size.z
                );
        }
        //총알에 관통효과 적용
        //bulletComponent.isPenetration = isSniperPenetrate;
        bulletComponent.Init();
    }

    private void FireRPG()
    {
        GameObject bullet = Managers.Instance.ObjectPool.RPGBulletPool.Get();
        bullet.transform.position = FirePosition[0].position;
        RPGBullet bulletComponent = bullet.GetComponent<RPGBullet>();
        bulletComponent.Damage = Damage;
        bulletComponent.RPGAOE.Damage = RPGAOEDamage;
        bulletComponent.FireDirection = transform.rotation.eulerAngles.y;

        //폭발범위 증가 액세 먹었을 경우
        if (rpgRangeUpgrade)
        {
            bulletComponent.Explosion.transform.localScale = new Vector3(
                0.26f, 0.26f, 0.26f
            );
            bulletComponent.RPGAOE.transform.localScale = new Vector3(
                0.2f, 0.2f, 0.2f
                );
        }

        bulletComponent.Init();
    }

    private void FireRevolver()
    {
        for (int i = 0; i < pelletCount; i++)
        {
            GameObject bullet = Managers.Instance.ObjectPool.ShotgunBulletPool.Get();
            bullet.transform.position = FirePosition[0].position;
            ShotgunBullet bulletComponent = bullet.GetComponent<ShotgunBullet>();
            bulletComponent.Damage = Damage;
            float angle = Random.Range(-spreadAngle, spreadAngle);
            bulletComponent.FireDirection = transform.rotation.eulerAngles.y + angle;
            //총알에 관통효과 적용
            bulletComponent.isPenetration = isPistolPenetrate;
            bulletComponent.Init();
        }
    }

    private void FireKnife()
    {
        GameObject bullet = Managers.Instance.ObjectPool.KnifeBulletPool.Get();
        bullet.transform.position = FirePosition[0].position;
        KnifeBullet bulletComponent = bullet.GetComponent<KnifeBullet>();
        bulletComponent.Damage = Damage;
        bulletComponent.FireDirection = transform.rotation.eulerAngles.y;
        bulletComponent.Init();
    }

    private void FireJavelin()
    {
        GameObject bullet = Managers.Instance.ObjectPool.JavelinBulletPool.Get();
        bullet.transform.position = FirePosition[0].position;
        JavelinBullet bulletComponent = bullet.GetComponent<JavelinBullet>();
        bulletComponent.Damage = Damage;
        bulletComponent.FireDirection = transform.rotation.eulerAngles.y;
        bulletComponent.Init();
    }

    /// <summary>
    /// 좀비 컨트롤러가 잡아준 가장 가까운 좀비의 포지션과 현 포지션을 비교하여 y축 거리를 제외하고 평면 상에서 바라보게 한다.
    /// </summary>
    public void LookClosestEnemy()
    {
        Vector3 direction = Managers.Instance.Game.ZombieController.ClosestZombie.transform.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
    public void LookBoss()
    {
        Vector3 direction = Managers.Instance.Game.Boss.transform.position - transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }
    //무기 레벨업 시 LV증가, 데미지 설정, 최대레벨 달성 시 배너에 안뜨게 요청
    private void SetWeaponBalance(int lv)
    {
        switch (Type)
        {
            case WeaponType.Pistol:
                if (lv == 2)
                {
                    WeaponDamageUp(1);
                    SetFireInterval(1.15f);
                }
                else if (lv == 3)
                {
                    WeaponDamageUp(2);
                    SetFireInterval(1.2f);
                }
                else if (lv == 4)
                {
                    WeaponDamageUp(1);
                    SetFireInterval(1.35f);
                }
                else if (lv == 5)
                {
                    WeaponDamageUp(2);
                    SetFireInterval(1.4f);
                }
                else if (lv == 6)
                {
                    WeaponDamageUp(2);
                    SetFireInterval(1.5f);
                }
                else if (lv == 7)
                {
                    Managers.Instance.Game.WeaponController.CheckMaxLevel(Number);
                    //리볼버형 샷건화 추가
                    Managers.Instance.Game.WeaponController.CreateReloverShotgun();
                }
                break;
            case WeaponType.Rifle:
                if (lv == 2)
                {
                    WeaponDamageUp(2);
                    SetFireInterval(1.04f);
                }
                else if (lv == 3)
                {
                    WeaponDamageUp(2);
                    SetFireInterval(1.09f);
                }
                else if (lv == 4)
                {
                    WeaponDamageUp(2);
                    SetFireInterval(1.16f);
                }
                else if (lv == 5)
                {
                    WeaponDamageUp(1);
                    SetFireInterval(1.25f);
                }
                else if (lv == 6)
                {
                    WeaponDamageUp(1);
                    SetFireInterval(1.36f);
                }
                else if (lv == 7)
                {
                    Managers.Instance.Game.WeaponController.CheckMaxLevel(Number);
                }
                break;
            case WeaponType.Shotgun:
                if (lv == 2)
                {
                    WeaponDamageUp(1);
                }
                else if (lv == 3)
                {
                    WeaponDamageUp(2);
                }
                else if (lv == 4)
                {
                    WeaponDamageUp(1);
                }
                else if (lv == 5)
                {
                    WeaponDamageUp(2);
                }
                else if (lv == 6)
                {
                    WeaponDamageUp(2);
                }
                else if (lv == 7)
                {
                    SetFireInterval(2.5f);
                    Managers.Instance.Game.WeaponController.CheckMaxLevel(Number);
                }
                break;
            case WeaponType.Sniper:
                if (lv == 2)
                {
                    WeaponDamageUp(1);
                    SetFireInterval(1.2f);
                }
                else if (lv == 3)
                {
                    WeaponDamageUp(3);
                    SetFireInterval(1.3f);
                }
                else if (lv == 4)
                {
                    WeaponDamageUp(1);
                    SetFireInterval(1.5f);
                }
                else if (lv == 5)
                {
                    WeaponDamageUp(3);
                    SetFireInterval(1.6f);
                }
                else if (lv == 6)
                {
                    WeaponDamageUp(1);
                    SetFireInterval(1.85f);
                }
                else if (lv == 7)
                {
                    Managers.Instance.Game.WeaponController.CheckMaxLevel(Number);
                    SetFireInterval(2.5f);
                }
                break;
            case WeaponType.RPG:
                if (lv == 2)
                {
                    RPGAOEDamage = 20f;
                    SetFireInterval(1.1f);
                }
                else if (lv == 3)
                {
                    RPGAOEDamage = 25f;
                    SetFireInterval(1.15f);
                }
                else if (lv == 4)
                {
                    RPGAOEDamage = 30f;
                    SetFireInterval(1.2f);
                }
                else if (lv == 5)
                {
                    RPGAOEDamage = 35f;
                    SetFireInterval(1.22f);
                }
                else if (lv == 6)
                {
                    RPGAOEDamage = 40f;
                    SetFireInterval(1.24f);
                }
                else if (lv == 7)
                {
                    RPGAOEDamage = 45f;
                    SetFireInterval(1.28f);
                }
                else if (lv == 8)
                {
                    RPGAOEDamage = 50f;
                    SetFireInterval(1.35f);
                }
                else if (lv == 9)
                {
                    WeaponDamageUp(10);
                    RPGAOEDamage = 70f;
                    SetFireInterval(1.5f);
                    Managers.Instance.Game.WeaponController.CheckMaxLevel(Number);
                }
                break;
        }
    }

    public void WeaponLevelUp(int lv, bool isTextBook)
    {
        //1레벨은 계산해 줄 필요 없으므로 return
        if (lv == 1)
            return;

        int CheckLV = 1;

        //전술교본이 있으면 해당 레벨이 될 때까지 능력치 적용
        if (isTextBook)
        {
            while (CheckLV < lv)
            {
                CheckLV++;
                SetWeaponBalance(CheckLV);
            }
        }
        //전술교본 없으면 해당 레벨에 대해서만 적용
        else
        {
            SetWeaponBalance(lv);
        }
    }

    //액세서리로 무기 데미지 증가
    public void WeaponDamageUp(float damage)
    {
        Damage += damage;

        if (Damage <= 0)
            Damage = 1;
    }

    //무기 데미지 2배 증가
    public void WeaponDamageDouble()
    {
        Damage *= 2;
    }

    //무기 데미지 %로 증가
    public void WeaponDamagePercent(float percent)
    {
        //10% = 100의 10퍼면 = 110
        float damage = percent / 100;
        damage = Damage * damage;
        Damage += damage;
    }

    //연사속도 조정
    public void SetFireInterval(float figure)
    {
        float fireTime = 60f / FireInterval;
        fireTime = fireTime * figure;

        FireInterval = 60f / fireTime;
    }

    //스나이퍼 범위 증가 1
    public void SetSniperRangeUpgrade1()
    {
        sniperRangeUpgrade1 = true;
    }

    //스나이퍼 범위 증가 2
    public void SetSniperRangeUpgrade2()
    {
        sniperRangeUpgrade1 = false;
        sniperRangeUpgrade2 = true;
    }

    //스나이퍼 총알에 맞은 적들 슬로우 적용
    public void SetSlowSniperBullet()
    {
        isSlow = true;
    }

    //권총 총알 관통할지
    public void SetPistolPenetration(bool penetrate)
    {
        isPistolPenetrate = penetrate;
    }
    //스나이퍼 총알 관통할지
    public void SetSniperPenetration(bool penetrate)
    {
        //isSniperPenetrate = penetrate;
    }
    //돌격소총 총알 관통할지
    public void SetRiflePenetration(bool penetrate)
    {
        isRiflePenetrate = penetrate;
    }
    //샷건 총알 관통할지
    public void SetShotgunPenetration(bool penetrate)
    {
        isShotgunPenetrate = penetrate;
    }

    //권총, 돌격 소총 총알 2줄기로 나가게
    public void SetTwoBullets()
    {
        isTwoBullets = true;
    }

    //샷건 총알 개수 1/3, 탄 퍼짐 각도 줄여줌
    public void SetShotgunChoke()
    {
        pelletCount /= 3;
        spreadAngle -= 10f;
    }

    //샷건 탄 퍼짐 각도 10도 증가
    public void SetShotgunDonald()
    {
        spreadAngle += 10f;
    }

    //RPG 폭발범위 증가
    public void SetRpgRangeUpgrade()
    {
        rpgRangeUpgrade = true;
    }
}
