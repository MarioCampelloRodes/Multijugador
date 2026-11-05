using System.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 15;
    [SerializeField] private Rigidbody rb;

    //La ID del jugador al que pertenece este proyectil
    private int ownerID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 6);
    }

    public void Shoot(Vector3 force, int playerID)
    {
        ownerID = playerID;
        rb.AddForce(force, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage, ownerID);
        }

        Destroy(gameObject);
    }
}
