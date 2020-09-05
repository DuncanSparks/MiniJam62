using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	const float MotionInterpolateSpeed = 10.0f;
	const float RotationInterpolateSpeed = 10.0f;

    [SerializeField]
	GameObject model = null;
    [SerializeField]
    float maxHealth;
	[SerializeField]
    float attackRange;
	[SerializeField]
    float minimumFollowDistance;
	[SerializeField]
    float maximumFollowDistance;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Transform projectileSpawnLocation;
    

    float health;
    Quaternion modelRotation = Quaternion.identity;
    new Camera camera;
	new Rigidbody rigidbody;
	Animator animator;
    Vector3 velocity = Vector3.zero;
    Vector3 targetVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = Controller.Singleton.player;
        if(target&&animator.GetBool("InIdleState"))
        {
            Vector3 diff = target.transform.position - transform.position;
            diff.y=0;
            float distance = diff.magnitude;
            if(distance>minimumFollowDistance && distance <maximumFollowDistance)
            {
                //normalizing manually to avoid recalculating magnitude in .normalized
                targetVelocity = diff/distance*speed;
                animator.SetFloat("Walking", 1);
                modelRotation = Quaternion.LookRotation(targetVelocity);
            }
            else
            {
                targetVelocity = Vector3.zero;
                animator.SetFloat("Walking", 0);
            }
            if(distance<attackRange)
            {
                AttackUpdate();
            }
        }
    }
    
    float attackTimer;
    [SerializeField]
    float attackTime;
    void AttackUpdate()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer>attackTime)
        {
            attackTimer=0;
            targetVelocity = Vector3.zero;
            animator.SetBool("Attack", true);
        }
    }

    public Player.PaintColor color;

    public void SpawnAttack()
    {
        var glob = Instantiate(projectile, projectileSpawnLocation.position, model.transform.rotation);
        foreach (PaintGlob comp in glob.GetComponentsInChildren<PaintGlob>())
        {
            comp.Color = color;
        }
        Destroy(glob.gameObject, 3.0f);
    }

    void FixedUpdate()
    {
        velocity = Vector3.Lerp(velocity, targetVelocity, MotionInterpolateSpeed*Time.deltaTime);
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
        model.transform.rotation = Quaternion.Slerp(model.transform.rotation, modelRotation, RotationInterpolateSpeed * Time.deltaTime);
    }
    bool dead = false;
    void Damage(float amount, Vector3 knockbackDirection)
    {
        if(dead)return;
        health -= amount;
        targetVelocity = velocity = knockbackDirection;
        if(health>0)
        {
            animator.SetBool("Hurt", true);
        } else {
            animator.SetBool("Die", true);
            dead = true;
            Destroy(gameObject, 1f);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag=="PlayerProjectile")
        {
            PaintGlob paintGlob = other.gameObject.GetComponent<PaintGlob>();
            float damage = paintGlob.Damage;
            float knockback = 20; //TODO: get from paintGlob
            Damage(damage, -knockback*other.gameObject.transform.forward);
        }
    }
}
