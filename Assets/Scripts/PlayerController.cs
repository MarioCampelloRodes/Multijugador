using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 4f;

    //Variable sincronizada automáticamente para la vida
    //Para que el controlador de este objeto pueda actualizar el valor de la vida, hay que cambiar el
    //permiso de escritura (WritePermission) a Owner. Si no, solo el Server puede modificar el valor
    public NetworkVariable<int> currentHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Renderer hat, hatCap, johnnyGlasses;
    public Material playerColor;

    public Image healthBarImage;

    void Start()
    {
        if (IsLocalPlayer)
        {
            hat.material = playerColor;
            hatCap.material = playerColor;
        }

        currentHealth.OnValueChanged += UpdateHealthBar;
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            //Hay que añadir .Value para que deje modificarla
            currentHealth.Value -= 15;
        }
    }

    //Lo que se envía como parámetro mantiene ese valor en todos los jugadores
    //Si se envía un color específico, todos reciben ese mismo color y se sincroniza correctamente
    [Rpc(SendTo.Everyone)]
    void ChangeColorRpc(Color color)
    {
        johnnyGlasses.material.color = color;
    }

    //Se añade al callback OnValueChanged de la variable de la vida para que se actualice solamente cuando la vida cambie su valor
    void UpdateHealthBar(int oldValue, int newValue)
    {
        healthBarImage.fillAmount = newValue / 100f;
    }
}
