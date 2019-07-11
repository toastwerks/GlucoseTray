﻿using System;
using Dexcom.Fetch.Enums;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Microsoft.Extensions.Configuration;

namespace GlucoseTrayCore
{
    public static class Constants
    {
        public static IConfiguration config { get; set; }
        private static IConfigurationSection AppSettings => config.GetSection("appsettings");
        
        public static FetchMethod FetchMethod => (FetchMethod)Convert.ToInt32(AppSettings["FetchMethod"]);
        public static string NightscoutUrl => AppSettings["NightscoutUrl"];
        public static string DexcomUsername => AppSettings["DexcomUsername"];
        public static string DexcomPassword => AppSettings["DexcomPassword"];
        public static string AccessToken => AppSettings["AccessToken"];
        public static int HighBg => int.Parse(AppSettings["HighBg"]);
        public static int DangerHighBg => int.Parse(AppSettings["DangerHighBg"]);
        public static int LowBg => int.Parse(AppSettings["LowBg"]);
        public static int DangerLowBg => int.Parse(AppSettings["DangerLowBg"]);
        public static int CriticalLowBg => int.Parse(AppSettings["CriticalLowBg"]);
        public static TimeSpan PollingThreshold => TimeSpan.FromSeconds(Convert.ToInt32(AppSettings["PollingThreshold"]));
        public static string ErrorLogPath => AppSettings["ErrorLogPath"];
        public static bool EnableDebugMode => Convert.ToBoolean(AppSettings["EnableDebugMode"]);
        public static LogEventLevel LogLevel => (LogEventLevel)Convert.ToInt32(AppSettings["LogLevel"]);

        public static void LogCurrentConfig(ILogger logger)
        {
            logger.LogDebug($"{nameof(FetchMethod)}: {FetchMethod}");
            logger.LogDebug($"{nameof(NightscoutUrl)}: {NightscoutUrl}");
            logger.LogDebug($"{nameof(DexcomUsername)}: {DexcomUsername}");
            logger.LogDebug($"{nameof(DexcomPassword)}: {DexcomPassword}");
            logger.LogDebug($"{nameof(AccessToken)}: {AccessToken}");
            logger.LogDebug($"{nameof(HighBg)}: {HighBg}");
            logger.LogDebug($"{nameof(DangerHighBg)}: {DangerHighBg}");
            logger.LogDebug($"{nameof(LowBg)}: {LowBg}");
            logger.LogDebug($"{nameof(DangerLowBg)}: {DangerLowBg}");
            logger.LogDebug($"{nameof(CriticalLowBg)}: {CriticalLowBg}");
            logger.LogDebug($"{nameof(PollingThreshold)}: {PollingThreshold}");
            logger.LogDebug($"{nameof(ErrorLogPath)}: {ErrorLogPath}");
            logger.LogDebug($"{nameof(EnableDebugMode)}: {EnableDebugMode}");
            logger.LogDebug($"{nameof(LogLevel)}: {LogLevel}");
        }
    }
}
