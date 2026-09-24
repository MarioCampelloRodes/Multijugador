using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 4f;
    void Start()
    {
        
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
    }
}
