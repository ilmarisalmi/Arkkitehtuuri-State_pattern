using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Monster monster;

    public float Width = 1f;
    public float Height = 1f;
    public float Depth = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, Depth));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //spawn monster, set position within bounds of spawner, give monster random positive y rigidbody velocity, destroy monster after 5 seconds.
        if (Input.GetKeyDown(KeyCode.F))
        {
            Monster m = Instantiate(monster, transform.position + new Vector3(Random.Range(-Width / 2, Width / 2), Random.Range(-Height / 2, Height / 2), Random.Range(-Depth / 2, Depth / 2)), Quaternion.identity);
            m.GetComponent<Rigidbody>().velocity = new Vector3(0, Random.Range(0, 10), 0);
            Destroy(m.gameObject, 5f);
        }
    }
}
