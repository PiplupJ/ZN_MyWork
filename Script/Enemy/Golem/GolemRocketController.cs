using UnityEngine;

public class GolemRocketController : MonoBehaviour
{
    [SerializeField] public Vector3 target { get; private set; }
    [SerializeField] private GameObject Explosion;
    //private Vector3 targetPos;
    private Vector3 LaunchDir = new Vector3(0, 1, 0);

    [SerializeField] private float speed = 15f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float RiseTime = 2f;
    [SerializeField] private float lifeTime = 15f;
    [SerializeField] private int RocketDamage = 1;

    public AudioClip moveSE;
    public AudioClip explodeSE;
    private AudioSource aud;

    private bool isInit = false;
    private Rigidbody rb;

    public void Init_Rocket(Vector3 target)
    {
        this.target = target;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(LaunchDir.normalized * speed, ForceMode.VelocityChange);
        isInit = true;
        aud = this.GetComponent<AudioSource>();
        aud.PlayOneShot(moveSE);
    }

    void FixedUpdate()
    {
        if(!isInit) { return; }
        if(RiseTime >0.0f){
            if (rb.linearVelocity.sqrMagnitude > 0.1f)
            rb.rotation = Quaternion.LookRotation(rb.linearVelocity);
            RiseTime -= Time.fixedDeltaTime;
        }
        else{
            HeadToTarget();
        }

        Vector3 curPos = this.transform.position;
        if(curPos.y<target.y||lifeTime<=0)
        {
            RocketExplosion();
            Destroy(gameObject);
            Debug.Log("Rocketlifetime");   
        }
        if(RiseTime>0){
            RiseTime -= Time.deltaTime;
        }
        lifeTime -= Time.deltaTime;
    }

    private void HeadToTarget()
    {
        if(this.target==null) return;

        Vector3 direction = (target - this.transform.position).normalized;

        Quaternion rotGoal = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotGoal, rotateSpeed * Time.fixedDeltaTime));

        rb.linearVelocity = transform.forward * speed;
    }

    /*
    private void ChasePlayer()
    {
        if (Player == null) return;
        targetPos = Player.transform.position + Vector3.up * 1.0f;
        Vector3 direction = (targetPos - transform.position).normalized;

    
        Quaternion rotGoal = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotGoal, rotateSpeed * Time.fixedDeltaTime));

        rb.linearVelocity = transform.forward * speed;
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //シュウが変更しました
            PlayerHP.instance.DealDamage(RocketDamage);

            // Apply damage
            Debug.Log("boom");
            RocketExplosion();
            Destroy(gameObject);
        }
    }
   
    private void RocketExplosion()
    {
        AudioSource.PlayClipAtPoint(explodeSE, transform.position);
        Vector3 spawnPos = transform.position;
            var e = Instantiate(Explosion, spawnPos, Quaternion.identity);
            e.GetComponent<ParticleSystem>().Play(); 
    }
}
