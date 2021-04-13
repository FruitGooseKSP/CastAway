using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CastAway
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class WindHandler : MonoBehaviour
    {
        private Vector3 windDirection = new Quaternion().eulerAngles;
        private float windSpeed;


        private float GetWindDirection()
        {
            System.Random random = new System.Random();
            return random.Next(0, 359);
        }

        private float GetWindSpeed()
        {
            System.Random random = new System.Random();
            return random.Next(5, 12);
        }

        public void Start()
        {

            windDirection.x = 0;
            windDirection.y = 0;
            windDirection.z = GetWindDirection();

            windSpeed = GetWindSpeed();

        }

        public float GetWindDir()
        {
            return windDirection.z;
        }

        public float GetWindSp()
        {
            return windSpeed;
        }




    }
}
