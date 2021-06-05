using UnityEngine;
using UnityEngine.UI;
using System;

public class Entity : MonoBehaviour {
    [SerializeField] Slider hpSlider;
    public Action<Entity> OnDestroy;

    int _hp = 10;
    const int MaxHp = 10;
    const int Damage = 2;
    const float AttackCd = 0.8f;
    public float CurrentCd { get; private set; } = 0.0f;

    public float AttackRange { get; } = 1f;

    public float VisionRange { get; } = 3.5f;

    public float MovementSpeed { get; } = 0.2f;

    private void Start() {
        // hpSlider.maxValue = maxHP;
        // hpSlider.value = hp;
        // hpSlider.interactable = false;
    }

    void FixedUpdate() {
        if (CurrentCd == 0) {
            return;
        }

        CurrentCd -= Time.fixedDeltaTime;
        if (CurrentCd < 0) {
            CurrentCd = 0;
        }
    }

    void Die() {
        OnDestroy(this);
        Destroy(this);
    }

    public bool Attack(Entity entity) {
        if (CurrentCd != 0) {
            return false;
        }

        entity.ReduceHp(Damage);
        CurrentCd = AttackCd;
        return true;
    }

    void ReduceHp(int amount) {
        _hp -= amount;
        //hpSlider.value = hp;
        if (_hp < 0) {
            Die();
        }
    }

    public void RestoreHp() {
        _hp = MaxHp;
    }
}
