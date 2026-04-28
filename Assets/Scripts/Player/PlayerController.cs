using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public PlayerData playerData;

    private float currentHP;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();


        if (playerData == null)
        {
            Debug.LogError($"PlayerData belum dipasang pada {gameObject.name}! Seret file ScriptableObject-mu ke Inspector.");
            return;
        }

        currentHP = playerData.maxHP;
    }

   
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {

        if (playerData != null)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {

        rb.linearVelocity = moveInput * playerData.moveSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Wall"))
        {
            TakeDamage(0.1f * Time.fixedDeltaTime);
        }
    }

    public void TakeDamage(float dmg)
    {
        if (currentHP <= 0) return; 

        currentHP -= dmg;
        currentHP = Mathf.Max(currentHP, 0);

        if (Time.frameCount % 60 == 0) 
        {
            Debug.Log($"Player HP: {currentHP:F1}");
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("<color=red>Player Dead!</color>");
        
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.LogWarning("GameManager.Instance tidak ditemukan! Pastikan ada GameManager di scene.");
        }

        this.enabled = false; 
        rb.linearVelocity = Vector2.zero;
    }
}