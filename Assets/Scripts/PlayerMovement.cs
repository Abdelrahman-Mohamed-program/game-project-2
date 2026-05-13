using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    public GameObject pauseMenu;

private bool isPaused = false;
    public float speed = 5f;
    private float velocityY;

private bool inDialogue = false;
    private bool nearDoor = false;
public float gravity = -9.81f;
    public float mouseSensitivity = 2f;
private Animator animator;
public Animator characterAnimator;

public TextMeshProUGUI sleepText;

//npc's
private bool nearNPC = false;
private bool dialogueActive = false;

private string[] currentDialogue;
private int dialogueIndex = 0;

private string currentNPC;

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("NPC"))
{
    nearNPC = true;

    sleepText.text = "Press 1 to talk";
    sleepText.gameObject.SetActive(true);

    currentNPC = other.gameObject.name;
}
    Debug.Log("TRIGGER HIT: " + other.name);

    if (other.CompareTag("Bed"))
    {
        nearBed = true;

        sleepText.text = "Press E to sleep";
        sleepText.gameObject.SetActive(true);

        Debug.Log("TEXT SHOULD SHOW NOW");
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

    if (!dialogueActive)
    {
        sleepText.gameObject.SetActive(false);
    }
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

animator = GetComponent<Animator>();
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
    dialogueActive = true;

    Time.timeScale = 0f;

    if (currentNPC == "jordan")
    {
        currentDialogue = new string[]
        {
            "You don't have much time Son.",
            "Stay focused.",
            "Do not waste seconds."
        };
    }
    else if (currentNPC == "remy")
    {
        currentDialogue = new string[]
        {
            "I still dance even in chaos.",
            "Keeps the fear away."
        };
    }
    else if (currentNPC == "bella")
    {
        currentDialogue = new string[]
        {
            "Something feels wrong here.",
            "Be careful."
        };
    }

    dialogueIndex = 0;

    sleepText.text = currentDialogue[dialogueIndex];
}
void NextDialogue()
{
    dialogueIndex++;

    if (dialogueIndex < currentDialogue.Length)
    {
        sleepText.text = currentDialogue[dialogueIndex];
    }
    else
    {
        EndDialogue();
    }
}
void EndDialogue()
{
    dialogueActive = false;

    Time.timeScale = 1f;

    sleepText.gameObject.SetActive(false);
}
    void Update()
    {
        if (nearNPC && !dialogueActive && Input.GetKeyDown(KeyCode.Alpha1))
{
    StartDialogue();
}

if (dialogueActive && Input.GetKeyDown(KeyCode.E))
{
    NextDialogue();
}

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
bool isMoving = x != 0 || z != 0;


if (isMoving)

{

    characterAnimator.speed = 1;

}

else

{

    characterAnimator.speed = 0;

}
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