using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Entity : MonoBehaviour
{
    [SerializeField]
    Slider hpSlider;
    public Action<Entity> onDestroy;

    int hp = 10;
    int maxHP = 10;
    int damage = 2;
    float attackCD = 0.8f;
    float currentCD = 0.0f;

    float attackRange = 1f;
    public float getAttackRange() { return attackRange; }
    float visionRange = 3.5f;
    public float getVisionRange() { return visionRange; }
    float movementSpeed = 0.2f;
    public float getMovementSpeed() { return movementSpeed; }

    private void Start()
    {
        // hpSlider.maxValue = maxHP;
        // hpSlider.value = hp;
        // hpSlider.interactable = false;
    }

    private void FixedUpdate()
    {
        if (currentCD > 0)
        {
            currentCD -= Time.fixedDeltaTime;
            if (currentCD < 0)
                currentCD = 0;
        }
    }
    void Die()
    {
        onDestroy(this);
        GameObject.Destroy(this, 0.2f);
    }
    public bool Attack(Entity entity)
    {
        if (currentCD == 0)
        {
            entity.ReduceHP(damage);
            currentCD = attackCD;
            return true;
        }
        return false;
    }

    public float GetCD() { return currentCD; }

    void ReduceHP(int amount)
    {
        hp -= amount;
        //hpSlider.value = hp;
        if (hp < 0)
            Die();
    }
    public void RestoreHP()
    {
        hp = maxHP;
    }
}
