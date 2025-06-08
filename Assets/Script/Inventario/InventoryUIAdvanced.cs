using UnityEngine;
using UnityEngine.UI;

public class InventoryUIAdvanced : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject inventoryPanel;
    public Image buttonImage;
    public Sprite openSprite;
    public Sprite closeSprite;

    [Header("Posiciones")]
    public Vector2 openPosition;
    public Vector2 closePosition;

    private RectTransform buttonRectTransform;
    private bool isOpen = false;

    void Start()
    {
        buttonRectTransform = GetComponent<RectTransform>();
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
