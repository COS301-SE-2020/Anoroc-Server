using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Anoroc_User_Management.Models
{
    public static class RISK
    {
        /// <summary>
        /// Within range of a carrier's location.
        /// </summary>
        public static int HIGH_RISK = 4;
        /// <summary>
        /// Within a cluster's radis.
        /// </summary>
        public static int MEDIUM_RISK = 3;
        /// <summary>
        /// Within range of an old carrier's location.
        /// </summary>
        public static int MODERATE_RISK = 2;
        /// <summary>
        /// Within an old cluster's radius.
        /// </summary>
        public static int LOW_RISK = 1;
        /// <summary>
        /// No risk to the user
        /// </summary>
        public static int NO_RISK = 0;
    }
}
