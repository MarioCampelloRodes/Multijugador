using UnityEngine;
using Unity.Netcode;

public class ScoreManager : MonoBehaviour
{
    //Almacena todas las puntuaciones de los jugadores
    //Relaciona cada puntuación con el OwnerClientID de cada jugador (0: Host, 1: Cliente 1, ...)

    public NetworkList<int> playerScores = new NetworkList<int>();

    public static ScoreManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0;i < playerScores.Count; i++)
        {
            Debug.Log($"Score for Player {i}: {playerScores[i]}");
        }
    }

    //Aumentar la puntuación del jugador
    public void AddScore(int playerID)
    {
        playerScores[playerID]++;
    }

    //Crear una nueva puntuación para el jugador que entre nuevo
    public void CreateNewScore()
    {
        playerScores.Add(0);
    }
}
