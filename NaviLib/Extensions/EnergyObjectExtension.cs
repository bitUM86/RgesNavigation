﻿using NaviLib.MyTypes;
using System.Runtime.CompilerServices;

namespace RgesNaviApi.Extensions
{
    public static class EnergyObjectExtension
    {

        public static bool IsImhoEqual(this EnergyObject eo, EnergyObject energyObject  )
        {
              return eo.Name == energyObject.Name
                  && eo.District == energyObject.District
                  && eo.EnergyObjectType == energyObject.EnergyObjectType;
               
        }
    }
}
