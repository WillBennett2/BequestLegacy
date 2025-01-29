using Assets.Scripts;
using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static event Action OnPurchaseItem;
    public static event Action OnShowInventory;

    private EntityStats stats;

    //[SerializeField] private float speed = 10.0f;
    private Rigidbody2D rb;
    private Vector2 input;

    private Skills skill1;
    private Skills skill2;

    //[SerializeField] private float maxHealth = 500.0f;
    //private float currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = GetComponent<EntityStats>();
        rb = GetComponent<Rigidbody2D>();

        skill1 = GetComponent<Flamethrower>();
        skill2 = GetComponent<Fireball>();

        //currentHealth = stats.maxHealth;
        Debug.Log("health: " + stats.health);
    }

    // Update is called once per frame
    void Update()
    {
        #region movement input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();
        #endregion

        #region face the mouse
        Vector2 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);

        transform.up = direction;
        #endregion

        #region player actions
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnPurchaseItem?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.I)|| Input.GetKeyDown(KeyCode.Tab))
        {
            OnShowInventory?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            skill1.Cast(transform.position, transform.rotation);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            skill2.Cast(transform.position, transform.rotation);
        }
        #endregion


        #region dead
        if (stats.health < 0)
        {
            Dead();
        }
        #endregion
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * stats.movementSpeed;      
    }

    private void Attack()
    {
        // Wick Attack Here
    }

    public void TakeDamage(float damage)
    {
        stats.UpdateHealthValue( damage);
        Debug.Log("health: " + stats.health);
    }

    private void Dead()
    {
        //death here
    }
}
