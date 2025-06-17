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
        Instantiate(criaturaExperimentoPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("¡Criatura experimento creada!");

        // Guardar el estado para usarlo en otra escena
        PlayerPrefs.SetInt("CriaturaDesbloqueada", 1);

        // Cargar la escena donde aparecerá la criatura
        SceneManager.LoadScene("SampleScene"); // Asegurate que el nombre sea exacto
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
                slot.spriteActual = icono;
                ActualizarSlotVisual(slot);
                break;
            }
        }

        RevisarADNCompletos(); // Revisión automática después de cada recolección
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
    }



}
