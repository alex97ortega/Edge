using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    public Obstaculo ob;

    private void OnTriggerStay(Collider other)
    {
        if (ob.EstaActivado() && other.tag == "player")
        {
            other.transform.position += new Vector3(ob.velocity * ob.x, ob.velocity * ob.y, ob.velocity * ob.z);
        }
    }
   
}
