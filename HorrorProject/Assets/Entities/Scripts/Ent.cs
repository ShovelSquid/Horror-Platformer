using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent : MonoBehaviour
{
    public Vector2 mouseInputVector;
    public Vector3 movementInputVector;
    public GameObject cursor;
    public float speed = 10f;
    public float mouseSensitivity = 1000;
    public float cursorDistance;
    public float userSphereCircumpherence;
    // Start is called before the first frame update
    void Start()
    {
        cursorDistance = Vector3.Distance(cursor.transform.position, transform.position);
        userSphereCircumpherence = cursorDistance * 2 * Mathf.PI;
    }
    protected void RotateTo(Vector3 target)
    {
        // Debug.Log("mewwooow");
        Vector3 targetDirection = target - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        // transform.rotation = Quaternion.RotateTowards(target.transform.rotation, Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0), 20);
    }
    protected void UpdateMouseInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        mouseInputVector = new Vector2(mouseX, mouseY);
        
        float horizontalInput = Input.GetAxisRaw("Horizontal");;
        float verticalInput = Input.GetAxisRaw("Vertical");;

        Vector3 prevMovementVector = movementInputVector;
        movementInputVector = ((horizontalInput * transform.right) + (verticalInput * transform.forward)).normalized;
    }

    protected void UpdateCursor(GameObject target)
    {
        Vector3 newPosition = target.transform.position + new Vector3(mouseInputVector.x, 0, mouseInputVector.y);
        if (newPosition != transform.position)
        {
            RotateTo(newPosition);
            Vector3 targetDirection = newPosition - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);
            target.transform.position = transform.position + cursorDistance * newDirection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseInput();
        UpdateCursor(cursor);
        Debug.DrawLine(transform.position, cursor.transform.position, Color.green);
        // RotateTo(cursor);
    }
}
