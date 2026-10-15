using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 4;
    public float rotationSpeed = 10;
    //Variable sincronizada automaticamente para la vida
    //Para que el controlador de este objeto pueda actualizar el valor de la vida, hay que cambiar el
    //permiso de escritura (WritePermission) a OWNER. Si no, solo el Server puede modificar el valor
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //Para que el jugador local tenga el sombrero de otro color
    public Renderer hat;
    public Image healthBarImg;

    private Rigidbody rb;
    private Vector3 input;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Si es el jugador local, cambia el color del sombrero
        if(IsLocalPlayer == true)
        {
            hat.material.color = Color.cornflowerBlue;
        }

        currentHealth.OnValueChanged += UpdateHealthBar;
    }

    void Update()
    {
        //Si este objeto NO pertenece al jugador local, no se mueve
        if (IsLocalPlayer == false)
        {
            return;
        }

        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(input), Time.deltaTime * rotationSpeed);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeColorRpc(Random.ColorHSV());
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            currentHealth.Value -= 15;
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = input.normalized;
        movement *= moveSpeed;
        movement.y = 0;
        rb.linearVelocity = movement;
    }

    //Lo que se envia como parametro mantiene ese valor en todos los jugadores
    //Si se envia un color especifico, todos reciben ese mismo color y se sincroniza correctamente
    [Rpc(SendTo.Everyone)]
    void ChangeColorRpc(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    //Se añade al callback OnValueChanged de la variable de la vida para que se actualice solamente
    //cuando la vida cambie su valor
    //Para cualquier OnValueChanged de una NetworkVariable hay que añadir un parametro para el valor anterior
    //y otro para el valor actual
    void UpdateHealthBar(int oldValue, int newValue)
    {
        healthBarImg.fillAmount = newValue / 100f;
    }
}
