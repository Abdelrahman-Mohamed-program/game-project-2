using UnityEngine;
using TMPro;

public class NotebookUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI pageText;
    public string[] pages;
public bool isOpen = false;
    private int currentPage = 0;

    void Start()
    {
        panel.SetActive(false);
        currentPage = 0;
    }

    void Update()
    {
        if (!panel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
            CloseNotebook();

        if (Input.GetKeyDown(KeyCode.RightArrow))
            NextPage();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            PrevPage();
    }

public void OpenNotebook()
{
    Debug.Log("OpenNotebook called! Panel is: " + panel.name + " | active before: " + panel.activeSelf);
    isOpen = true;
    currentPage = 0;
    UpdatePage();
    panel.SetActive(true);
    Debug.Log("Panel active after SetActive: " + panel.activeSelf);
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    Time.timeScale = 0f;
}

public void CloseNotebook()
{
    isOpen = false;
    panel.SetActive(false);
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    Time.timeScale = 1f;
}
    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        pageText.text = pages[currentPage];
    }
}