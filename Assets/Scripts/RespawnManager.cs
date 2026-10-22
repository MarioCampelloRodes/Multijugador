using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] respawnPoints;

    public static RespawnManager Instance;

    public float timeToRespawn = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        respawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
    }

    public Transform GetRespawn()
    {
        //Devuelve un punto aleatorio
        return respawnPoints[Random.Range(0, respawnPoints.Length)].transform;
    }
}
