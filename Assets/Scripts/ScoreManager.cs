using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Collections.Generic;

public class ScoreManager : NetworkBehaviour
{
    //Almacena todas las puntuaciones de los jugadores
    //Relaciona cada puntuación con el OwnerClientID de cada jugador (0: Host, 1: Cliente 1, ...)
    public NetworkList<int> playerScores = new NetworkList<int>();

    //Lista con los textos de la UI que se irán actualizando. Usará la misma ID que la de puntuaciones
    public List<TMP_Text> scoreTexts = new();

    public static ScoreManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    //private void Start()
    //{
    //    //Añadir una función para cuando se modifica cualquier puntuación de la lista 
    //    playerScores.OnListChanged += UpdateScores;
    //}

    //void UpdateScores(NetworkListEvent<int> scores)
    //{
    //    for(int i = 0; i < scores.Value; i++)
    //    {

    //    }
    //}

    //Aumentar la puntuación del jugador
    public void AddScore(int playerID)
    {
        //La lista sólo la puede modificar el servidor / host
        //Si esta persona ya lo es, puede modificar la lista directamente
        if (NetworkManager.Singleton.IsServer)
        {
            playerScores[playerID]++;
        }
        else //Si esta persona NO es el Host, le pide al server que actualice la puntuación
        {
            AddScoreRpc(playerID);
        }
    }

    //Este RPC solo se envía al Host para que pueda hacer cosas con privilegios
    [Rpc(SendTo.Server)]
    void AddScoreRpc(int playerID)
    {
        playerScores[playerID]++;
    }
    //Crear una nueva puntuación para el jugador que entre nuevo
    public void CreateNewScore()
    {
        playerScores.Add(0);
        //Activar un nuevo texto de puntuación
        scoreTexts[playerScores.Count -1].gameObject.SetActive(true);
    }
}
