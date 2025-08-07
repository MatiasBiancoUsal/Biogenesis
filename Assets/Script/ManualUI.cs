using UnityEngine;

public class ManualUI : MonoBehaviour
{
    public GameObject manualPanel;

    public void OpenManual()
    {
        manualPanel.SetActive(true);
    }

    public void CloseManual()
    {
        manualPanel.SetActive(false);
    }
}
