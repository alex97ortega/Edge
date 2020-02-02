using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMini : MonoBehaviour {

    public bool reduce;
    Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame

    void Update () {
        transform.localScale = new Vector3(transform.localScale.x *0.95f, transform.localScale.y, transform.localScale.z * 0.95f);
        if (transform.localScale.x <= 0.1f)
            transform.localScale = initialScale;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().TriggerMiniController(reduce);
        }
    }
}
