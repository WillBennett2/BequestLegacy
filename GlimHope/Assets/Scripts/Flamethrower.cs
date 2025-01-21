using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Flamethrower : Skills
    {

        private bool isToggled = false;
        private GameObject Flames;
        private GameObject Player;
        [SerializeField] private float offset = 1.0f;
        public Flamethrower()
        {
            SetCost(2.0f);           
        }

        public void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        public void Update()
        {
            if (isToggled)
            {
                DealDamage(Player, GetCost());
            }
        }

        public override void Cast(Vector2 currentPosition, Quaternion currentRotation)
        {

            isToggled = !isToggled;
            if (isToggled)
            {
                Vector2 facingVector = currentRotation * Vector2.up;
                currentPosition = currentPosition + (facingVector * offset);
                Flames = Instantiate(skillPrefab, currentPosition, (currentRotation * Quaternion.Euler(0, 0, 180)));
                Flames.transform.parent = Player.transform;
            }
            else
            {
                Destroy(Flames);
            }
        }

    }
}
