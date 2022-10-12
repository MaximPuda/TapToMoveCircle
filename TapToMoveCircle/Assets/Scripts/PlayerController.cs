using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 3;
    [SerializeField] private SpriteRenderer _outline;

    public int Coins { get; private set; }
    public Vector3 StartPosition { get; private set; }

    public event UnityAction<int> OnAddCoin;
    public event UnityAction OnPlayerDeath;
    
    private Camera _camera;
    private LineRenderer _line;
    private SpriteRenderer _spriteRenderer;
    private ParticleSystem _deathFx;

    private List<Vector3> _targets;
    private GameState _gameState;

    #region UnityFunctions
    private void Awake()
    {
        _camera = Camera.main;
        _line = GetComponent<LineRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _deathFx = transform.GetComponentInChildren<ParticleSystem>();

        _targets = new();
    }

    private void Update()
    {
        _gameState = GameManager.Instance.State;

        if(_gameState == GameState.Running)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 newTarget = _camera.ScreenToWorldPoint(Input.mousePosition);
                AddTarget(new Vector3(newTarget.x, newTarget.y, 0f));
            }

            if (_targets.Count > 0)
            {
                float deltaMagnitude = (_targets[0] - transform.position).magnitude;
                if (deltaMagnitude <= 0.01f)
                    DeleteTarget();
                else
                    Move();
            }
        }
    }
    #endregion

    #region Movement
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targets[0], _speed * Time.deltaTime);
        UpdateLineZeroPoint();
    }

    private void AddTarget(Vector3 target)
    {
        _targets.Add(target);
        _line.positionCount++;
        _line.SetPosition(_line.positionCount - 1, target);
    }

    private void DeleteTarget()
    {
        _targets.RemoveAt(0);
        RefreshLine();
    }

    private void RefreshLine()
    {
        var newLinePositions = new Vector3[_targets.Count + 1];
        var newZeroPos = _line.GetPosition(1);

        _targets.CopyTo(newLinePositions, 1);
        newLinePositions[0] = newZeroPos;
        _line.positionCount = newLinePositions.Length;
        _line.SetPositions(newLinePositions);
    }

    private void UpdateLineZeroPoint()
    {
        _line.SetPosition(0, transform.position);
    }

    public void ClearTargets()
    {
        _targets.Clear();
        _line.positionCount = 1;
        UpdateLineZeroPoint();
    }
    #endregion

    #region Actions
    public void Death()
    {
        _deathFx.Play();
        _spriteRenderer.enabled = false;
        _outline.enabled = false;
        _line.positionCount = 0;

        OnPlayerDeath?.Invoke();
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        OnAddCoin?.Invoke(Coins);
    }
    #endregion
}
