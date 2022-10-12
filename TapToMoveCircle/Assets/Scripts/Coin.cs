using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int _coast;

    public int Coast => _coast;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.AddCoins(_coast);
            Destroy(transform.gameObject);
        }
    }
}
