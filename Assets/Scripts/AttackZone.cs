using UnityEngine;

public class AttackZone : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((transform.root.CompareTag("Player1") && other.CompareTag("Player2")) ||
            (transform.root.CompareTag("Player2") && other.CompareTag("Player1")))
        {
            HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }
    }
}
