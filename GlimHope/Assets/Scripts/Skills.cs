using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal abstract class Skills : MonoBehaviour
    {
        public Vector2 position;

        [SerializeField] private float damage;
        [SerializeField] private float attackSpeed;
        [SerializeField] private float lifeTime;
        [SerializeField] private float cost;

        public GameObject skillPrefab;

        public Skills()
        {
            position = Vector2.zero;
            damage = 0f;
            attackSpeed = 0f;
            lifeTime = 0f;
        }

        public abstract void Cast(Vector2 currentPosition, Quaternion currentRotation);

        public void AddLifeTime(float inputTime)
        {
            lifeTime += inputTime;
        }

        public float GetLifeTime() { return lifeTime; }

        public void SetCost(float inputCost) { cost = inputCost; }
        public float GetCost() { return cost; }

        public void SetDamage(float inputdamage) { damage = inputdamage; }
        public float GetDamage() { return damage; }

        public void DealDamage(GameObject target,  float damage)
        {
            target.SendMessage("TakeDamage", damage);
        }
    }
}
