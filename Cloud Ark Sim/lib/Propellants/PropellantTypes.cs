using Cloud_Ark_Sim.lib.Propellants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud_Ark_Sim.lib
{
    class PropellantTypes
    {
        public static Dictionary<String, Propellant> propellants = new();

        public static void Init()
        {
            propellants["NitrogenTetroxide"] = new Propellant(0.092011); //kg/mol
            propellants["Aerozine50"] = new Propellant(0.09214); //kg/mol
        }
       
    }
}
