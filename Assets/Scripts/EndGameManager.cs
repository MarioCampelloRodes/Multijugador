using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public float timeToExit = 1;
    void Start()
    {
        ScoreManager.Instance.OnGameEnded += TheEndOfTheGame;

        //OnClientStopped se llama automáticamente cuando se desconecta del online
        //Añadimos una función anónima para recargar la escena cuando se haya desconectado del todo
        NetworkManager.Singleton.OnClientStopped += (stopped) =>
        {
            //Recargar la escena
            SceneManager.LoadScene(SceneManager. GetActiveScene().buildIndex);
        };
    }

    void TheEndOfTheGame(ulong winnerID)
    {
        StartCoroutine(EndGameCRT());
    }

    IEnumerator EndGameCRT()
    {
        //Efecto cámara lenta
        Time.timeScale = 0.3f;

        yield return new WaitForSeconds(timeToExit);

        Time.timeScale = 1;
        //Con esta función se termina la conexión con el Host / Cliente y se vuelve a modo de un jugador, sin conexión
        NetworkManager.Singleton.Shutdown();
    }
}
