using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    Collider col;
    Renderer rend;
    Color myColor;
    Color gray = new Color(.5f, .5f, .5f, .5f);

    private void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        myColor = rend.material.color;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(.4f, .4f, .4f));
    }

    public IEnumerator UseFuel()
    {
        col.enabled = false;
        rend.material.color = gray;
        yield return new WaitForSeconds(10);
        col.enabled = true;
        rend.material.color = myColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        AddFuel auto = other.GetComponent<AddFuel>();
        if(auto != null && auto.Add())
        {
            StartCoroutine(UseFuel());
        }
    }
}
