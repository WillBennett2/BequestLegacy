using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Explosion : MonoBehaviour
    {
        public GameObject Radius;

        public void OnDestroy()
        {
            Radius = Instantiate(Radius, transform.position, Quaternion.identity);
            Destroy(Radius, 1.0f);
        }
    }
}
