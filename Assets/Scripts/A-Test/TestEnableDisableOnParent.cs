using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnableDisableOnParent : MonoBehaviour {


    private void OnEnable() {
        print("Child is Enabled");
    }


    private void OnDisable() {
        print("Child is Disabled");
    }

}
