using UnityEngine;

public class ZenithGravityHandler : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float posResetTime = 5.0f;

    private float VelocityY;

    private float airTime = 0.0f;

    private Vector3 defaultPos; 

    private void Start()
    {
        defaultPos = this.transform.position;
    }

    void Update()
    {
        HandleGravity();
        
        if(airTime >= posResetTime) { 
            ResetPos();
        }
        Vector3 ApplyGravity = new Vector3(0, VelocityY, 0);

        controller.Move(ApplyGravity * Time.deltaTime);
    }

    private void HandleGravity()
    {
        if (controller.isGrounded)
        {
            airTime = 0;
            if (VelocityY < 0)
            {
                VelocityY= -2f; 
            }
        }
        else 
        {
            VelocityY += gravity * Time.deltaTime;
            airTime += Time.deltaTime;
        }
    }

    private void ResetPos()
    {
        controller.enabled = false;
        this.transform.position = defaultPos; 
        airTime = 0;
        controller.enabled = true;
    }
}
