using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void EventHandler();
    public event EventHandler OnGridSceneLoaded;
    public event EventHandler OnMainSceneLoaded;

    public Player Player;
    public Boss Boss;

    [Header("Zombie Logic")]
    public ZombieController ZombieController;
    public ZombieBoxSpawner ZombieBoxSpawner_Sc;
    public ZombieBoxSpawner ZombieBoxSpawner_Ex;
    public ZombieSpawnManager ZombieSpawnManager;
    public ZombieStatsManager ZombieStatsManager;

    public TimeController TimeController;
    public InputManager InputManager;
    public UIController UIController;
    public CinemachineController CinemachineController;
    public WeaponController WeaponController;
    public AccessoryController AccessoryController;
    public MenuPresenter MenuPresenter;
    public OptionPresenter OptionPresenter;
    public ItemInfoPresenter ItemInfoPresenter;
    public EmotionPresenter EmotionPresenter;
    public BannerPresenter BannerPresenter;
    public DieUIPresenter DieUIPresenter;
    public GameAudio GameAudio;

    public bool isVictory = false;
    private void Awake()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        isVictory = false;
    }
    private void OnSceneChanged(Scene beforeScene, Scene afterScene)
    {
        if (afterScene.name == "SampleScene")
        {
            OnGridSceneLoaded();
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Managers.Instance.TimeManager.RestartTimeManager();
        GameAudio.PlayBgm(true);
    }
}
