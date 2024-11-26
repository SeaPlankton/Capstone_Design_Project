using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무조건 실행 순위 1순위. 실행 순서가 제일 맨 앞이어야 함.
/// </summary>
public class PrefabInjection : MonoBehaviour
{
    [Header("Controllers")]
    public TimeController TimeController;
    public UIController UIController;
    public WeaponController WeaponController;
    public AccessoryController AccessoryController;
    public CinemachineController CinemachineController;
    [Header("Prefabs and GameObjects")]
    public GameObject ZombiePrefab;
    public GameObject MeteoPrefab;
    public GameObject PistolPrefab;
    public GameObject RiflePrefab;
    public GameObject ShotgunPrefab;
    public GameObject SniperBulletPrefab;
    public GameObject RPGBulletPrefab;
    public GameObject KnifeBulletPrefab;
    public GameObject JavelinBulletPrefab;
    public Player Player;
    public Boss Boss;
    [Header("Zombie Controllers")]
    public ZombieController ZombieController;
    public ZombieBoxSpawner ZombieBoxSpawner_Ex;
    public ZombieBoxSpawner ZombieBoxSpawner_Sc;
    public ZombieStatsManager ZombieStatsManager;
    public ZombieSpawnManager ZombieSpawnManager;

    public InputManager InputManager;
    public MenuPresenter MenuPresenter;
    public OptionPresenter OptionPresenter;
    public ItemInfoPresenter ItemInfoPresenter;
    public EmotionPresenter EmotionPresenter;
    public BannerPresenter BannerPresenter;
    public DieUIPresenter DieUIPresenter;
    public GameAudio GameAudio;

    [Header("단순 묶기용")]
    public Transform SpawnBullets;
    public Transform SpawnZombies;
    public Transform Meteos;

    private void Awake()
    {
        Managers.Instance.Game.TimeController = TimeController;
        Managers.Instance.Game.UIController = UIController;
        Managers.Instance.Game.WeaponController = WeaponController;
        Managers.Instance.Game.AccessoryController = AccessoryController;
        Managers.Instance.Game.CinemachineController = CinemachineController;

        Managers.Instance.ObjectPool.InjectPrefab(ZombiePrefab, MeteoPrefab, PistolPrefab, ShotgunPrefab, RiflePrefab, SniperBulletPrefab, RPGBulletPrefab,
            KnifeBulletPrefab, JavelinBulletPrefab);

        Managers.Instance.Game.Player = Player;
        Managers.Instance.Game.Boss = Boss;

        Managers.Instance.Game.ZombieController = ZombieController;
        Managers.Instance.Game.ZombieBoxSpawner_Ex = ZombieBoxSpawner_Ex;
        Managers.Instance.Game.ZombieBoxSpawner_Sc = ZombieBoxSpawner_Sc;
        Managers.Instance.Game.ZombieStatsManager = ZombieStatsManager;
        Managers.Instance.Game.ZombieSpawnManager = ZombieSpawnManager;

        Managers.Instance.Game.InputManager = InputManager;
        Managers.Instance.Game.MenuPresenter = MenuPresenter;
        Managers.Instance.Game.OptionPresenter = OptionPresenter;
        Managers.Instance.Game.ItemInfoPresenter = ItemInfoPresenter;
        Managers.Instance.Game.EmotionPresenter = EmotionPresenter;
        Managers.Instance.Game.BannerPresenter = BannerPresenter;
        Managers.Instance.Game.DieUIPresenter = DieUIPresenter;
        Managers.Instance.Game.GameAudio = GameAudio;

        Managers.Instance.ObjectPool.SpawnBullets = SpawnBullets;
        Managers.Instance.ObjectPool.SpawnZombies = SpawnZombies;
        Managers.Instance.ObjectPool.Meteos = Meteos;
    }
}