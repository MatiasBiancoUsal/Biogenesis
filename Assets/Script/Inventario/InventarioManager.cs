using UnityEngine;
using UnityEngine.SceneManagement;

public class InventarioManager : MonoBehaviour
{
    public Slot[] slots;

    [Header("Criatura Experimento")]
    public GameObject criaturaExperimentoPrefab;
    public Transform spawnPoint; // Donde aparece la criatura
    public static InventarioManager instancia; // <--- ESTA ES LA LÍNEA CLAVE

    public bool criaturaCreada = false;

    void Awake()
    {
        if (instancia == null)
            instancia = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        SincronizarInventario();
    }

    //

    void SincronizarInventario()
    {
        foreach (Slot slot in slots)
        {
            // Buscamos en el InventarioGlobal si existe ese ADN recolectado
            InventarioGlobal.ADNItem item = InventarioGlobal.Instance.itemsADN.Find(i => i.nombre == slot.nombreADN);
            if (item != null)
            {
                slot.cantidad = item.cantidad;

                if (slot.cantidad > 0 && slot.spriteActual != null)
                {
                    slot.imagenSlot.sprite = slot.spriteActual;
                    slot.imagenSlot.enabled = true;
                }
                else
                {
                    slot.imagenSlot.enabled = false;
                }

                //ACTUALIZAR EL TEXTO CON LA CANTIDAD
                if (slot.cantidadTexto != null)
                {
                    slot.cantidadTexto.text = slot.cantidad.ToString();
                }
            }
        }
    }

    //

    void Update()
    {
        if (!criaturaCreada && TodosLosADNRecolectados())
        {
            CrearCriatura();
            CriaturaCreada.Instance.criaturaCreada = true;
        }
    }

    bool TodosLosADNRecolectados()
    {
        foreach (Slot slot in slots)
        {
            if (slot.cantidad <= 0)
                return false;
        }
        return true;
    }

    void CrearCriatura()
    {
        //Instantiate(criaturaExperimentoPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("¡Criatura experimento creada!");

        // Guardar el estado para usarlo en otra escena
        CriaturaCreada.Instance.criaturaCreada = true;
        criaturaCreada = true;

        // Cargar la escena donde aparecerá la criatura
        //SceneManager.LoadScene("SampleScene"); // Asegurate que el nombre sea exacto
    }
    private void RevisarADNCompletos()
    {
        bool todosReunidos = true;

        foreach (Slot slot in slots)
        {
            if (slot.cantidad <= 0)
            {
                todosReunidos = false;
                break;
            }
        }

        if (todosReunidos)
        {
            InstanciarCriatura();
        }
    }

    private void InstanciarCriatura()
    {
        if (criaturaExperimentoPrefab != null && spawnPoint != null)
        {
            Instantiate(criaturaExperimentoPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("¡Criatura Experimento creada!");
        }
    }

    public void AñadirADN(string nombreADN, Sprite icono)
    {
        foreach (Slot slot in slots)
        {
            if (slot.nombreADN == nombreADN)
            {
                slot.cantidad++;
                Debug.Log($"Añadido {nombreADN}: cantidad ahora {slot.cantidad}");

                slot.spriteActual = icono;
                ActualizarSlotVisual(slot); // ESTA FUNCIÓN DEBE LLAMARSE AQUÍ
                break;
            }
        }

        RevisarADNCompletos();
    }

    private void ActualizarSlotVisual(Slot slot)
    {
        if (slot.cantidad > 0 && slot.spriteActual != null)
        {
            slot.imagenSlot.sprite = slot.spriteActual;
            slot.imagenSlot.enabled = true;
        }
        else
        {
            slot.imagenSlot.enabled = false;
        }

        //ACTUALIZAR EL TEXTO SIEMPRE QUE CAMBIE LA CANTIDAD
        if (slot.cantidadTexto != null)
        {
            slot.cantidadTexto.text = slot.cantidad.ToString();
        }
    }



}
