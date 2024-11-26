using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    //private static InputManager _inputManager; 
    private static GameManager _gameManager;
    private static ObjectPoolManager _objectPoolManager;
    private static TimeManager _timeManager;
    private static DataManager _dataManager;

    public static Managers Instance
    {
        get
        {
            Init();
            return _instance;
        }
    }

    //public InputManager Input { get { return _inputManager; } }
    public GameManager Game { get { return _gameManager; } }
    public ObjectPoolManager ObjectPool { get { return _objectPoolManager; } }
    public TimeManager TimeManager { get { return _timeManager; } }
    public DataManager DataManager { get { return _dataManager; } }

    private void Awake()
    {
        _instance = this;
        //_inputManager = GetComponent<InputManager>();
        _gameManager = GetComponent<GameManager>();
        _objectPoolManager = GetComponent<ObjectPoolManager>();
        _timeManager = GetComponent<TimeManager>();
        _dataManager = GetComponent<DataManager>();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Init()
    {
        if (Time.timeScale == 0)
            return;

        if (_instance == null)
        {
            GameObject obj = GameObject.Find("Managers");
            if (obj == null)
            {
                obj = new GameObject { name = "Managers" };
                _instance = obj.AddComponent<Managers>();
                //_inputManager = obj.AddComponent<InputManager>();
                _gameManager = obj.AddComponent<GameManager>();
                _objectPoolManager = obj.AddComponent<ObjectPoolManager>();
                _timeManager = obj.AddComponent<TimeManager>();
                _dataManager = obj.AddComponent<DataManager>();
            }
            DontDestroyOnLoad(obj);
        }
    }
    /// <summary>
    /// 색상있는 디버그 문 출력
    /// </summary>
    /// <param name="ColorCode">컬러 코드 (#abcdef or blue or red)</param>
    /// <param name="msg">출력할 메세지</param>
    public static void Print(string ColorCode, string msg) => Debug.Log($"<color={ColorCode}>{msg}</color>");

    private void OnApplicationQuit()
    {
        Time.timeScale = 0;
    }
}
