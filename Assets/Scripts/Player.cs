﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    const float CameraMouseRotationSpeed = 12f;
    const float CameraControllerRotationSpeed = 3.0f;
    const float CameraXRotMin = -26.0f;
    const float CameraXRotMax = 30.0f;

    const float DirectionInterpolateSpeed = 1.0f;
    const float MotionInterpolateSpeed = 10.0f;
    const float RotationInterpolateSpeed = 10.0f;

    const float Speed = 12.0f;
    const float JumpForce = 20.0f;

    [SerializeField]
    float respawnY = -20f;

    [SerializeField]
    float dashSpeed = 24.0f;
    [SerializeField]
    GameObject dashEffect;

    Quaternion modelRotation = Quaternion.identity;
    public Quaternion ModelRotation { get => modelRotation; set => modelRotation = value; }
    public Quaternion AimRotation { get => aimRotation; set => aimRotation = value; }
    
    Quaternion aimRotation = Quaternion.identity;
    float cameraXRot = 0.0f;

    float horizontal = 0.0f;
    float vertical = 0.0f;

    float mouseX = 0f;
    float mouseY = 0f;

    const int maxHealth = 4;
    int health = maxHealth;

    bool onGround = false;
    bool attacking = false;
    bool dashing = false;
    bool hurt = false;

    bool globalMouseAim = true;

    bool lockMovement = false;
    public bool LockMovement { get => lockMovement; set => lockMovement = value; }

    public enum PaintColor
    {
        Red,
        Blue,
        Yellow
    }

    PaintColor currentColor = PaintColor.Red;
    public PaintColor CurrentColor { set => currentColor = value; get => currentColor; }

    new Camera camera;
    new Rigidbody rigidbody;
    Animator animator;

    SkinnedMeshRenderer modelMaterials;
    MeshRenderer bucketMaterials;

    [SerializeField]
    Material[] colorMaterials;

	[SerializeField]
	AudioClip jumpSound;

    [SerializeField]
    AudioClip dashSound;

    [SerializeField]
    AudioClip damageSound;

    [SerializeField]
    AudioClip paintSound;

    [SerializeField]
    GameObject model = null;

    [SerializeField]
    GameObject cameraPivot = null;

    [SerializeField]
    GameObject cameraBase = null;

    [SerializeField]
    GameObject paintGlobsRed = null;

    [SerializeField]
    GameObject paintGlobBlue = null;

    [SerializeField]
    GameObject paintGlobsYellow = null;

    [SerializeField]
    LayerMask collisionMask;

    void Start()
    {
        camera = GetComponentInChildren<Camera>();
        rigidbody = GetComponent<Rigidbody>();
        animator = model.GetComponent<Animator>();
        modelMaterials = GetComponentInChildren<SkinnedMeshRenderer>();
        bucketMaterials = GetComponentInChildren<MeshRenderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyHitbox")
        {
            HitboxCollision(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyHitbox")
        {
            HitboxCollision(other.gameObject);
        }
    }

    void HitboxCollision(GameObject hitbox)
    {
        float damage = 1;
        float knockback = 20;
        Damage(damage, -knockback*hitbox.transform.forward);
    }

    Vector3 knockback;
    void Damage(float amount, Vector3 knockbackDirection)
    {
        Controller.Singleton.PlaySoundOneShot(damageSound, Random.Range(0.95f, 1.05f), 0.75f);
        animator.SetBool("Hurt", true);
        Controller.Singleton.ShowComicText("Splat", transform.position + new Vector3(0, 0.5f, 0), camera);
        knockback = knockbackDirection;
        ModelRotation = Quaternion.LookRotation(-knockback);
        model.transform.rotation = ModelRotation;
        health = Mathf.Clamp(--health, 0, maxHealth);
        GameUI.Singleton.SetHealth(health, maxHealth);
    }

    void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        attacking = animator.GetBool("InAttackState");
        dashing = animator.GetBool("InDashState");
        dashEffect.SetActive(dashing);
        hurt = animator.GetBool("InHurtState");
        if (attacking || hurt || lockMovement)
        {
            horizontal = 0;
            vertical = 0;
        }
        else
        {
            if (!dashing && Input.GetButtonDown("Fire3"))
            {
                Controller.Singleton.PlaySoundOneShot(dashSound, Random.Range(0.95f, 1.05f));
                Controller.Singleton.ShowComicText("Whoosh", transform.position + new Vector3(0, 0.5f, 0), camera);
                Dash();
            }
        }

        if (Input.GetButtonDown("AimMode"))
        {
            globalMouseAim ^= true;
            GameUI.Singleton.SetAimMode(globalMouseAim);
        }

        if (horizontal != 0f || vertical != 0f)
        {
            animator.SetFloat("Walking", 1);
        }
        else
        {
            animator.SetFloat("Walking", 0);
        }
        
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Fire1") && !hurt && !lockMovement)
        {
            animator.SetBool("Attack", true);
            Aim();
        }

        if (Input.GetButtonDown("Fire2") && !attacking && !hurt && !lockMovement)
        {
            currentColor = (PaintColor)(((int)currentColor + 1) % 3);
            GameUI.Singleton.SetIndicatorColor(currentColor);
           
            UpdateColorInfo();
        }

        onGround = Physics.Raycast(transform.position, Vector3.down, 1.2f, collisionMask);

        if (Input.GetButtonDown("Jump") && onGround && !attacking && !hurt && !lockMovement)
        {
			Controller.Singleton.PlaySoundOneShot(jumpSound, Random.Range(0.95f, 1.05f), 0.6f);
            animator.SetBool("EndJump", false);
            rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            animator.SetBool("StartJump", true);
            animator.SetBool("EndJump", false);
            animator.SetFloat("Jumping", 1);
        }

        if (!onGround)
        {
            if (rigidbody.velocity.y <= 0)
            {
                animator.SetFloat("Jumping", 2);
            }
            else
            {
                animator.SetFloat("Jumping", 1);
            }
        }
        else
        {
            animator.SetFloat("Jumping", 0);
        }

        if (transform.position.y < respawnY)
        {
            Controller.Singleton.Respawn();
        }
    }

    void Aim()
    {
        if (globalMouseAim && currentColor != PaintColor.Yellow) 
        {
            MouseAim();
        }
        else
        {
            DirectionAim();
        }
    }

    void DirectionAim()
    {
        model.transform.rotation = ModelRotation;
        AimRotation = ModelRotation;
    }

    void MouseAim()
    {
        Vector3 aim = camera.transform.forward;
        if (aim.y<0 && onGround)
        {
            aim.y = 0;
        }
        AimRotation = Quaternion.LookRotation(aim);
        aim.y=0;
        ModelRotation = Quaternion.LookRotation(aim);
        model.transform.rotation = ModelRotation;
        horizontal = 0; vertical = 0;
    }


    void FixedUpdate()
    {
        Vector3 target = new Vector3(horizontal, 0f, vertical);
        target = camera.transform.TransformDirection(target);
        target.y = 0f;
        Vector3 result = target * Speed;
        
        if(hurt||dashing)
        {
            result = knockback;
        }
        else if (horizontal != 0 || vertical != 0)
        {
            ModelRotation = Quaternion.LookRotation(target, Vector3.up);
        }

        if(!dashing)
        {
            result.y = rigidbody.velocity.y;
        }
        rigidbody.velocity = result;


        Quaternion newrot = Quaternion.Slerp(model.transform.rotation, ModelRotation, RotationInterpolateSpeed * Time.deltaTime);
        model.transform.rotation = newrot;

        RotateCamera(mouseX, mouseY);
    }

    void RotateCamera(float movex, float movey)
    {
        cameraBase.transform.RotateAround(cameraBase.transform.position, new Vector3(0, 1, 0), movex * CameraMouseRotationSpeed);
        cameraXRot += -movey * CameraMouseRotationSpeed;
        cameraXRot = Mathf.Clamp(cameraXRot, CameraXRotMin, CameraXRotMax);
        Vector3 rot = cameraPivot.transform.rotation.eulerAngles;
        rot.x = cameraXRot;
        cameraPivot.transform.rotation = Quaternion.Euler(rot);
    }

    void Dash() 
    {
        animator.SetBool("Dash", true);
        model.transform.rotation = ModelRotation;
        knockback = model.transform.forward * dashSpeed;
    }

    public void UpdateColorInfo()
    {
        Material[] mats = modelMaterials.materials;
        mats[1] = colorMaterials[(int)currentColor];
        modelMaterials.materials = mats;

        if(bucketMaterials==null)return;
        Material[] mats2 = bucketMaterials.materials;
        if(mats2.Length==0)return;
        mats2[1] = colorMaterials[(int)currentColor];
        bucketMaterials.materials = mats2;
    }

    public void Attack()
    {
        Controller.Singleton.PlaySoundOneShot(paintSound, Random.Range(0.9f, 1.1f));
        GameObject obj;
        switch (currentColor)
        {
            case PaintColor.Red:
                obj = paintGlobsRed;
                break;
            case PaintColor.Blue:
                obj = paintGlobBlue;
                break;
            case PaintColor.Yellow:
                obj = paintGlobsYellow;
                break;
            default:
                obj = paintGlobsRed;
                break;
        }

        var glob = Instantiate(obj, transform.position + model.transform.forward * 1.5f, AimRotation);

        foreach (PaintGlob comp in glob.GetComponentsInChildren<PaintGlob>())
        {
            comp.Color = currentColor;
        }

        Destroy(glob.gameObject, 3.0f);
    }
}
