using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoAmount = 15;
    public GameObject boxDespawnAnimation;

    // Start is called before the first frame update
    void Start()
    {
        //määrä
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Pickup", GetComponent<Transform>().position);
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn()
    {
        Debug.Log("Despawn cube");
        this.gameObject.GetComponent<Renderer>().enabled = false;
        Instantiate(boxDespawnAnimation, transform.position, transform.rotation);
        yield return new WaitForSeconds(10);
        this.gameObject.GetComponent<Renderer>().enabled = true;
        Debug.Log("Respawn cube");
    }

    private void FixedUpdate()
    {
        gameObject.transform.Rotate(new Vector3(0, 0, 10 * Time.deltaTime));
    }

}
