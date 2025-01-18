using Assets.Scripts;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    private Rigidbody2D rb;
    private Vector2 input;

    private Skills skill1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        skill1 = GetComponent<Flamethrower>();
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

        if (Input.GetKeyDown(KeyCode.Q))
        {
            skill1.Cast(transform.position, transform.rotation);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = input * speed;      
    }

    private void Attack()
    {
        // Wick Attack Here
    }
}
