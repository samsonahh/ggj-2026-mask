// dylan's version of player movement, disregard. 

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool isPlayerOne = true;

    public float moveSpeed = 5f;
    
    public float attackRange = 2f;
    public int attackDamage = 10;

    Rigidbody rb;
    Vector3 lastMoveDir = Vector3.forward;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        float x = 0f;
        float z = 0f;

        if (isPlayerOne)
        {
            if (Input.GetKey(KeyCode.A)) x = -1;
            if (Input.GetKey(KeyCode.D)) x = 1;
            if (Input.GetKey(KeyCode.W)) z = 1;
            if (Input.GetKey(KeyCode.S)) z = -1;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) x = -1;
            if (Input.GetKey(KeyCode.RightArrow)) x = 1;
            if (Input.GetKey(KeyCode.UpArrow)) z = 1;
            if (Input.GetKey(KeyCode.DownArrow)) z = -1;
        }

        Vector3 move = new Vector3(x, 0f, z).normalized;

        if (move != Vector3.zero)
        {
            lastMoveDir = move; // stores in which direction player last faced
        }

        rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);
    }

    void HandleAttack()
    {
        if (isPlayerOne && Input.GetKeyDown(KeyCode.Alpha1))
            Attack();

        if (!isPlayerOne && Input.GetKeyDown(KeyCode.Alpha2))
            Attack();
    }

    void Attack()
    {
        Vector3 origin = transform.position + Vector3.up * 0.8f;
        float radius = 0.5f;

        RaycastHit hit;
        if (Physics.SphereCast(origin, radius, lastMoveDir, out hit, attackRange))
        {
            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }
        }
    }
}