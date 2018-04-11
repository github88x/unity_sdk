#if (UNITY_ANDROID || UNITY_IOS)
using System;
#endif
using UnityEngine;

namespace com.adjust.sdk.test
{
    public class TestApp : MonoBehaviour
    {
        public static readonly string TAG = "[TestApp]";
        private static bool _isLaunched = false;

#if (UNITY_WSA || UNITY_WP8)
        public const string CLIENT_SDK = "unity4.12.0@wuap4.12.0";
        private const string PORT = ":8080";
        private const string PROTOCOL = "http://";
        private const string BASE_URL = PROTOCOL + "localhost" + PORT;        // Windows simulator
#elif UNITY_ANDROID
        public const string CLIENT_SDK = "unity4.12.5@android4.12.4";
        private const string PORT = ":8443";
        private const string PROTOCOL = "https://";
        private const string BASE_URL = PROTOCOL + "10.0.2.2" + PORT;          // Android simulator
#elif UNITY_IOS
        public const string CLIENT_SDK = "unity4.12.5@ios4.12.3";
        private const string PORT = ":8080";
        private const string PROTOCOL = "http://";
        private const string BASE_URL = PROTOCOL + "127.0.0.1" + PORT;            // iOS simulator
        // private const string BASE_URL = PROTOCOL + "192.168.8.141" + PORT;    // Over WiFi

        private TestFactoryIOS _testFactoryiOS;
#else
        public const string CLIENT_SDK = null;
#endif
        // private const string BASE_URL = PROTOCOL + "10.0.2.2" + PORT;           // Android simulator
        // private const string BASE_URL = PROTOCOL + "localhost" + PORT;          // Windows simulator
        // private const string BASE_URL = PROTOCOL + "192.168.8.141" + PORT;      // Over WiFi
        // private const string BASE_URL = PROTOCOL + "127.0.0.1" + PORT;          // iOS simulator

        void OnGUI()
        {
            if (_isLaunched)
            {
                return;
            }

            ITestFactory testFactory = GetPlatformSpecificTestLibrary ();

            #if UNITY_IOS
            _testFactoryiOS = testFactory as TestFactoryIOS;
            #endif

            // Set specific tests to run.
            // testFactory.AddTest("current/eventBuffering/Test_EventBuffering_sensitive_packets");

            Log ("Starting test session...");
            testFactory.StartTestSession();
            _isLaunched = true;
        }

        private ITestFactory GetPlatformSpecificTestLibrary()
        {
#if UNITY_IOS
            return new TestFactoryIOS(BASE_URL);
#elif UNITY_ANDROID
            return new TestFactoryAndroid(BASE_URL);
#elif (UNITY_WSA || UNITY_WP8)
            return new TestFactoryWindows(BASE_URL);
#else
            Debug.Log("Cannot run integration tests (Error in TestApp.GetPlatformSpecificTestLibrary(...)). None of the supported platforms selected.");
            return null;
#endif
        }

#if UNITY_IOS
        public void ExecuteCommand(string commandJson)
        {
            _testFactoryiOS.ExecuteCommand(commandJson);
        }
#endif
        public static void Log(string message, bool useUnityDebug = false)
        {
#if UNITY_ANDROID
            var now = DateTime.Now;
            string currentTimeString = string.Format("{0}:{1}", now.ToShortTimeString(), now.Second);
            string output = string.Format("[{0}{1}]: {2}", currentTimeString, TAG, message);
            if (!useUnityDebug)
            {
                Console.WriteLine(output);
            }
            else
            {
                Debug.Log(output);
            }
#else
            Debug.Log(message);
#endif
        }

        public static void LogError(string message, bool useUnityDebug = false)
        {
#if UNITY_ANDROID
            var now = DateTime.Now;
            string currentTimeString = string.Format("{0}:{1}", now.ToShortTimeString(), now.Second);
            string output = string.Format("[{0}{1}][Error!]: {2}", currentTimeString, TAG, message);
            if (!useUnityDebug)
            {
                Console.WriteLine(output);
            }
            else
            {
                Debug.Log(output);
            }
#else
            Debug.LogError(message);
#endif
        }
    }
}
