using UnityEngine;
using Cinemachine;

public class AutoAlignCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    public Transform player;
    public float alignSpeed = 2f;

    private float mouseMoveTimer = 0f;
    private float mouseMoveCooldown = 1.5f;

    void Update()
    {
        // Detectar movimiento del mouse
        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.1f || Mathf.Abs(Input.GetAxis("Mouse Y")) > 0.1f)
        {
            mouseMoveTimer = mouseMoveCooldown; // resetea el timer
        }
        else
        {
            mouseMoveTimer -= Time.deltaTime; // cuenta regresiva
        }

        bool mouseMovedRecently = mouseMoveTimer > 0f;

        // Detectar movimiento del jugador
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isMoving = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (isMoving && !mouseMovedRecently)
        {
            Transform camTransform = virtualCam.transform;
            Vector3 targetDirection = player.forward;
            Vector3 currentDirection = camTransform.forward;
            Vector3 newDirection = Vector3.Slerp(currentDirection, targetDirection, alignSpeed * Time.deltaTime);

            camTransform.rotation = Quaternion.LookRotation(newDirection, Vector3.up);
        }
    }
}
