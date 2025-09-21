using UnityEngine;
using UnityEngine.UI;

public class InventoryUIAdvanced : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject inventoryPanel;
    public Image buttonImage;
    public RectTransform buttonRectTransform; // Referencia para saber a quién mover
    public Sprite openSprite;
    public Sprite closeSprite;

    [Header("Posiciones")]
    public Vector2 openPosition;
    public Vector2 closePosition;

    private bool isOpen = false;

    void Start()
    {
        // El script ahora usa la referencia pública 'buttonRectTransform', 
        // por lo que ya no necesitamos la línea que estaba aquí.
        UpdateButtonUI();
        inventoryPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
        UpdateButtonUI();
    }

    private void UpdateButtonUI()
    {
        buttonImage.sprite = isOpen ? closeSprite : openSprite;
        buttonRectTransform.anchoredPosition = isOpen ? closePosition : openPosition;
    }
}