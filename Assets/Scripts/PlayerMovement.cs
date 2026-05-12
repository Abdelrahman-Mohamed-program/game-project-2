using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public GameObject pauseMenu;

private bool isPaused = false;
    public float speed = 5f;
    private float velocityY;
    private bool nearNPC = false;
private bool inDialogue = false;
    private bool nearDoor = false;
public float gravity = -9.81f;
    public float mouseSensitivity = 2f;



public TextMeshProUGUI sleepText;


void OnTriggerEnter(Collider other)
{
    Debug.Log("TRIGGER HIT: " + other.name);

    if (other.CompareTag("Bed"))
    {
        nearBed = true;

        sleepText.text = "Press E to sleep";
        sleepText.gameObject.SetActive(true);

        Debug.Log("TEXT SHOULD SHOW NOW");
    }
    if (other.CompareTag("NPC"))
{
    Debug.Log("NPC TRIGGER HIT");
    nearNPC = true;

    sleepText.text = "You are stronger than you think.\nPress E to continue";
    sleepText.gameObject.SetActive(true);
}

    if (other.CompareTag("Door"))
    {
        nearDoor = true;

        sleepText.text = "Press E to enter";
        sleepText.gameObject.SetActive(true);

        Debug.Log("DOOR TEXT SHOULD SHOW NOW");
    }
}


void OnTriggerExit(Collider other)
{
    if (other.CompareTag("Bed"))
    {
        nearBed = false;
        sleepText.gameObject.SetActive(false);
    }
    if (other.CompareTag("NPC"))
{
    nearNPC = false;
    sleepText.gameObject.SetActive(false);
}

    if (other.CompareTag("Door"))
    {
        nearDoor = false;
        sleepText.gameObject.SetActive(false);
    }
}
private bool nearBed = false;
    private CharacterController controller;
    private float verticalRotation = 0f;

    public Transform playerCamera;

  void Start()
{
    controller = GetComponent<CharacterController>();

    if (sleepText == null)
        Debug.LogError("SleepText is NOT assigned in Inspector!");

    sleepText.gameObject.SetActive(false);

    Cursor.lockState = CursorLockMode.Locked;
}
void PauseGame()
{
    pauseMenu.SetActive(true);

    Time.timeScale = 0f;
    isPaused = true;

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}

public void ExitGame()
{
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
}
public void ResumeGame()
{
    pauseMenu.SetActive(false);

    Time.timeScale = 1f;
    isPaused = false;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}
void Sleep()
{
   sleepText.text = "Press E to sleep";

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
}

void StartDialogue()
{
    inDialogue = true;

    sleepText.text = "You are stronger than you think.\n\tkeep going\n\nPress E to continue";

    Time.timeScale = 0f;

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
}

void EndDialogue()
{
    inDialogue = false;

    sleepText.gameObject.SetActive(false);

    Time.timeScale = 1f;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}
    void Update()
    {
    if (Input.GetKeyDown(KeyCode.Escape))
{
    if (isPaused)
    {
        ResumeGame();
    }
    else
    {
        PauseGame();
    }
}


if (nearNPC && Input.GetKeyDown(KeyCode.E))
{
    if (!inDialogue)
    {
        StartDialogue();
    }
    else
    {
        EndDialogue();
    }
}

        if (nearBed && Input.GetKeyDown(KeyCode.E)){
    Sleep();
}

if (nearDoor && Input.GetKeyDown(KeyCode.E))
{
   SceneManager.LoadScene(1);
}
        Move();
        Look();
    }


void Move()
{
    if (inDialogue || isPaused) return;
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 move = transform.right * x + transform.forward * z;

    // gravity handling
    if (controller.isGrounded && velocityY < 0)
    {
        velocityY = -2f; // keeps player stuck to ground
    }

    velocityY += gravity * Time.deltaTime;

    Vector3 finalMove = move * speed;
    finalMove.y = velocityY;

    controller.Move(finalMove * Time.deltaTime);
}
    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}