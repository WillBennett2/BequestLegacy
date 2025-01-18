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

        public float damage;
        public float attackSpeed;
        public float lifeTime;

        public GameObject skillPrefab;

        public Skills()
        {
            position = Vector2.zero;
            damage = 0f;
            attackSpeed = 0f;
            lifeTime = 0f;
        }

        public abstract void Cast(Vector2 currentPosition, Quaternion currentRotation);
    }
}
