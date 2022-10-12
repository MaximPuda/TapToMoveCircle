using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _maxEnemies;
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private float _enemyPositionOffset;

    [Header("Coins")]
    [SerializeField] private GameObject _coinPrefab;
    [SerializeField] private int _maxCoins;
    [SerializeField] private Transform _coinsContainer;
    [SerializeField] private float _coinPositionOffset;

    [Header("Position")]
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _borderOffset = 0.3f;
    
    private float _maxX;
    private float _maxY;
    private float _radius;

    private void Awake()
    {
        var camBorder = Camera.main.ViewportToWorldPoint(Vector2.zero);
        _maxX = camBorder.x * -1 - _borderOffset;
        _maxY = camBorder.y * -1 - _borderOffset;
    }

    private void Start()
    {
        GameManager.Instance.OnGameRun += SpawnAll;
        GameManager.Instance.OnGameLose += DisableObjects;
        GameManager.Instance.OnGameWin += DisableObjects;
    }    

    private void OnDestroy()
    {
        GameManager.Instance.OnGameRun -= SpawnAll;
        GameManager.Instance.OnGameLose -= DisableObjects;
        GameManager.Instance.OnGameWin -= DisableObjects;
    }    

    public void SpawnAll()
    {
        float _coinSize = _coinPrefab.GetComponent<Collider2D>().bounds.size.x;
        float _playerSize = _player.GetComponent<Collider2D>().bounds.size.x;

        _radius = _playerSize + _enemyPositionOffset;
        SpawnObject(_enemyPrefab, _maxEnemies, _enemiesContainer);

        _radius = _coinSize + _coinPositionOffset;
        SpawnObject(_coinPrefab, _maxCoins, _coinsContainer);
    }

    private void SpawnObject(GameObject obj, int count, Transform container)
    {
        int counter = 0;
        while (container.childCount < count)
        {
            Vector3 position = GetNewPosition();
            if (position != _player.StartPosition)
                SpawnObject(obj, position, container);
            else counter++;

            if (counter >= 50)
                break;
        }
    }

    private void SpawnObject(GameObject obj, Vector3 position, Transform container)
    {
        var newObj = Instantiate(obj, position, Quaternion.identity, container);
    }

    private Vector3 GetNewPosition()
    {
        Vector3 pos = GetRandomPosition();

        if(CheckIsOverlap(pos))
            return _player.StartPosition;

        return pos;
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-_maxX, _maxX), Random.Range(-_maxY, _maxY), 0);
    }

    private bool CheckIsOverlap(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, _radius);
        return colliders.Length > 0;
    }

    public void DisableObjects()
    {
        _coinsContainer.gameObject.SetActive(false);
        _enemiesContainer.gameObject.SetActive(false);
    }
}
