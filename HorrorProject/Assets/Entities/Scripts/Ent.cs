using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ent : MonoBehaviour
{
    public GameObject cursor;
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected void RotateTo(GameObject target)
    {
        Debug.Log("mewwooow");
        Vector3 targetDirection = target.transform.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, speed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
        // transform.rotation = Quaternion.RotateTowards(target.transform.rotation, Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0), 20);
    }

    // Update is called once per frame
    void Update()
    {
        RotateTo(cursor);
    }
}
