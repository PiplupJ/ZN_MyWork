using UnityEngine;

public class ZenithShotController : MonoBehaviour
{
    [SerializeField] private GameObject Explosion;

    [SerializeField] private float speed = 15f;
    //[SerializeField] private float rotateSpeed = 360f;
    [SerializeField] private float lifeTime = 6f;
    [SerializeField] private int Damage = 1;

    private Rigidbody rb;

    private bool isInit = false;

    private Vector3 targetPos;
    private Vector3 dir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Init_Shot(Transform _target)
    {   
        targetPos = _target.position;
        targetPos.y = transform.position.y;
        rb = GetComponent<Rigidbody>();
            dir = (targetPos - this.transform.position).normalized;
            transform.forward = dir;
            rb.linearVelocity = dir * speed;

            isInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInit) { return; }
        if(lifeTime<=0)
        {
            RocketExplosion();
            Destroy(gameObject);
            Debug.Log("Rocketlifetime");   
        }
        lifeTime -= Time.deltaTime;
    }

     private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHP.instance.DealDamage(Damage);
            RocketExplosion();
            Destroy(gameObject);
        }
    }
   
    private void RocketExplosion()
    {
            Vector3 spawnPos = transform.position;
            var e = Instantiate(Explosion, spawnPos, Quaternion.identity);
            e.GetComponent<ParticleSystem>().Play(); 
    }
}
