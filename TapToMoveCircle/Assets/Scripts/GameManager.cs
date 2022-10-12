using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private Counter _coinsCounter;
    public GameState State { get; private set; }

    public event UnityAction OnGameStart;
    public event UnityAction OnGameRun;
    public event UnityAction OnGameWin;
    public event UnityAction OnGameLose;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _coinsCounter.OnCountEnd += Win;
        _player.OnPlayerDeath += Lose;
        StartGame();
    }

    private void Update()
    {
        if (State == GameState.Start && Input.GetMouseButtonDown(0))
            Run();
    }

    private void OnDestroy()
    {
        _coinsCounter.OnCountEnd -= Win;
        _player.OnPlayerDeath -= Lose;
    }

    private void StartGame()
    {
        State = GameState.Start;
        OnGameStart?.Invoke();
    }

    private void Run()
    {
        State = GameState.Running;
        _player.gameObject.SetActive(true);
        OnGameRun?.Invoke();
    }

    private void Win()
    {
        State = GameState.Win;
        OnGameWin?.Invoke();
    }

    private void Lose()
    {
        State = GameState.Lose;
        OnGameLose?.Invoke();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}

public enum GameState
{
    Start, Running, Win, Lose
}