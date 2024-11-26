using UnityEngine;
using UnityEngine.Pool;
/// <summary>
/// https://starlightbox.tistory.com/84
/// 오브젝트 풀링 매니저
/// - 유니티 풀링 매니저 활용함
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    // 오브젝트 풀 기본 사이즈
    public int DefaultCapacity_Zombie = 500;
    public int DefaultCapacity_Meteor = 10;
    // 오브젝트 풀 맥스 사이즈
    public int MaxZombiePoolSize = 2000;
    public int MaxMeteorPoolSize = 200;
    // 오브젝트 풀 기본 사이즈 
    public int DefaultCapacity_Bullet = 100;
    public int DefaultCapacity_SniperBullet = 100;
    public int DefaultCapacity_RPGBullet = 100;
    public int DefaultCapacity_KnifeBullet = 100;
    // 오브젝트 풀 맥스 사이즈
    public int MaxPistolBulletPoolSize = 1000;
    public int MaxShotgunBulletPoolSize = 1000;
    public int MaxRifleBulletPoolSize = 1000;
    public int MaxSniperBulletPoolSize = 200;
    public int MaxRPGBulletPoolSize = 200;
    public int MaxKnifeBulletPoolSize = 200;

    // 프리팹들
    // Prefabinjection이 삽입해줌
    public GameObject ZombiePrefab;
    public GameObject MeteoPrefab;
    public GameObject PistolBulletPrefab;
    public GameObject ShotgunBulletPrefab;
    public GameObject RifleBulletPrefab;
    public GameObject SniperBulletPrefab;
    public GameObject RPGBulletPrefab;
    public GameObject KnifeBulletPrefab;
    public GameObject JavelinBulletPrefab;

    // 생성 위치
    public Transform SpawnBullets;
    public Transform SpawnZombies;
    public Transform Meteos;

    // 오브젝트 풀
    public IObjectPool<GameObject> ZombiePool { get; private set; }
    public IObjectPool<GameObject> MeteorPool { get; private set; }
    public IObjectPool<GameObject> PistolBulletPool { get; private set; }
    public IObjectPool<GameObject> ShotgunBulletPool { get; private set; }
    public IObjectPool<GameObject> RifleBulletPool { get; private set; }
    public IObjectPool<GameObject> SniperBulletPool { get; private set; }
    public IObjectPool<GameObject> RPGBulletPool { get; private set; }
    public IObjectPool<GameObject> KnifeBulletPool { get; private set; }
    public IObjectPool<GameObject> JavelinBulletPool { get; private set; }

    private void Init()
    {
        // 링크 참조
        ZombiePool = new ObjectPool<GameObject>(CreatePooledZombieItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
            true, DefaultCapacity_Zombie, MaxZombiePoolSize);
        MeteorPool = new ObjectPool<GameObject>(CreatePooledMeteorItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
            true, DefaultCapacity_Meteor, MaxMeteorPoolSize);
        PistolBulletPool = new ObjectPool<GameObject>(CreatePooledPistolBulletItem, OnTakeFromPool, OnReturnedToPistolBulletPool, OnDestroyPoolObject,
            true, DefaultCapacity_Bullet, MaxPistolBulletPoolSize);
        ShotgunBulletPool = new ObjectPool<GameObject>(CreatePooledShotgunBulletItem, OnTakeFromPool, OnReturnedToShotgunBulletPool, OnDestroyPoolObject,
            true, DefaultCapacity_Bullet, MaxShotgunBulletPoolSize);
        RifleBulletPool = new ObjectPool<GameObject>(CreatePooledRifleBulletItem, OnTakeFromPool, OnReturnedToRifleBulletPool, OnDestroyPoolObject,
            true, DefaultCapacity_Bullet, MaxRifleBulletPoolSize);
        SniperBulletPool = new ObjectPool<GameObject>(CreatePooledSniperBulletItem, OnTakeFromPool, OnReturnedToSniperBulletPool, OnDestroyPoolObject,
            true, DefaultCapacity_SniperBullet, MaxSniperBulletPoolSize);
        RPGBulletPool = new ObjectPool<GameObject>(CreatePooledRPGBulletItem, OnTakeFromPool, OnReturnedToRPGBulletPool, OnDestroyPoolObject,
            true, DefaultCapacity_RPGBullet, MaxRPGBulletPoolSize);
        KnifeBulletPool = new ObjectPool<GameObject>(CreatePooledKnifeBulletItem, OnTakeFromPool, OnReturnedToKnifeBulletPool, OnDestroyPoolObject,
            true, DefaultCapacity_KnifeBullet, MaxKnifeBulletPoolSize);
        JavelinBulletPool = new ObjectPool<GameObject>(CreatePooledJavelinBulletItem, OnTakeFromPool, OnReturnedToJavelinBulletPool, OnDestroyPoolObject,
            true, DefaultCapacity_KnifeBullet, MaxKnifeBulletPoolSize);
    }
    // 프리팹인젝션이 초반에 이코드를 실행해서 프리팹을 넣어준다.
    public void InjectPrefab(GameObject zombiePrefab, GameObject meteoPrefab, GameObject pistolBulletPrefab, GameObject shotgunBulletPrefab,
        GameObject rifleBulletPrefab, GameObject sniperBulletPrefab, GameObject rpgBulletPrefab, GameObject knifeBulletPrefab,
        GameObject javelinBulletPrefab)
    {
        ZombiePrefab = zombiePrefab;
        MeteoPrefab = meteoPrefab;
        PistolBulletPrefab = pistolBulletPrefab;
        ShotgunBulletPrefab = shotgunBulletPrefab;
        RifleBulletPrefab = rifleBulletPrefab;
        SniperBulletPrefab = sniperBulletPrefab;
        RPGBulletPrefab = rpgBulletPrefab;
        KnifeBulletPrefab = knifeBulletPrefab;
        JavelinBulletPrefab = javelinBulletPrefab;
        Init();
    }
    // 여기 아래부터는 모두 함수들 블로그 참고
    private GameObject CreatePooledZombieItem()
    {
        GameObject poolGO = Instantiate(ZombiePrefab, SpawnZombies);
        Zombie zombie = poolGO.GetComponent<Zombie>();
        zombie.ZombiePool = ZombiePool;
        Managers.Instance.Game.ZombieController.Zombies.Add(zombie.ZombieChaseAI);
        return poolGO;
    }
    private GameObject CreatePooledMeteorItem()
    {
        GameObject poolGO = Instantiate(MeteoPrefab, Meteos);
        Meteo meteo = poolGO.GetComponent<Meteo>();
        meteo.MeteoPool = MeteorPool;
        return poolGO;
    }
    private GameObject CreatePooledPistolBulletItem()
    {
        GameObject poolGO = Instantiate(PistolBulletPrefab, SpawnBullets);
        PistolBullet bullet = poolGO.GetComponent<PistolBullet>();
        bullet.BulletPool = PistolBulletPool;
        bullet.Trail.SetActive(false);
        return poolGO;
    }
    private GameObject CreatePooledShotgunBulletItem()
    {
        GameObject poolGO = Instantiate(ShotgunBulletPrefab, SpawnBullets);
        ShotgunBullet bullet = poolGO.GetComponent<ShotgunBullet>();
        bullet.BulletPool = ShotgunBulletPool;
        bullet.Trail.SetActive(false);
        return poolGO;
    }
    private GameObject CreatePooledRifleBulletItem()
    {
        GameObject poolGO = Instantiate(RifleBulletPrefab, SpawnBullets);
        RifleBullet bullet = poolGO.GetComponent<RifleBullet>();
        bullet.BulletPool = RifleBulletPool;
        bullet.Trail.SetActive(false);
        return poolGO;
    }
    private GameObject CreatePooledSniperBulletItem()
    {
        GameObject poolGO = Instantiate(SniperBulletPrefab, SpawnBullets);
        SniperBullet bullet = poolGO.GetComponent<SniperBullet>();
        bullet.BulletPool = SniperBulletPool;
        bullet.Trail.SetActive(false);
        return poolGO;
    }
    private GameObject CreatePooledRPGBulletItem()
    {
        GameObject poolGO = Instantiate(RPGBulletPrefab, SpawnBullets);
        RPGBullet bullet = poolGO.GetComponent<RPGBullet>();
        bullet.BulletPool = RPGBulletPool;
        bullet.Trail.SetActive(false);
        return poolGO;
    }
    private GameObject CreatePooledKnifeBulletItem()
    {
        GameObject poolGO = Instantiate(KnifeBulletPrefab, SpawnBullets);
        KnifeBullet bullet = poolGO.GetComponent<KnifeBullet>();
        bullet.BulletPool = KnifeBulletPool;
        bullet.Trail.SetActive(false);
        return poolGO;
    }
    private GameObject CreatePooledJavelinBulletItem()
    {
        GameObject poolGO = Instantiate(JavelinBulletPrefab, SpawnBullets);
        JavelinBullet bullet = poolGO.GetComponent<JavelinBullet>();
        bullet.BulletPool = JavelinBulletPool;
        bullet.Trail.SetActive(false);
        return poolGO;
    }
    private void OnTakeFromPool(GameObject poolGO)
    {
        poolGO.SetActive(true);
    }
    private void OnReturnedToPool(GameObject poolGO)
    {
        poolGO.transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        poolGO.layer = 7;
        poolGO.SetActive(false);
    }
    private void OnReturnedToPistolBulletPool(GameObject poolGO)
    {
        PistolBullet bullet = poolGO.GetComponent<PistolBullet>();
        bullet.Trail.SetActive(false);
        poolGO.SetActive(false);
    }
    private void OnReturnedToShotgunBulletPool(GameObject poolGO)
    {
        ShotgunBullet bullet = poolGO.GetComponent<ShotgunBullet>();
        bullet.Trail.SetActive(false);
        poolGO.SetActive(false);
    }
    private void OnReturnedToRifleBulletPool(GameObject poolGO)
    {
        RifleBullet bullet = poolGO.GetComponent<RifleBullet>();
        bullet.Trail.SetActive(false);
        poolGO.SetActive(false);
    }
    private void OnReturnedToSniperBulletPool(GameObject poolGO)
    {
        SniperBullet bullet = poolGO.GetComponent<SniperBullet>();
        bullet.Trail.SetActive(false);
        poolGO.SetActive(false);
    }
    private void OnReturnedToRPGBulletPool(GameObject poolGO)
    {
        RPGBullet bullet = poolGO.GetComponent<RPGBullet>();
        bullet.Trail.SetActive(false);
        bullet.Explosion.gameObject.SetActive(false);
        bullet.BulletMesh.SetActive(true);
        poolGO.SetActive(false);
    }
    private void OnReturnedToKnifeBulletPool(GameObject poolGO)
    {
        KnifeBullet bullet = poolGO.GetComponent<KnifeBullet>();
        bullet.Trail.SetActive(false);
        poolGO.SetActive(false);
    }

    private void OnReturnedToJavelinBulletPool(GameObject poolGO)
    {
        JavelinBullet bullet = poolGO.GetComponent<JavelinBullet>();
        bullet.Trail.SetActive(false);
        poolGO.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject poolGO)
    {
        Destroy(poolGO);
    }
}
