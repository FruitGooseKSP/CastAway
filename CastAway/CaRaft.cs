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


        public bool anchorIsDown;
        private bool windIsActive;
        private Vector3 windDirection;
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
            windDir = windDirection.z;
            windSp = windSpeed;
            windIsActive = true;
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
                    Tuple<Vector3, float> windDat = windHandler.GetWindData();
                    windDirection = windDat.Item1;
                    windSpeed = windDat.Item2;
                    SetWindFields();
                }


            }



        }








    }
}
