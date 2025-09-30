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

    private AudioSource audioSource; //Script lucy

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        audioSource = GetComponent<AudioSource>(); //Script lucy
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = rectTransform.position;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; // transparencia mientras arrastro
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Posición del mouse en el mundo
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        // Raycast hacia la escena 2D
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero);

        bool comidaConsumida = false; //NUEVO SOFI

        if (hit.collider != null)
        {
            CriaturaComidaEsp criatura = hit.collider.GetComponent<CriaturaComidaEsp>();
            if (criatura != null)
            {
                // Validamos si esta criatura acepta este tipo de comida
                if (criatura.comidaAceptada == comida2DPrefab)
                {
                    // Instancia rápida del prefab (opcional: sirve para animaciones visuales)
                    //GameObject comida = Instantiate(comida2DPrefab, mouseWorldPos2D, Quaternion.identity);
                    //Destroy(comida, 0.1f); // lo destruimos enseguida

                    // Buscar la barra de hambre en la criatura y alimentarla
                    HungerBar hungerBar = criatura.GetComponent<CreatureEat>()?.hungerBar;
                    if (hungerBar != null)
                    {
                        hungerBar.Feed(0.2f); // Ajustá el valor según cuánto debe llenar
                        Debug.Log($"{criatura.name} comió y se alimentó!");
                    }

                    //Script lucy
                    if (audioSource != null && audioSource.clip != null)
                    {
                        audioSource.Play();
                    }
                    //

                    comidaConsumida = true;
                }
            }
        }


        // Si la comida no fue consumida por una criatura, la devolvemos a la bandeja
        if (!comidaConsumida)
        {
            // Lógica para DEVOLVER el ícono a la bandeja
            rectTransform.position = startPos;
            canvasGroup.blocksRaycasts = true; // Se restaura
            canvasGroup.alpha = 1f; // Se restaura
        }
        else
        {
            // Lógica para CONSUMIR el ícono
            // 1. Avisar al BandejaManager que se liberó un espacio
            BandejaManager bandejaManager = FindFirstObjectByType<BandejaManager>();
            if (bandejaManager != null)
            {
                bandejaManager.QuitarComida();
            }

            // 2. Hacer el ícono invisible e in-clickeable mientras suena
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;

            // 3. Destruir el icono de la bandeja CON RETRASO
            float duracionSonido = (audioSource != null && audioSource.clip != null) ? audioSource.clip.length : 0.1f;
            Destroy(gameObject, duracionSonido);
        }
    }
}