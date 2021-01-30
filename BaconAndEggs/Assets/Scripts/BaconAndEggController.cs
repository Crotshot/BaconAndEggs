using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class BaconAndEggController : MonoBehaviour{

    [SerializeField]
    private GameObject playerCamera, poop;
    [SerializeField]
    public int flap, poops;
    [SerializeField]
    private float moveSpeed, lookSpeedHorizontal, lookSpeedVertical, flapPower, regenFlaps;
    private float speed, cameraX, flapTimer;
    private bool jump, crouch, sprint, pooped;
    [SerializeField]
    public bool canMove, bacon;
    [SerializeField]
    private AudioSource source;

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
                Regen();
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
                source.volume = Random.Range(0.5f, 0.8f);
                source.pitch = Random.Range(0.8f, 1.2f);
                source.Play();
                Instantiate(poop, transform.position, Quaternion.identity);
            }
            else if(Input.GetAxisRaw("Fire1") == 0) {
                pooped = false;
            }
        }
    } //Use poop bool for both falpping and pooping as they are never used on the same object

    private void Jump() {
        if(flapTimer <= 0.1f && flap > 0) {
            if (Input.GetAxisRaw("Fire1") == 1 && !pooped) {
                pooped = true;
                flap--;
                rb.AddForce(0, flapPower, 0);
                source.volume = Random.Range(0.5f, 0.8f);
                source.pitch = Random.Range(1.2f, 1.6f);
                source.Play();
            }
        }
        if (Input.GetAxisRaw("Fire1") == 0) {
            pooped = false;
        }
    }

    private void Regen() {
        if(regenFlaps > 0 && flap < 10) {
            regenFlaps -= Time.deltaTime;
        }
        else if (regenFlaps < 0 && flap < 10) {
            regenFlaps = 1f;
            flap++;
        }
    }
}