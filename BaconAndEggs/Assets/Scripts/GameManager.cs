using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#pragma warning disable CS0649
public class GameManager : MonoBehaviour{
    private bool bacon, canSwap;
    [SerializeField]
    private GameObject baconCam, eggCam;
    [SerializeField]
    private BaconAndEggController baconCon, eggCon;
    [SerializeField]
    private Sprite poop, flap;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text amount, objective, congradulationsText;

    private bool timerStart = false;
    private float timer = 2.5f;

    private void Awake() {
        bacon = true;
        canSwap = true;
        SwapToEgg();
        SwapToBacon();
    }

    private void FixedUpdate() {
        if (bacon) {
            amount.text = baconCon.poops.ToString();
        }
        else {
            amount.text = eggCon.flap.ToString();
        }
        if (Input.GetKey(KeyCode.E)) {
            if (canSwap) {
                canSwap = false;

                if (bacon) {
                    SwapToEgg();
                    bacon = false;
                    objective.text = "Find Bacon";
                }
                else {
                    SwapToBacon();
                    bacon = true;
                    objective.text = "Find Eggs";
                }
            }
        }
        else{
            canSwap = true;
        }

        if (Helper.Vector3Distance(baconCam.transform.position, eggCam.transform.position) <= 1f) {
            congradulationsText.text = "Bacon and Eggs have been reunited!";
            timerStart = true;
        }
        if (timerStart & timer > 0) {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void SwapToEgg() {
        eggCam.SetActive(true);
        baconCam.SetActive(false);
        eggCon.canMove = true;
        baconCon.canMove = false;
        icon.sprite = flap;
    }
    
    private void SwapToBacon() {
        baconCam.SetActive(true);
        eggCam.SetActive(false);
        baconCon.canMove = true;
        eggCon.canMove = false;
        icon.sprite = poop;
    }
}
