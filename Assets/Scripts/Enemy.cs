using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 10; // Enemy's health

    public void TakeDamage(int damage) // Method to apply damage to the enemy
    {
        health -= damage;
        if (health <= 0)
        {
           Destroy(gameObject);
        }
    }

}
