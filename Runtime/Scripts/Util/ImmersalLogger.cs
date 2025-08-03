/*===============================================================================
Copyright (C) 2024 Immersal - Part of Hexagon. All Rights Reserved.

This file is part of the Immersal SDK.

The Immersal SDK cannot be copied, distributed, or made available to
third-parties for commercial purposes without written permission of Immersal Ltd.

Contact sales@immersal.com for licensing requests.
===============================================================================*/

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;
using UnityEngine;

namespace Immersal
{
    public class ImmersalLogger : MonoBehaviour
    {
        public enum LoggingLevel
        {
            All = 0,
            Verbose = 1,
            ErrorsAndWarnings = 2,
            ErrorsOnly = 3,
            None = 4
        }
        
        public static LoggingLevel Level = LoggingLevel.All;

        // File logging
        private static bool m_LogToFile = false;
        private static StreamWriter m_FileWriter;
        private static readonly object m_FileLock = new object();

        private const bool m_IncludeCallerName = true;
        private const string m_AdditionalPrefix = "";

        // Include calling class name in logging messages
        private static string ProcessMessage(string message, string filePath = "")
        {
            if (m_IncludeCallerName)
            {
                string callerName;
                
                // check if filePath is provided (by System.Runtime.CompilerServices)
                if (!string.IsNullOrEmpty(filePath))
                {
                    callerName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                }
                else
                {
                    // fall back to fetching calling class name from stack with reflection
                    // note: will give unexpected results when call originates in an async Task
                
                    callerName = new StackFrame(2, false).GetMethod().DeclaringType?.Name
                                 ?? "Unknown";
                }
                
                message = $"[{callerName}] {message}";
            }

            return $"{m_AdditionalPrefix}{message}";
        }
        
        // note: filePath parameter is automagically included by System.Runtime.CompilerServices.CallerFilePath

        public static void Log(string message, LoggingLevel messageLevel = LoggingLevel.All, [CallerFilePath] string filePath = "")
        {
            if (Level > messageLevel) return;
            string processed = ProcessMessage(message, filePath);
            UnityEngine.Debug.Log(processed);
            WriteToFile(processed);
        }

        public static void LogWarning(string message,  [CallerFilePath] string filePath = "")
        {
            if (Level > LoggingLevel.ErrorsAndWarnings) return;
            string processed = ProcessMessage(message, filePath);
            UnityEngine.Debug.LogWarning(processed);
            WriteToFile(processed);
        }

        public static void LogError(string message, [CallerFilePath] string filePath = "")
        {
            if (Level > LoggingLevel.ErrorsOnly) return;
            string processed = ProcessMessage(message, filePath);
            UnityEngine.Debug.LogError(processed);
            WriteToFile(processed);
        }

        public static void EnableFileLogging(bool enable, string filePath)
        {
            lock (m_FileLock)
            {
                if (enable)
                {
                    try
                    {
                        m_FileWriter = new StreamWriter(filePath, true);
                        m_FileWriter.AutoFlush = true;
                        m_LogToFile = true;
                    }
                    catch (Exception e)
                    {
                        m_LogToFile = false;
                        UnityEngine.Debug.LogError($"Failed to enable file logging: {e.Message}");
                    }
                }
                else
                {
                    m_LogToFile = false;
                    if (m_FileWriter != null)
                    {
                        m_FileWriter.Flush();
                        m_FileWriter.Close();
                        m_FileWriter = null;
                    }
                }
            }
        }

        private static void WriteToFile(string message)
        {
            if (!m_LogToFile || m_FileWriter == null) return;
            lock (m_FileLock)
            {
                m_FileWriter.WriteLine($"{DateTime.Now:O} {message}");
            }
        }
    }
}

