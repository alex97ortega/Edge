using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    float contColor;
    bool r, g, b;
    // Use this for initialization
    void Start () {
        r = true;
        g = b = false;
        contColor = 0.5f;
    }
	
	// Update is called once per frame
	void Update () {
        CambiaColor();
    }
    private void CambiaColor()
    {
        contColor += (3*Time.deltaTime);
        if (r)
        {
            if (contColor > 1 || contColor < 0)
            {
                r = false;
                g = true;
                contColor = 0.5f;
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = new Color(contColor, 1, 1);
            }
        }
        else if (g)
        {
            if (contColor > 1|| contColor < 0)
            {
                g = false;
                b = true;
                contColor = 0.5f;
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = new Color(1, contColor, 1);
            }
        }
        else if (b)
        {
            if (contColor > 1 || contColor < 0)
            {
                r = true;
                b = false;
                contColor = 0.5f;
            }
            else
            {
                GetComponent<MeshRenderer>().material.color = new Color(1, 1, contColor);
            }
        }
    }
}
