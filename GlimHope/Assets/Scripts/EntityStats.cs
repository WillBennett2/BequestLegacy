using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] public float healthBurn;
    [SerializeField] public float spellDamage;
    [SerializeField] public float armour;
    [SerializeField] public float movementSpeed;
    [SerializeField] public float magicCritDamage;
    [SerializeField] public float magicCritChance;
    [SerializeField] public float attackSpeed;


    public void UpdateHealthValue(float changeValue)
    {
        health += changeValue;
    }
    public void UpdateMaxHealthValue(int changeValue)
    {
        maxHealth += changeValue;
    }
    public void UpdateHealthBurnValue(int changeValue)
    {
        healthBurn += changeValue;
    }
    public void UpdateSpellDamageValue(int changeValue)
    {
        spellDamage += changeValue;
    }
    public void UpdateArmourValue(int changeValue)
    {
        armour += changeValue;
    }
    public void UpdateMovementSpeedValue(int changeValue)
    {
        movementSpeed += changeValue;
    }
    public void UpdateMagicCritDamageValue(int changeValue)
    {
        magicCritDamage += changeValue;
    }
    public void UpdateMagicCritChanceValue(int changeValue)
    {
        magicCritChance += changeValue;
    }
    public void UpdateAttackSpeedValue(int changeValue)
    {
        attackSpeed += changeValue;
    }

}
