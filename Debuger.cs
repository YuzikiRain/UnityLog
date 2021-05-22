using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace BordlessFramework
{
    /// <summary>
    /// 调试消息打印器
    /// <para>使用宏 BORDLESSFRAMEWORK_DEBUGER_DISABLE 关闭所有消息打印  </para>
    /// </summary>
    public static class Debuger
    {
        private const string defaultGroupName = "Default";
        private static Dictionary<string, bool> logGroups = new Dictionary<string, bool>();
        private static Dictionary<string, bool> logWarningGroups = new Dictionary<string, bool>();
        private static Dictionary<string, bool> logErrorGroups = new Dictionary<string, bool>();
        private static readonly Color whiteColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        private static readonly Color blackColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        private static Color defaultColor = blackColor;

        public static void SetDefaultColor(Color color) { defaultColor = color; }

        /// <summary>
        /// 使对应group的Log有效
        /// </summary>
        /// <param name="group"></param>
        /// 
        public static void EnableLogGroup(string group)
        {
            logGroups[group] = true;
        }

        /// <summary>
        /// 使对应group的Log无效
        /// </summary>
        /// <param name="group"></param>
        public static void DisableLogGroup(string group)
        {
            logGroups[group] = false;
        }

        /// <summary>
        /// 使对应group的LogError有效
        /// </summary>
        /// <param name="group"></param>
        public static void EnableLogErrorGroup(string group)
        {
            logErrorGroups[group] = true;
        }

        /// <summary>
        /// 使对应group的LogError无效
        /// </summary>
        /// <param name="group"></param>
        public static void DisableLogErrorGroup(string group)
        {
            logErrorGroups[group] = false;
        }

        /// <summary>
        /// 使对应group的LogWarning有效
        /// </summary>
        /// <param name="group"></param>
        public static void EnableLogWarningGroup(string group)
        {
            logWarningGroups[group] = true;
        }

        /// <summary>
        /// 使对应group的LogWarning无效
        /// </summary>
        /// <param name="group"></param>
        public static void DisableLogWarningGroup(string group)
        {
            logWarningGroups[group] = false;
        }

        /// <summary>
        /// 启用所有Log、LogWarning、LogError
        /// </summary>
        public static void EnableAll()
        {
            foreach (var group in logGroups) { logGroups[group.Key] = true; }
            foreach (var group in logWarningGroups) { logWarningGroups[group.Key] = true; }
            foreach (var group in logErrorGroups) { logErrorGroups[group.Key] = true; }
        }

        /// <summary>
        /// 禁用所有Log、LogWarning、LogError
        /// </summary>
        public static void DisableAll()
        {
            foreach (var group in logGroups) { logGroups[group.Key] = false; }
            foreach (var group in logWarningGroups) { logWarningGroups[group.Key] = false; }
            foreach (var group in logErrorGroups) { logErrorGroups[group.Key] = false; }
        }

        /// <summary>
        /// 打印消息
        /// </summary>
        /// <param name="message">消息</param>
        [Conditional("ENABLE_BORDLESSFRAMEWORK_DEBUGER")]
        public static void Log(object message, params DebugerOption[] debugerOptions)
        {
            Log(message, defaultGroupName, debugerOptions);
        }

        /// <summary>
        /// 打印消息，分组至<paramref name="groupName"/>
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="groupName">被划分到的组</param>
        [Conditional("ENABLE_BORDLESSFRAMEWORK_DEBUGER")]
        public static void Log(object message, string groupName, params DebugerOption[] debugerOptions)
        {
            if (!logGroups.TryGetValue(groupName, out var isEnable)) EnableLogGroup(groupName);
            if (!isEnable) return;

            string result = message.ToString();
            foreach (var debugerOption in debugerOptions) result = WrapMessageWithDebugerOption(result, debugerOption);
            UnityEngine.Debug.Log(result);
        }

        /// <summary>
        /// 打印警告消息
        /// </summary>
        /// <param name="message">消息</param>
        [Conditional("ENABLE_BORDLESSFRAMEWORK_DEBUGER")]
        public static void LogWarning(object message, params DebugerOption[] debugerOptions)
        {
            LogWarning(message, defaultGroupName, debugerOptions);
        }

        /// <summary>
        /// 打印警告消息，分组至<paramref name="groupName"/>
        /// </summary>
        /// <param name="message">警告消息</param>
        /// <param name="groupName">被划分到的组</param>
        [Conditional("ENABLE_BORDLESSFRAMEWORK_DEBUGER")]
        public static void LogWarning(object message, string groupName, params DebugerOption[] debugerOptions)
        {
            if (!logWarningGroups.TryGetValue(groupName, out var isEnable)) EnableLogWarningGroup(groupName);
            if (!isEnable) return;

            string result = message.ToString();
            foreach (var debugerOption in debugerOptions) result = WrapMessageWithDebugerOption(result, debugerOption);
            UnityEngine.Debug.LogWarning(result);
        }

        /// <summary>
        /// 打印错误消息
        /// </summary>
        /// <param name="message">消息</param>
        [Conditional("ENABLE_BORDLESSFRAMEWORK_DEBUGER")]
        public static void LogError(object message, params DebugerOption[] debugerOptions)
        {
            LogError(message, defaultGroupName, debugerOptions);
        }

        /// <summary>
        /// 打印错误消息，分组至<paramref name="groupName"/>
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="groupName">被划分到的组</param>
        [Conditional("ENABLE_BORDLESSFRAMEWORK_DEBUGER")]
        public static void LogError(object message, string groupName, params DebugerOption[] debugerOptions)
        {
            if (!logErrorGroups.TryGetValue(groupName, out var isEnable)) EnableLogErrorGroup(groupName);
            if (!isEnable) return;

            string result = message.ToString();
            foreach (var debugerOption in debugerOptions) result = WrapMessageWithDebugerOption(result, debugerOption);
            UnityEngine.Debug.LogError(result);
        }

        /// <summary>
        /// 根据 <paramref name="debugerOption"/> 依次对<paramref name="message"/> message进行处理
        /// </summary>
        /// <param name="message"></param>
        /// <param name="debugerOption"></param>
        /// <returns></returns>
        internal static string WrapMessageWithDebugerOption(string message, DebugerOption debugerOption)
        {
            switch (debugerOption.type)
            {
                case DebugerOption.DebugerOptionType.Color:
                    message = WrapMessageWithColor(message, debugerOption.color.Value);
                    break;
                case DebugerOption.DebugerOptionType.Bold:
                    message = WrapMessageWithBold(message);
                    break;
                case DebugerOption.DebugerOptionType.Italic:
                    message = WrapMessageWithItalic(message);
                    break;
                default:
                    break;
            }
            return message;
        }

        /// <summary>
        /// 颜色
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private static string WrapMessageWithColor(string message, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>";
        }

        /// <summary>
        /// 加粗
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string WrapMessageWithBold(string message)
        {
            return $"<b>{message}</b>";
        }

        /// <summary>
        /// 斜体
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static string WrapMessageWithItalic(string message)
        {
            return $"<i>{message}</i>";
        }

        private static MethodInfo clearMethod;

        /// <summary>
        /// 清空控制台消息
        /// </summary>
        public static void ClearConsole()
        {
            if (clearMethod == null)
            {
                var logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
                clearMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
            }
            clearMethod.Invoke(null, null);
        }
    }
}