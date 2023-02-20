using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neutronium.PostProcessing.LUT
{
    /// <summary>
    /// Enumerates the LUT variants supported by the game.
    /// </summary>
    public enum LUTSlot
    {
        /// <summary>
        /// A pseudo-slot that allows a LUT to be applied all the time.
        /// </summary>
        All,
        /// <summary>
        /// The LUT used during the day.
        /// </summary>
        Day,
        /// <summary>
        /// The LUT used during the night.
        /// </summary>
        Night
    }
}
