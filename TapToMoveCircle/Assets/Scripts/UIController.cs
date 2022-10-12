using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private Animator _uiAnimator;
    [SerializeField] private GameObject _loading;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _tapToStart;
    [SerializeField] private TextMeshProUGUI _gameOverTitle;
    [Header("Messages")]
    [SerializeField] private string _winMessage;
    [SerializeField] private string _looseMessage;

    private void Start()
    {
        GameManager.Instance.OnGameStart += ShowPreStart;
        GameManager.Instance.OnGameRun += ShowHUD;
        GameManager.Instance.OnGameLose += ShowLoseScreen;
        GameManager.Instance.OnGameWin += ShowWinScreen;
        _player.OnAddCoin += UpdateScore;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= ShowPreStart;
        GameManager.Instance.OnGameRun -= ShowHUD;
        GameManager.Instance.OnGameLose -= ShowLoseScreen;
        GameManager.Instance.OnGameWin -= ShowWinScreen;
        _player.OnAddCoin -= UpdateScore;
    }

    private void ShowPreStart()
    {
        _uiAnimator.SetBool("GameOver", false);
        _uiAnimator.SetBool("GameRun", false);
    }

    private void ShowHUD()
    {
        _uiAnimator.SetBool("GameRun", true);
        _loading.SetActive(false);
    }

    private void ShowLoseScreen()
    {
        _gameOverTitle.text = _looseMessage;
        _uiAnimator.SetBool("GameOver", true);
    }

    private void ShowWinScreen()
    {
        _gameOverTitle.text = _winMessage;
        _uiAnimator.SetBool("GameOver", true);
    }

    private void UpdateScore(int score) => _score.text = score.ToString();
}
