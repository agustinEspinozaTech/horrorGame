using UnityEngine;

public class MenuCursorSetup : MonoBehaviour
{
    [SerializeField] bool showCursor = true;

    void Awake()
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = 1f; // por si vienes desde un pause
    }
}