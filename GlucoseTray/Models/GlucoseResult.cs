﻿using GlucoseTray.Enums;
using System;

namespace GlucoseTray.Models
{
    public class GlucoseResult
    {
        public int Id { get; set; }
        public int MgValue { get; set; }
        public double MmolValue { get; set; }
        public DateTime DateTimeUTC { get; set; }
        public TrendResult Trend { get; set; }
        public bool WasError { get; set; }
        public FetchMethod Source { get; set; }
        public bool IsCriticalLow { get; set; }
    }
}
