using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper{

    public static float Vector3Distance(Vector3 A, Vector3 B) {
        return Mathf.Sqrt(Mathf.Pow(B.x - A.x, 2) + Mathf.Pow(B.y - A.y, 2) + Mathf.Pow(B.z - A.z, 2));
    }
}