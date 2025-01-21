using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Fireball : Skills
    {
        public float flySpeed = 5.0f;
        private GameObject Orb;

        [SerializeField] private float offset = 1.0f;
        public Fireball()
        {
            AddLifeTime(1.0f);
            SetCost(30.0f);
        }

        public override void Cast(Vector2 currentPosition, Quaternion currentRotation)
        {
            DealDamage(GameObject.FindGameObjectWithTag("Player"), GetCost());

            Vector2 facingVector = currentRotation * Vector2.up;
            currentPosition = currentPosition + (facingVector * offset);
            Orb = Instantiate(skillPrefab, currentPosition, currentRotation);
            Orb.GetComponent<Rigidbody2D>().AddForce(facingVector * flySpeed);
            Destroy(Orb, GetLifeTime());
        }

    }
}
