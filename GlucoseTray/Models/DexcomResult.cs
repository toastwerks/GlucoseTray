﻿namespace GlucoseTray.Models
{
    /// <summary>
    /// Class that maps to the JSON received from DexCom queries.
    /// </summary>
    public class DexcomResult
    {
        public string ST { get; set; }
        public string DT { get; set; }
        public string Trend { get; set; }
        public double Value { get; set; }
        public string WT { get; set; }
    }
}
