using UnityEngine;
using UnityEngine.Events;

public class Counter : MonoBehaviour
{
    public event UnityAction OnCountEnd;

    private GameState _gameState;
    private void Update()
    {
        _gameState = GameManager.Instance.State;

        if (_gameState == GameState.Running && transform.childCount == 0)
            OnCountEnd?.Invoke();
    }
}
