using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649
public class GameManager : MonoBehaviour{
    private bool bacon, canSwap;
    [SerializeField]
    private GameObject baconCam, eggCam;
    [SerializeField]
    private BaconAndEggController baconCon, eggCon;

    private void Awake() {
        bacon = true;
        canSwap = true;
    }

    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (canSwap) {
                canSwap = false;

                if (bacon) {
                    SwapToEgg();
                    bacon = false;
                    Debug.Log("Egg Time");
                }
                else {
                    SwapToBacon();
                    bacon = true;
                    Debug.Log("Bacon Time");
                }

            }
        }
        if (Input.GetKeyUp(KeyCode.E)) {
            canSwap = true;
        }
    }

    private void SwapToEgg() {
        eggCam.SetActive(true);
        baconCam.SetActive(false);
        eggCon.canMove = true;
        baconCon.canMove = false;
    }
    
    private void SwapToBacon() {
        baconCam.SetActive(true);
        eggCam.SetActive(false);
        baconCon.canMove = true;
        eggCon.canMove = false;
    }
}
