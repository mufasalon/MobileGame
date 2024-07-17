using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    bool hasGameFinished;

    // Start is called before the first frame update
    void Start()
    {
        hasGameFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, 2f))
        {
            GetComponent<Rigidbody>().velocity = Vector3.down * 25f;
            if (!hasGameFinished)
            {
                hasGameFinished = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().isGameOver = true;
                GameManager.instance.GameOver();
                Destroy(gameObject, 5f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Diamond"))
        {
            GameManager.instance.UpdateDiamond();
            Destroy(other.gameObject);
        }
    }
}
