using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 4f;

    public Renderer hat, hatCap;
    public Material playerColor;
    void Start()
    {
        if (IsLocalPlayer)
        {
            hat.material = playerColor;
            hatCap.material = playerColor;
        }
    }

    void Update()
    {
        //Si este objeto NO pertenece al jugador local, no se mueve
        if (!IsLocalPlayer)
        {
            return;
        }
        
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += input * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.H))
        {
            Color randomColor = Random.ColorHSV(); //IMPORTANTE (aleatoriedad fuera de la función RPC)
            ChangeColorRpc(randomColor);
        }
    }

    //Lo que se envía como parámetro mantiene ese valor en todos los jugadores
    //Si se envía un color específico, todos reciben ese mismo color y se sincroniza correctamente
    [Rpc(SendTo.Everyone)]
    void ChangeColorRpc(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }
}
