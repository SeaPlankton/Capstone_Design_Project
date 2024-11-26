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
    //���� �̸�
    public string Name;
    [HideInInspector]
    //���� ��ȣ
    public int Number;
    //���� ����
    public int LV = 1;
    [HideInInspector]
    //���� ����
    public string Description;
    // ���� ����
    public WeaponType Type;
    // ���� ������
    public float Damage = 100;
    //RPG ���� ������
    public float RPGAOEDamage;
    // �߻� ���͹�
    public float FireInterval = 0.2f;
    // ���� ��Ÿ�
    public float FireDistance = 100f;
    // ���⸶�� �ٸ� �ѱ� ��ġ
    public Transform[] FirePosition;
    // �ѱ� ȭ��
    public ParticleSystem MuzzleFlash;
    // �ٶ󺸴� ����
    public Vector3 direction;
    // �߻� ��Ÿ�� ī��Ʈ
    private float fireCount = 0f;
    // �ִ� ���� �Ÿ� (��Ÿ�)
    private float maxSprDistance;
    // ���� ���� ǥ����
    private bool isGameStop;

    //����
    private int pelletCount = 8; // �Ѿ��� ����
    private float spreadAngle = 15f; // ���� ����

    //�������� �Ѿ˿� ���ο� ��������
    private bool isSlow = false;

    //������ �ѵ� ���� ����
    private bool isPistolPenetrate = false;
    //private bool isSniperPenetrate = false;
    private bool isRiflePenetrate = false;
    private bool isShotgunPenetrate = false;

    //����, ���� ���� �Ѿ� 2�ٱ� ����
    private bool isTwoBullets = false;

    //����ī�� ���߹��� ����
    private bool rpgRangeUpgrade = false;

    //�������� ���߹���
    private bool sniperRangeUpgrade1 = false;
    private bool sniperRangeUpgrade2 = false;

    private void Start()
    {
        if (MuzzleFlash != null)
        {
            var mainModule = MuzzleFlash.main;
            mainModule.duration = FireInterval;
        }
        if (this.Name == "��ô�� ������" || this.Name == "Javelin")
        {
            //�˵��� ���ư��°� �Ⱥ��̰�
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

        // �ι� ����Ǹ� �ȵǱ� ������ return�� �ޱ�
        if (Managers.Instance.Game.Player.PlayerController.IsShootBoss)
        {
            if (Managers.Instance.Game.ZombieController.ClosestZombie == null)
            {
                // ���� ���ٸ� ������ ���� �õ�
                TryFireBoss();
                return;
            } else
            {
                if (Mathf.Pow(Managers.Instance.Game.Boss.BossAI.PlayerDistance,2f) > Managers.Instance.Game.ZombieController.ClosestZombie.SqrDistance)
                {
                    // ���� �� �����ٸ� ���� ���� �õ�
                    TryFireZombie();
                    return;
                } else
                {
                    // ������ �� �����ٸ� ���� ���� �õ�
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
    /// ���� ��� �õ�
    /// </summary>
    public void TryFireZombie()
    {
        fireCount += Time.deltaTime;
        if (Managers.Instance.Game.ZombieController.ClosestZombie == null) return;
        // ���� ����� ���� �ٶ�
        LookClosestEnemy();
        direction = Managers.Instance.Game.ZombieController.ClosestZombie.transform.position;
        direction = new Vector3(direction.x - transform.position.x, direction.y, direction.z - transform.position.z);
        Debug.DrawRay(transform.position, direction, Color.blue);      
        // �߻� ��Ÿ�� ������ �߻�, �׸��� ���� ����� ���� �Ÿ��� ������ ���.
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
    /// ���� ��� �õ�
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
        // �߻� ��Ÿ�� ������ �߻�, �׸��� ���� ����� ���� �Ÿ��� ������ ���.
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
        //�Ѿ��� 2�ٱ��� ��� �Ѱ� �� ����
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
            //�Ѿ˿� ����ȿ�� ����
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
            //�Ѿ˿� ����ȿ�� ����
            bulletComponent.isPenetration = isPistolPenetrate;
            bulletComponent.Init();
        }
    }

    private void FireRifle()
    {
        //�Ѿ��� 2�ٱ��� ��� �Ѱ� �� ����
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
            //�Ѿ˿� ����ȿ�� ����
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
            //�Ѿ˿� ����ȿ�� ����
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
            //�Ѿ˿� ����ȿ�� ����
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
        //�Ѿ˿� ���ο�ȿ�� ����
        if (isSlow)
        {
            bulletComponent.isSlow = true;
        }

        //�Ѿ� �ǰݹ��� ����
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
        //�Ѿ˿� ����ȿ�� ����
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

        //���߹��� ���� �׼� �Ծ��� ���
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
            //�Ѿ˿� ����ȿ�� ����
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
    /// ���� ��Ʈ�ѷ��� ����� ���� ����� ������ �����ǰ� �� �������� ���Ͽ� y�� �Ÿ��� �����ϰ� ��� �󿡼� �ٶ󺸰� �Ѵ�.
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
    //���� ������ �� LV����, ������ ����, �ִ뷹�� �޼� �� ��ʿ� �ȶ߰� ��û
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
                    //�������� ����ȭ �߰�
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
        //1������ ����� �� �ʿ� �����Ƿ� return
        if (lv == 1)
            return;

        int CheckLV = 1;

        //���������� ������ �ش� ������ �� ������ �ɷ�ġ ����
        if (isTextBook)
        {
            while (CheckLV < lv)
            {
                CheckLV++;
                SetWeaponBalance(CheckLV);
            }
        }
        //�������� ������ �ش� ������ ���ؼ��� ����
        else
        {
            SetWeaponBalance(lv);
        }
    }

    //�׼������� ���� ������ ����
    public void WeaponDamageUp(float damage)
    {
        Damage += damage;

        if (Damage <= 0)
            Damage = 1;
    }

    //���� ������ 2�� ����
    public void WeaponDamageDouble()
    {
        Damage *= 2;
    }

    //���� ������ %�� ����
    public void WeaponDamagePercent(float percent)
    {
        //10% = 100�� 10�۸� = 110
        float damage = percent / 100;
        damage = Damage * damage;
        Damage += damage;
    }

    //����ӵ� ����
    public void SetFireInterval(float figure)
    {
        float fireTime = 60f / FireInterval;
        fireTime = fireTime * figure;

        FireInterval = 60f / fireTime;
    }

    //�������� ���� ���� 1
    public void SetSniperRangeUpgrade1()
    {
        sniperRangeUpgrade1 = true;
    }

    //�������� ���� ���� 2
    public void SetSniperRangeUpgrade2()
    {
        sniperRangeUpgrade1 = false;
        sniperRangeUpgrade2 = true;
    }

    //�������� �Ѿ˿� ���� ���� ���ο� ����
    public void SetSlowSniperBullet()
    {
        isSlow = true;
    }

    //���� �Ѿ� ��������
    public void SetPistolPenetration(bool penetrate)
    {
        isPistolPenetrate = penetrate;
    }
    //�������� �Ѿ� ��������
    public void SetSniperPenetration(bool penetrate)
    {
        //isSniperPenetrate = penetrate;
    }
    //���ݼ��� �Ѿ� ��������
    public void SetRiflePenetration(bool penetrate)
    {
        isRiflePenetrate = penetrate;
    }
    //���� �Ѿ� ��������
    public void SetShotgunPenetration(bool penetrate)
    {
        isShotgunPenetrate = penetrate;
    }

    //����, ���� ���� �Ѿ� 2�ٱ�� ������
    public void SetTwoBullets()
    {
        isTwoBullets = true;
    }

    //���� �Ѿ� ���� 1/3, ź ���� ���� �ٿ���
    public void SetShotgunChoke()
    {
        pelletCount /= 3;
        spreadAngle -= 10f;
    }

    //���� ź ���� ���� 10�� ����
    public void SetShotgunDonald()
    {
        spreadAngle += 10f;
    }

    //RPG ���߹��� ����
    public void SetRpgRangeUpgrade()
    {
        rpgRangeUpgrade = true;
    }
}
