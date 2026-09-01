using System;
using UnityEngine;

public class SuperBulletController : MonoBehaviour
{
    private GameObject target;
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.target = GameObject.FindGameObjectWithTag("SuperAttackTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 direction = target.transform.position - this.transform.position;
        direction.Normalize();

        this.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        this.transform.rotation = Quaternion.LookRotation(direction);
        //this.transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetSpeed(float _s)
    {
        this.speed = _s;
    }
}
