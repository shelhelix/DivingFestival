using UnityEngine;

using System;
using System.Collections.Generic;

public class ParallaxController : MonoBehaviour {
    [Serializable]
    public class ParallaxObject {
        public Transform Object;
        public float     SpeedMultiplier = 1;
    }

    // Speed in units per second
    public float Speed = 1;
    
    public List<ParallaxObject> Objects;

    void Update() {
        var offset = Speed * Time.deltaTime;
        foreach ( var obj in Objects ) {
            obj.Object.position += (offset * obj.SpeedMultiplier) * Vector3.up;
        }
    }
}
