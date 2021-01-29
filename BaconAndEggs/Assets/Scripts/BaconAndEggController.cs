using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class BaconAndEggController : MonoBehaviour{

    [SerializeField]
    private GameObject playerCamera, poop;

    [SerializeField]
    private float moveSpeed, lookSpeedHorizontal, lookSpeedVertical, flap;
    private float speed, cameraX, flapTimer;
    private bool jump, crouch, sprint, pooped;
    [SerializeField]
    public bool canMove, bacon;
    [SerializeField]
    private int poops;

    private Rigidbody rb;

    private void Awake() {
        speed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        playerCamera.transform.localRotation = Quaternion.identity;
        cameraX = 0;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (canMove) {
            KeyboardMovementInput();
            MouseInput();
            if (bacon) {
                Poop();
            }
            else {
                Jump();
            }
        }
    }

    private void KeyboardMovementInput() {
        int ver = 0, hor = 0;
        if (Input.GetAxisRaw("Vertical") == 1) {
            ver = 1;
        }
        else if (Input.GetAxisRaw("Vertical") == -1) {
            ver = -1;
        }
        if (Input.GetAxisRaw("Horizontal") == 1) {
            hor = 1;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1) {
            hor = -1;
        }
        if (ver != 0 && hor == 0) { //Just forward backward
            transform.position += transform.forward * speed * ver * Time.deltaTime;
        }
        else if (ver == 0 && hor != 0) { //Just left right
            transform.position += transform.right * speed * hor * Time.deltaTime;
        }
        else { //Both  --> Diagonal Constant 0.7111111
            transform.position += transform.forward * speed * 0.71111f * ver * Time.deltaTime;
            transform.position += transform.right * speed * 0.71111f * hor * Time.deltaTime;
        }
    }

    private void MouseInput() {
        Vector2 inputMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (inputMouse.x != 0) {
            transform.RotateAround(transform.position, transform.up, lookSpeedHorizontal * inputMouse.x * Time.deltaTime);
        }
        if (inputMouse.y != 0) {
            float change = inputMouse.y * lookSpeedVertical * Time.deltaTime;
            cameraX = Mathf.Clamp(cameraX + change, -0.44f, 0.84f);
            playerCamera.transform.localRotation = new Quaternion(cameraX, 0, 0, -1);
        }
    }

    private void Poop() {
        if(poops > 0) {
            if(Input.GetAxisRaw("Fire1") == 1 && !pooped) {
                pooped = true;
                poops--;
                Instantiate(poop, transform.position, Quaternion.identity);
            }
            else if(Input.GetAxisRaw("Fire1") == 0) {
                pooped = false;
            }
        }
    } //Use poop bool for both falpping and pooping as they are never used on the same object

    private void Jump() {
        if(flapTimer <= 0.1f) {
            if (Input.GetAxisRaw("Fire1") == 1 && !pooped) {
                pooped = true;
                rb.AddForce(0, flap, 0);
            }
        }
        if (Input.GetAxisRaw("Fire1") == 0) {
            pooped = false;
        }
    }
}


/*
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649
public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCamera, toolWheel;

    [SerializeField]
    private LayerMask rayIgnore;
    [SerializeField]
    private Image crosshair;
    [SerializeField]
    private Text crosshairText;
    [SerializeField]
    private Sprite defaultCross, intertactCross;

    private GameObject[] toolbelt = new GameObject[8];
    int selectedTool = 0, targetSlot;

    private void Awake() {
        canMove = true;
        speed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        playerCamera.transform.localRotation = Quaternion.identity;
        cameraX = 0;

        GameObject dud = GameObject.FindGameObjectWithTag("DUD");
        toolbelt[0] = dud;
        toolbelt[1] = dud;
        toolbelt[2] = dud;
        toolbelt[3] = dud;
        toolbelt[4] = dud;
        toolbelt[5] = dud;
        toolbelt[6] = dud;
        toolbelt[7] = dud;

        toolWheelAngle = 0;
        toolWheel.transform.Rotate(0, 0, 0);
    }

    private void FixedUpdate() {
        if (canMove) {
            KeyboardMovementInput();
            MouseInput();
            InteractionsInput();
        }
        if (toolWheelAngle != targetAngle) {
            changingToolAngle = true;
            if(targetAngle > toolWheelAngle) {
                toolWheelAngle += Time.deltaTime * 120f;
                if(targetAngle >= toolWheelAngle + 360) {
                    targetAngle -= 360;
                }
            }
            else {
                toolWheelAngle -= Time.deltaTime * 120f;
                if (targetAngle <= toolWheelAngle - 360) {
                    targetAngle += 360;
                }
            }

            if(Helper.Within(toolWheelAngle, targetAngle, 2f)) {
                toolWheelAngle = targetAngle;
            }
            toolWheel.transform.localRotation = Quaternion.Euler(0, 0, toolWheelAngle);
        }
        else if(changingToolAngle) {
            if(toolWheelAngle >= 360) {
                toolWheelAngle -= 360;
                targetAngle -= 360;
            }
            else if(toolWheelAngle < 0){
                toolWheelAngle += 360;
                targetAngle += 360;
            }
            changingToolAngle = false;
            selectedTool = targetSlot;
        }
    }

    private void KeyboardMovementInput() {
        int ver = 0, hor = 0;
        if (Input.GetAxisRaw("Vertical") == 1) {
            ver = 1;
        }
        else if (Input.GetAxisRaw("Vertical") == -1) {
            ver = -1;
        }
        if (Input.GetAxisRaw("Horizontal") == 1) {
            hor = 1;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1) {
            hor = -1;
        }
        if (ver != 0 && hor == 0) { //Just forward backward
            transform.position += transform.forward * speed * ver * Time.deltaTime;
        }
        else if(ver == 0 && hor != 0){ //Just left right
            transform.position += transform.right * speed * hor * Time.deltaTime;
        }
        else { //Both  --> Diagonal Constant 0.7111111
            transform.position += transform.forward * speed * 0.71111f * ver * Time.deltaTime;
            transform.position += transform.right * speed * 0.71111f * hor * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Alpha1)) {
            ChangeToolBySlot(0);
        }
        else if (Input.GetKey(KeyCode.Alpha2)) {
            ChangeToolBySlot(1);
        }
        else if (Input.GetKey(KeyCode.Alpha3)) {
            ChangeToolBySlot(2);
        }
        else if (Input.GetKey(KeyCode.Alpha4)) {
            ChangeToolBySlot(3);
        }
        else if (Input.GetKey(KeyCode.Alpha5)) {
            ChangeToolBySlot(4);
        }
        else if (Input.GetKey(KeyCode.Alpha6)) {
            ChangeToolBySlot(5);
        }
        else if (Input.GetKey(KeyCode.Alpha7)) {
            ChangeToolBySlot(6);
        }
        else if (Input.GetKey(KeyCode.Alpha8)) {
            ChangeToolBySlot(7);
        }
    }

    private void MouseInput() {
        Vector2 inputMouse = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (inputMouse.x != 0) {
            transform.RotateAround(transform.position, transform.up, lookSpeedHorizontal * inputMouse.x * Time.deltaTime);
        }
        if (inputMouse.y != 0) {
            float change = inputMouse.y * lookSpeedVertical * Time.deltaTime;
            cameraX = Mathf.Clamp(cameraX + change, -0.84f, 0.84f);
            playerCamera.transform.localRotation = new Quaternion(cameraX, 0, 0, -1);
        }

        //Scrolling of tool wheel
        float wheel = Input.GetAxis("Mouse ScrollWheel");
        if (wheel != 0) {
            ChangeTool(wheel);
        }
    }

    private void InteractionsInput() {
        Ray ray = new Ray(playerCamera.transform.position,playerCamera.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 3f, Color.blue);
        crosshair.sprite = defaultCross;
        crosshairText.text = "";
        if (Physics.Raycast(ray, out hit, 3f, ~rayIgnore)) {
            if (hit.collider.tag.Equals("PickUp")) {
                crosshair.sprite = intertactCross;
                crosshairText.text = "Press [Interact] to pick up " + hit.collider.name;
            }
        }
    }

    //////////////////////////////////////////////////////////// TOOL WHEEL FUNCTIONS
    private void ChangeTool(float f) {
        if(f > 0f) {
            if (targetSlot < toolbelt.Length - 1) {
                targetSlot++;
            }
            else {
                targetSlot = 0;
            }
            targetAngle += 45;
        }
        else {
            if (targetSlot == 0) {
                targetSlot = toolbelt.Length - 1;
            }
            else {
                targetSlot--;
            }
            targetAngle -= 45;
        }
    }

    private void ChangeToolBySlot(int i) {
        if(i > 7) {
            i = 7;
        }
        else if(i < 0) {
            i = 0;
        }
        targetSlot = i;
        targetAngle = targetSlot * 45;
    }

    public GameObject[] GetToolbelt() {
        return toolbelt;
    }

    private void AddToBelt(GameObject obj, int beltSlot) {
        toolbelt[beltSlot] = obj;
        //Add to a slot id there is a free slot
        //If ot replace item in current slot with added item
    }

    private void RemoveFromBelt() {
        //Drop obect in current slot
    }
//////////////////////////////////////////////////////////// 
}
     */
