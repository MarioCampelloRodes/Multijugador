using System.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 15;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 6);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
