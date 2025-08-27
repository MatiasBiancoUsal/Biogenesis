using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItemComidaUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Prefab 2D correspondiente a este icono UI")]
    public GameObject comida2DPrefab;

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector3 startPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // transparencia mientras arrastro
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position += (Vector3)eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Posici�n del mouse en el mundo
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // Raycast hacia la escena 2D
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero);

        if (hit.collider != null)
        {
            CriaturaComidaEsp criatura = hit.collider.GetComponent<CriaturaComidaEsp>();
            if (criatura != null)
            {
                // Validamos si esta criatura acepta este tipo de comida
                if (criatura.comidaAceptada == comida2DPrefab)
                {
                    // Instancia r�pida del prefab (opcional: sirve para animaciones visuales)
                    GameObject comida = Instantiate(comida2DPrefab, mouseWorldPos2D, Quaternion.identity);
                    Destroy(comida, 0.1f); // lo destruimos enseguida

                    // Buscar la barra de hambre en la criatura y alimentarla
                    HungerBar hungerBar = criatura.GetComponent<CreatureEat>()?.hungerBar;
                    if (hungerBar != null)
                    {
                        hungerBar.Feed(0.2f); // Ajust� el valor seg�n cu�nto debe llenar
                        Debug.Log($"{criatura.name} comi� y se aliment�!");
                    }

                    // Eliminar el icono de la bandeja (consumir comida)
                    Destroy(gameObject);

                    // Avisar al BandejaManager que se liber� un espacio
                    BandejaManager bandejaManager = FindFirstObjectByType<BandejaManager>();
                    if (bandejaManager != null)
                    {
                        bandejaManager.QuitarComida();
                    }
                }
                else
                {
                    Debug.Log(criatura.name + " no acepta esta comida.");
                    rectTransform.position = startPos; // vuelve a la bandeja
                }
            }
        }
        else
        {
            rectTransform.position = startPos;
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }
}
