using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float Sensitivity = 100f;
    public Transform Player;
    public float xRot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Sensitivity * Time.deltaTime; 

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        Player.Rotate(Vector3.up * mouseX);
    }
}
