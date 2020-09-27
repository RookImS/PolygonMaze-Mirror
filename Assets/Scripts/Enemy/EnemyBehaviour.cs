using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float moveSpeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Bullet")
        {
            //  mat.color = new Color(0, 1, 1);
            Debug.Log("Destroy Bullet");
            Destroy(gameObject);

        }

    }
}
