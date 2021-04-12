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
            var random = new System.Random();
            return random.Next(0, 359);
        }

        private float GetWindSpeed()
        {
            var random = new System.Random();
            return random.Next(5, 12);
        }

        public void Start()
        {

            windDirection.x = 0;
            windDirection.y = 0;
            windDirection.z = GetWindDirection();

            windSpeed = GetWindSpeed();

        }

        public Tuple<Vector3, float> GetWindData()
        {
            var windData = Tuple.Create(windDirection, windSpeed);
            return windData;
        }




    }
}
