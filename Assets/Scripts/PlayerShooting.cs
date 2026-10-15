using UnityEngine;
using Unity.Netcode;
using System.Collections;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Rigidbody projectilePrefab;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject hatVisuals;
    [SerializeField] private float timeBetweenShots = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsLocalPlayer) return;

        //Solo puede disparar si el objeto del sombrero está activado
        if (Input.GetMouseButtonDown(0) && hatVisuals.activeSelf)
        {
            ShootRpc(firePoint.position, transform.forward * projectileSpeed);
        }
    }

    //Este Rpc recibe como parámetro la posición y la dirección del proyectil para que se sincronice con todos los jugadores y vean el mismo disparo
    [Rpc(SendTo.Everyone)]
    void ShootRpc(Vector3 spawnPosition, Vector3 shootDirection)
    {
        StartCoroutine(ShootCRT(spawnPosition, shootDirection));
    }

    private IEnumerator ShootCRT(Vector3 spawnPosition, Vector3 shootDirection)
    {
        hatVisuals.SetActive(false);
        Rigidbody projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.AddForce(shootDirection, ForceMode.VelocityChange);

        yield return new WaitForSeconds(timeBetweenShots);

        hatVisuals.SetActive(true);
    }
}
