using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Engine
{
    public class WorldMap
    {
        public List<Location> Zones { get; set; }

        public WorldMap()
        {
            Zones = new List<Location>();
        }

        // Add a Location to the WorldMap, if it has not already been added
        public void AddLocation(Location place)
        {
            if (!Zones.Contains(place))
                Zones.Add(place);
        }

        public bool HasLocation(Location place)
        {
            return Zones.Contains(place);
        }
        
    }
}
