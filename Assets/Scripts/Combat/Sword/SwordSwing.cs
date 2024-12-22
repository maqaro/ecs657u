using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordSwing : MonoBehaviour
{
    public GameObject Sword;
    public bool canAttack = true;
    public float Cooldown = 1.0f;
    public float attackRange = 1.5f;
    public float damage = 10f;

    public InputActionAsset inputActionAsset;
    public InputAction swordSwingAction;

    private WeaponAnimation wa;
    private Animator animator;

    void OnEnable()
    {
        // Assuming you have an Input Action called "SwordSwing"
        var gameplayActionMap = inputActionAsset.FindActionMap("Player");
        swordSwingAction = gameplayActionMap.FindAction("SwordSwing");
        swordSwingAction.performed += ctx => OnSwordSwing(); // Add a listener for when the action is performed
        swordSwingAction.Enable(); 
        wa = GetComponent<WeaponAnimation>();
        animator = GetComponentInChildren<Animator>();

    }

    void OnDisable()
    {
        swordSwingAction.Disable(); // Disable when the object is disabled
    }

    // Method that triggers when the sword swing action is performed
    private void OnSwordSwing()
    {
        if (canAttack && animator.GetBool("WeaponUp"))
        {
            Attack();
            wa.SwingAnimation();
        }
    }

    // Method that triggers when the sword swing action is performed
    public void Attack()
    {
        canAttack = false;


        // Check for enemies in the attack range
        Collider[] hitColliders = Physics.OverlapSphere(Sword.transform.position, attackRange);
        foreach (var hitCollider in hitColliders)
        {
            // If the collider is an enemy, apply damage
            if (hitCollider.CompareTag("Enemy"))
            {
                // Get the EnemyAi script and apply damage
                EnemyAi enemyScript = hitCollider.GetComponent<EnemyAi>();
                if (enemyScript != null)
                {
                    // Apply damage to the enemy
                    enemyScript.health -= damage;
                    if (enemyScript.health <= 0)
                    {
                        enemyScript.health = 0;
                    }
                }
            }
        }

        StartCoroutine(ResetAttackCooldown());
    }

    // Coroutine to reset the attack cooldown
    IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(Cooldown);
        canAttack = true;
    }
}
