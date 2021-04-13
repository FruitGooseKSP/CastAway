using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CastAway
{
    public class CaRaft : PartModule
    {
        [KSPEvent(isPersistent = false, active = true, guiActive = true, guiActiveEditor = false, guiName = "Raise Anchor")]
        public void AnchorToggle()
        {
            if (anchorIsDown)
            {
                anchorIsDown = false;
                Events["AnchorToggle"].guiName = "Lower Anchor";
                
            }
            else
            {
                anchorIsDown = true;
                Events["AnchorToggle"].guiName = "Raise Anchor";
                
            }

        }

        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Wind Direction")]
        public float windDir;
        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Wind Speed")]
        public float windSp;
        [KSPField(isPersistant = false, guiActive = true, guiActiveEditor = false, guiName = "Boat Heading")]
        public float boatHeading;


        public bool anchorIsDown;
        public Vector3 boatPos;
        public float windMultiplier;
        public Rigidbody rB;
        public KeyBinding leftKey;
        public KeyBinding rightKey;
        private bool windIsActive;
        private float windDirection;
        private float windSpeed;

        public bool LookForSail()
        {
            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                if (part.HasModuleImplementing<CaSail>())
                {
                    return true;
                }
            }
            return false;
        }

        public void SetWindFields()
        {
            windDir = windDirection;
            windSp = windSpeed;
            windIsActive = true;
        }

        public float GetWindX()
        {
            float boatX = 0;
            float windX = 0;

            if (windDir <= 180)
            {
                windDir = 180 - windDir;                    //  0   =   180-0=180
            }
            else
            {
                windDir -= 180;
            }
            if (boatHeading <= 180)
            {
                boatHeading = 180 - boatHeading;        // 5 = 180-5 = 175     or   355, 355-180 = 175     or  150, 180 - 150 = 30
            }
            else
            {
                boatHeading -= 180;
            }

            float product;

            if (windDir >= boatHeading)
            {
                product = windDir - boatHeading;        // w 20, bh 1, 20-1 = 19        or w 140, bh 10 = 130
            }
            else
            {
                product = boatHeading - windDir;        // w 5, bh 10, 10 - 5 = 5
            }

            if (product == 180)
            {
                boatX = 0;
            }
            else if (product < 180 && product >= 160)
            {
                boatX = 10;
            }
            else if (product < 160 && product >= 140)
            {
                boatX = 20;
            }
            else if (product < 140 && product >= 120)
            {
                boatX = 30;
            }
            else if (product < 120 && product >= 100)
            {
                boatX = 40;
            }
            else if (product < 100 && product >= 80)
            {
                boatX = 50;
            }
            else if (product < 80 && product >= 60)
            {
                boatX = 65;
            }
            else if (product < 60 && product >= 40)
            {
                boatX = 75;
            }
            else if (product < 40 && product >= 20)
            {
                boatX = 80;
            }
            else if (product < 20 && product >= 0)
            {
                boatX = 100;
            }

            windX = (boatX * windSp) / 10;
            return windX;

        }

        public void SetMasses()
        {
            foreach (var part in FlightGlobals.ActiveVessel.Parts)
            {
                part.mass = 0.001F;
                part.buoyancy = 1.75f;
            }
        }

        public void Start()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                anchorIsDown = true;
                windIsActive = false;
                bool g2g = LookForSail();

                if (!g2g)
                {
                    Events["AnchorToggle"].active = false;
                }
                else
                {
                    Events["AnchorToggle"].active = true;
                    WindHandler windHandler = new WindHandler();
                    windDirection = windHandler.GetWindDir();

                    Debug.LogError("windDirection = " + windDirection);

                    windSpeed = windHandler.GetWindSp();

                    Debug.LogError("windSpeed = " + windSpeed);
                    SetWindFields();
                    rB = FlightGlobals.ActiveVessel.GetComponent<Rigidbody>();
                    leftKey = GameSettings.YAW_LEFT;
                    rightKey = GameSettings.YAW_RIGHT;
                    SetMasses();
                }


            }



        }

        public void FixedUpdate()
        {
            if (HighLogic.LoadedSceneIsFlight)
            {
                if (!anchorIsDown && windIsActive && FlightGlobals.ActiveVessel.Splashed)
                {
                    Debug.LogError("argument 1 entered");

                    boatPos = FlightGlobals.ActiveVessel.transform.forward.normalized;

                    Debug.LogError("boatPos = " + boatPos);
                    
                    boatHeading = boatPos.z;

                    Debug.LogError("boatHeading = " + boatHeading);

                    rB = FlightGlobals.ActiveVessel.GetComponent<Rigidbody>();

                    Debug.LogError("rB assigned");

                    windMultiplier = GetWindX();

                    Debug.LogError("windMult = " + windMultiplier);


                    rB.velocity = transform.forward * windMultiplier;

                    Debug.LogError("rB.velocity = " + rB.velocity);


                    if (leftKey.GetKey(false))
                    {
                        FlightGlobals.ActiveVessel.transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * 10);
                        Debug.LogError("left");
                    }
                    if (rightKey.GetKey(false))
                    {
                        FlightGlobals.ActiveVessel.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 10);
                        Debug.LogError("right");
                    }

                }

                else if (anchorIsDown && FlightGlobals.ActiveVessel.checkSplashed())
                {
                    rB.velocity = Vector3.zero;
                }








            }



        }








    }
}
