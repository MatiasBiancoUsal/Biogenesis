using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ModoPocion { Mesa, Maletin }
public enum TipoPocion { Vida, Mejora }

public class AgarrarPocion : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 posicionInicial;

    [Header("Inventario (UI)")]
    // Prefab UI que usás para guardar en el maletín (tu variable original)
    public GameObject prefabParaMaletin;

    [Header("Prefab 2D que debe instanciarse en el mundo")]
    public GameObject prefabMundo2D;     // Asigná aquí PocionVida_2D o PocionMejora_2D
    public TipoPocion tipo = TipoPocion.Vida;

    [Header("Modo")]
    public bool autoDetectarModo = true;
    public ModoPocion modo = ModoPocion.Mesa;

    [Header("Opcional")]
    public float zMundo = 0f;            // Z donde spawnea en tu mundo 2D

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (autoDetectarModo)
            DetectarModoAutomatico();
    }

    void DetectarModoAutomatico()
    {
        if (canvas == null) { modo = ModoPocion.Mesa; return; }

        //Si el Canvas tiene el tag específico, gana
        if (canvas.CompareTag("MaletinCanvas")) { modo = ModoPocion.Maletin; return; }

        //Fallback por nombre
        string cname = canvas.gameObject.name.ToLower();
        if (cname.Contains("malet")) modo = ModoPocion.Maletin;
        else modo = ModoPocion.Mesa;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        posicionInicial = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (modo == ModoPocion.Mesa)
        {
            //Flujo MESA guardar en maletín (tu comportamiento original)
            if (prefabParaMaletin != null && MaletinManager.instancia != null)
            {
                MaletinManager.instancia.GuardarPocion(prefabParaMaletin);
            }

            gameObject.SetActive(false); // ocultar de la mesa
            return;
        }

        //Flujo MALETÍN usar sobre criatura (instanciar 2D encima del objetivo) 
        if (modo == ModoPocion.Maletin)
        {
            if (prefabMundo2D == null)
            {
                Debug.LogWarning("[AgarrarPocion] Falta asignar prefabMundo2D.");
                rectTransform.anchoredPosition = posicionInicial;
                return;
            }

            Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(mouseWorld);

            if (col == null)
            {
                // No soltaste sobre nada "válido" del mundo vuelve al slot
                rectTransform.anchoredPosition = posicionInicial;
                return;
            }

            // Validación de objetivo según el tipo de poción
            bool objetivoValido = false;
            Transform objetivoTransform = col.transform;

            if (tipo == TipoPocion.Vida)
            {
                // VidaPickup cura a objetos que tengan Personaje
                var pj = col.GetComponent<Personaje>() ??
                         col.GetComponentInChildren<Personaje>() ??
                         col.GetComponentInParent<Personaje>();

                if (pj != null)
                {
                    objetivoValido = true;
                    objetivoTransform = pj.transform;
                }
            }
            else if (tipo == TipoPocion.Mejora)
            {
                // PocionMejora busca IMutable
                var mutable = col.GetComponent<IMutable>() ??
                              col.GetComponentInChildren<IMutable>() ??
                              col.GetComponentInParent<IMutable>();

                if (mutable != null)
                {
                    objetivoValido = true;
                    objetivoTransform = (mutable as MonoBehaviour)?.transform ?? col.transform;
                }
            }

            if (!objetivoValido)
            {
                // Soltaste en algo del mundo, pero no es target válido para esta poción
                rectTransform.anchoredPosition = posicionInicial;
                return;
            }

            // Instanciar la versión 2D sobre el objetivo (para que el trigger entre)
            Vector3 spawnPos = objetivoTransform.position;
            spawnPos.z = zMundo;
            Instantiate(prefabMundo2D, spawnPos, Quaternion.identity);

            // Quitar del inventario / UI
            if (MaletinManager.instancia != null)
            {
                MaletinManager.instancia.QuitarPocion(prefabParaMaletin.tag);
            }

            Destroy(gameObject); // eliminar el icono del maletín
        }
    }
}
