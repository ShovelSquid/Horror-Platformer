using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent : MonoBehaviour
{
    public Vector2 mouseInputVector;
    public Vector3 movementInputVector;
    public GameObject cursor;
    public GameObject head;
    public CapsuleCollider pC;
    public Rigidbody rb;
    public float speed = 10f;
    public float flightDrag = 2f;
    public float downForceBase;
    public float downForce;
    public bool downForceOn = true;
    public float mouseSensitivity = 1000;
    public float cursorDistance;
    public float userSphereCircumpherence;
    public float xrotation;
    public float yrotation;
    protected bool leftclick = false;
    protected bool rightclick = false;
    protected bool moving = false;
    protected bool flying = false;
    protected bool running = false;
    public bool telekenetics = false;
    protected bool movingForward = false;
    protected bool movingBackward = false;
    protected bool movingLeftward = false;
    protected bool movingRightward = false;
    protected bool spaceDown = false;
    protected bool ctrlDown = false;
    public float airAccelerationSpeed;
    public float groundAccelerationSpeed;
    public float jumpForce;
    public float maxSpeed;
    public RaycastHit raycastHit;
    public GameObject hovering;
    public bool canHover = false;
    public bool canJump = false;
    public GameObject selected;
    private float selectedDistance;
    // Start is called before the first frame update
    void Start()
    {
        xrotation = transform.rotation.x;
        yrotation = transform.rotation.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
    protected void RotateMouse(GameObject obj)
    {
        yrotation += mouseInputVector.x;
        xrotation -= mouseInputVector.y;
        obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, Quaternion.Euler(xrotation, yrotation, 0), 20);
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
    protected void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            leftclick = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            leftclick = false;
            OnLeftClickUp();
        }
        if (Input.GetMouseButtonDown(1))
        {
            rightclick = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightclick = false;
            OnRightClickUp();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            movingForward = true;
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            movingForward = false;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movingBackward = true;
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            movingBackward = false;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            movingLeftward = true;
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            movingLeftward = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            movingRightward = true;
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            movingRightward = false;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceDown = true;
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            spaceDown = false;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ctrlDown = true;
            moving = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            ctrlDown = false;
        }
    }

    private void OnLeftClickUp()
    {
        if (telekenetics) {
            DeTelekenesize();
        }
    }
    private void OnRightClickUp()
    {
        Debug.Log("yeah cool man alert");
        if (telekenetics) {
            Telekenetics(false);
            StartFlying(false);
        }
        else {
            Telekenetics(true);
            StartFlying(true);
        }
    }

    protected void Telekenetics(bool on)
    {
        if (on) {
            canHover = true;
            telekenetics = true;
        }
        if (!on) {
            StopHovering();
            DeTelekenesize();
            canHover = false;
            telekenetics = false;
        }
        
    }

    protected void Telekinesize()
    {
        if (hovering != null) {
            // make selected and swap outline colors
            selected = hovering;
            Rigidbody r = selected.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.isKinematic = true;
            }
            canHover = false;
            Color primary;
            Color alternate;
            Outline outline = selected.GetComponent<Outline>();
            primary = outline.OutlineColor;
            alternate = outline.OutlineColorVariant;
            outline.OutlineColorVariant = primary;
            outline.OutlineColor = alternate;
            selectedDistance = raycastHit.distance*1.5f;
            hovering = null;
        }
        else if (selected == null) {
            return;
        }
        Debug.Log("Selected Distance: " + selectedDistance);
        selectedDistance += Input.mouseScrollDelta.y * selectedDistance/5f;
        RaycastHit cheese;
        Ray ray = new Ray(transform.position, Camera.main.transform.forward);
        selected.transform.position = ray.GetPoint(selectedDistance);
    }
    protected void DeTelekenesize()
    {
        if (selected != null) {
            StopHovering();
            Outline outline = selected.GetComponent<Outline>();
            Color primary = outline.OutlineColor;
            Color alternate = outline.OutlineColorVariant;
            outline.OutlineColorVariant = primary;
            outline.OutlineColor = alternate;
            canHover = true;
            Rigidbody r = selected.GetComponent<Rigidbody>();
            if (r != null)
            {
                r.isKinematic = false;
            }
            selected = null;
        }
    }

    protected void Run()
    {
        if (movingForward)
        {
            rb.AddForce(groundAccelerationSpeed * transform.forward, ForceMode.Acceleration);
        }
        if (movingBackward)
        {
            rb.AddForce(groundAccelerationSpeed * -transform.forward, ForceMode.Acceleration);
        }
        if (movingLeftward)
        {
            rb.AddForce(groundAccelerationSpeed * -transform.right, ForceMode.Acceleration);
        }
        if (movingRightward)
        {
            rb.AddForce(groundAccelerationSpeed * transform.right, ForceMode.Acceleration);
        }
        if (spaceDown)
        {
            TryJump();
        }
    }
    protected void TryJump()
    {
        if (canJump) {
            Jump();
        }
    }
    protected void Jump()
    {
        rb.AddForce(jumpForce * transform.up, ForceMode.Impulse);
        downForce = downForce/3;
    }

    protected void Fly() 
    {
        if (movingForward)
        {
            rb.AddForce(airAccelerationSpeed * transform.forward, ForceMode.Acceleration);
        }
        if (movingBackward)
        {
            rb.AddForce(airAccelerationSpeed * -transform.forward, ForceMode.Acceleration);
        }
        if (movingLeftward)
        {
            rb.AddForce(airAccelerationSpeed * -transform.right, ForceMode.Acceleration);
        }
        if (movingRightward)
        {
            rb.AddForce(airAccelerationSpeed * transform.right, ForceMode.Acceleration);
        }
        if (spaceDown)
        {
            rb.AddForce(airAccelerationSpeed * transform.up, ForceMode.Acceleration);
        }
        if (ctrlDown)
        {
            rb.AddForce(airAccelerationSpeed * -transform.up, ForceMode.Acceleration);
        }
        if (!movingForward && !movingBackward && !movingLeftward && !movingRightward && !spaceDown && !ctrlDown)
        {
            if (rb.velocity == Vector3.zero)
            {
                moving = false;
            }
            else 
            {
                moving = true;
            }
        }
        if (ctrlDown && spaceDown)
        {
            StartFlying(false);
        }
    }
    protected void StartFlying(bool on) 
    {
        if (!on)
        {
            flying = false;
            // rb.useGravity = true;
            rb.drag = 0;
            downForceOn = true;
            canJump = true;
            running = true;
        }
        else {
            flying = true;
            // rb.useGravity = false;
            rb.drag = flightDrag;
            downForceOn = false;
            canJump = false;
            running = false;
        }
    }

    protected void UpdateCursor(GameObject target)
    {
        // target.transform.position = transform.forward * 40f;
        // Vector3 newPosition = target.transform.position + new Vector3(mouseInputVector.x, mouseInputVector.y, 0);
        // target.transform.RotateAround(transform.position, newPosition, 20 * Time.deltaTime);
        // if (newPosition != transform.position)
        // {
        //     RotateTo(newPosition);
        //     Vector3 targetDirection = newPosition - transform.position;
        //     Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);
        //     target.transform.position = transform.position + cursorDistance * newDirection;
        // }
    }

    protected void SeeObjects()
    {
        Ray ray = Camera.main.ScreenPointToRay(transform.position);
        Physics.Raycast(transform.position, Camera.main.transform.forward, out raycastHit, 100000f);
        if (raycastHit.collider != null)
        {
            var mom = raycastHit.collider.gameObject;
            if (mom != hovering)
            {
                StopHovering();
            }
            if (mom.layer == 3)
            {
                hovering = mom;
                mom.GetComponent<Outline>().enabled = true;
            }
            cursor.transform.position = mom.transform.position;
        }
        else {
            StopHovering();
        }
    }
    void StopHovering()
    {
        if (hovering != null) {
            hovering.GetComponent<Outline>().enabled = false;
            hovering = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseInput();
        // UpdateCursor(cursor);
        RotateMouse(gameObject);
        Debug.DrawLine(transform.position, cursor.transform.position, Color.green);
        Debug.DrawLine(transform.position, transform.forward * 100000f, Color.green);
        Debug.DrawLine(head.transform.position, head.transform.forward * 100000f, Color.blue);
        // RotateTo(cursor);
        HandleInput();
        if (moving && flying) {
            Fly();
        }
        if (moving && running) {
            Run();
        }
        if (leftclick && telekenetics) {
            Telekinesize();
        }
        if (canHover) {
            SeeObjects();
        }
        if (downForceOn) {
            rb.velocity += new Vector3(0, -downForce, 0) * Time.deltaTime;
            if (downForce != downForceBase) {
                // Debug.Log("cheeeeese");
                downForce = Mathf.Lerp(downForce, downForceBase, 0.005f);
                if (downForceBase - downForce < 1) {
                    downForce = downForceBase;
                }
            }
        }
    }
}
