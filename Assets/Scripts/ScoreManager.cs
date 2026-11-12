using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Events;

public class ScoreManager : NetworkBehaviour
{
    //Almacena todas las puntuaciones de los jugadores
    //Relaciona cada puntuación con el OwnerClientID de cada jugador (0: Host, 1: Cliente 1, ...)
    public NetworkList<int> playerScores = new NetworkList<int>();

    //Lista con los textos de la UI que se irán actualizando. Usará la misma ID que la de puntuaciones
    public List<TMP_Text> scoreTexts = new();
    public TMP_Text endGameTxt;

    public int scoreToWin = 5;

    //Callback para cuando se termina la partida
    public UnityAction<ulong> OnGameEnded;

    public static ScoreManager Instance;

    public bool gameEnded;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //Añadir una función para cuando se modifica cualquier puntuación de la lista 
        playerScores.OnListChanged += UpdateScores;
    }

    //El callback del NetworkList envía por separado el índice y el valor del elemento que ha cambiado
    //En la variable de tipo Event, el Index es el índice del objeto y Value su valor
    void UpdateScores(NetworkListEvent<int> ev)
    {
        //Si el texto de esa puntuación está oculto, se activa
        //scoreTexts[ev.Index].gameObject.SetActive(true);

        //Actualizar el texto de puntuación correspondiente a la ID del jugador que ha cambiado
        scoreTexts[ev.Index].text = ev.Value.ToString();

        //Se comprueba si se ha llegado a la puntuación máxima. Si ocurre, se acaba la partidilla
        if(ev.Value >= scoreToWin)
        {
            EndGame((ulong)ev.Index);
        }
    }

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
            if(!gameEnded)
            {
                AddScoreRpc(playerID);
            }
        }
    }

    //Este RPC solo se envía al Host para que pueda hacer cosas con privilegios
    [Rpc(SendTo.Server)]
    void AddScoreRpc(int playerID)
    {
        playerScores[playerID]++;
    }
    //Crear una nueva puntuación para el jugador que entre nuevo
    public void CreateNewScore(int playerID)
    {
        if(IsServer == true)
        {
            playerScores.Add(0);
        }

        //Activar un nuevo texto de puntuación para el jugador que ha entrado
        scoreTexts[playerID].gameObject.SetActive(true);

        //Si el jugador que ejecuta esta función es el mismo que nuestro jugador local, es nuestra puntuación
        if((int)NetworkManager.LocalClientId  == playerID)
        {
            scoreTexts[playerID].color = Color.mediumSpringGreen;
        }
    }

    //ulong es como un int pero guarda números más grandes y solo puede ser positivo
    void EndGame(ulong winnerID)
    {
        gameEnded = true;
        //Llamar al callback de partida terminada
        OnGameEnded?.Invoke(winnerID);
        endGameTxt.gameObject.SetActive(true);

        if(NetworkManager.LocalClientId == winnerID)
        {
            endGameTxt.color = Color.mediumSpringGreen;
            endGameTxt.text = "VICTORIA";
        }

        //Si la ID del ganador coincide con la nuestra, hemos ganado
        if(NetworkManager.LocalClientId == winnerID)
        {
            Debug.Log("Victoria");
        }
        else //Si no, somos unos fracasados
        {
            Debug.Log("Derrota");
        }
    }
}
