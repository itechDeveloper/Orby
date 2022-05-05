using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCurser : MonoBehaviour
{
    // Control speed
    private float speed;
    public float accelaration;
    public float maxAttackSpeed;
    public float maxRetrieveSpeed;
    public float followSpeed;

    // Control target
    private Vector3 target;
    public Transform player;

    // Control direction
    bool followPlayer;
    bool attacking;
    bool moving;
    bool retrieving;

    // Control reaching to target
    bool reachedTarget;
    public float retrieveDistance;
    public float reachedTargetDistance;
    float totalDistance;
    float currentDistance;

    // Cooldown
    private float coolDownTimer;
    public float coolDown;

    void Start()
    {
        followPlayer = true;
    }

    void Update()
    {
        if (!PlayerHealthSystem.dead)
        {
            FollowPlayer();
            Retrieve();
            Attack();
            if (!reachedTarget)
            {
                CheckDistance();
                AddAccelaration();
                transform.position = Vector3.MoveTowards(transform.position, target, -speed * Time.deltaTime);
            }
        }
    }

    void FollowPlayer()
    {
        if (followPlayer && !moving)
        {
            target = player.position;
            target.z = transform.position.z;
            speed = followSpeed;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            reachedTarget = true;
        }
    }

    void Retrieve()
    {
        if (Input.GetKeyDown(KeyCode.R) || (Vector2.Distance(transform.position, player.position) > retrieveDistance && !moving))
        {
            followPlayer = false;
            retrieving = true;
        }

        if (retrieving && !moving)
        {
            target = player.position;
            target.z = transform.position.z;

            speed = Mathf.Clamp(speed, 0, maxRetrieveSpeed);
            totalDistance = Mathf.Abs(Vector2.Distance(transform.position, target));

            reachedTarget = false;
            moving = true;
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Q) && coolDownTimer <= 0)
        {
            retrieving = false;
            followPlayer = false;
            attacking = true;
            coolDownTimer = coolDown;
        }
        else if (coolDown > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if (attacking && !moving)
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;

            speed = Mathf.Clamp(speed, 0, maxAttackSpeed);
            totalDistance = Mathf.Abs(Vector2.Distance(transform.position, target));

            reachedTarget = false;
            moving = true;
        }
    }

    void CheckDistance()
    {
        currentDistance = Vector2.Distance(transform.position, target);

        if (Vector2.Distance(transform.position, target) < reachedTargetDistance)
        {
            if (retrieving)
            {
                followPlayer = true;
            }
            
            reachedTarget = true;
            attacking = false;
            retrieving = false;
            speed = 0;
            moving = false;
        }
    }

    void AddAccelaration()
    {
        if (currentDistance < 1/3 * totalDistance)
        {
            speed += accelaration * Time.deltaTime;
        }
        else
        {
            speed -= 2 * accelaration * Time.deltaTime;
        }
    }
}