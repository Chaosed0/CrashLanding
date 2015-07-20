using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
    public int maxHealth = 100;
    private int health = 100;

    public delegate void HealthChanged(int health);
    public event HealthChanged OnHealthChanged;

    public delegate void Died();
    public event Died OnDied;

    void Start() {
        health = maxHealth;
    }

    public int getHealth() {
        return health;
    }

    public void setHealth(int health) {
        this.health = health;
        if (OnHealthChanged != null) {
            OnHealthChanged(health);
        }
        if (this.health <= 0) {
            if (OnDied != null) {
                OnDied();
            }
        } else if (this.health > maxHealth) {
            this.health = maxHealth;
        }
    }

    public void changeHealth(int change) {
        setHealth(health + change);
    }

    public void damage(int damage) {
        setHealth(health - damage);
    }

    public bool isDead() {
        return health <= 0;
    }
}
