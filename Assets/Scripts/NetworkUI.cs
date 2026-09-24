using UnityEngine;
using Unity.Netcode;

public class NetworkUI : MonoBehaviour
{
    public void Play()
    {
        if (!NetworkManager.Singleton.StartHost())
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
