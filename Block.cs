using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.instance.UpdateScore();
            Invoke("FallDown", 0.2f);
        }
    }

    void FallDown()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        Destroy(gameObject, 2f);
    }
}