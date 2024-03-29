﻿using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using System.Text;
using System.Collections.Generic;

namespace KKSpeech {
	
	public enum AuthorizationStatus {
		Authorized,
		Denied,
		NotDetermined,
		Restricted
	}

	public struct SpeechRecognitionOptions {
		public bool shouldCollectPartialResults;
	}

	public struct LanguageOption {
		public readonly string id;
		public readonly string displayName;

		public LanguageOption(string id, string displayName) {
			this.id = id;
			this.displayName = displayName;
		}
	}

	/*
	 * check readme.pdf before using!
	 */

	public class SpeechRecognizer : System.Object {

		#pragma warning disable CS0162 
		public static bool ExistsOnDevice() {

		    if (Application.isEditor)
		    {
		        StartMicrophone();
		    }

		    if (WaitForVoiceApprove._ALWAYS_ONLY_LOAD_ENOUGH)
		    {
		        StartMicrophone();
                return true;
		    }

            #if UNITY_IOS && !UNITY_EDITOR
			return iOSSpeechRecognizer._EngineExists();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			return AndroidSpeechRecognizer.EngineExists();
			#endif
			return false; // sorry, no support besides Android and iOS :-(
		}

		public static void RequestAccess() {

			#if UNITY_IOS && !UNITY_EDITOR
			iOSSpeechRecognizer._RequestAccess();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidSpeechRecognizer.RequestAccess();
			#endif
            
		    StartMicrophone();

		}

        private static AudioClip _clipRecord;
	    private static int _sampleWindow = 128;
	    private static float _currentMicrophoneMax = 0;
	    private static float _loadEnough = 0.04f;
	    private static float _waitBeforeMicrophoneCheck = 0.2f;
	    private static float _startTimeForWaitBeforeMicrophoneCheck;

        //Yoav - units microphone
        private static void StartMicrophone()
        {
            ResetMicrophoneStatus();
            _clipRecord = Microphone.Start(null, true, 999, 44100);
	        Debug.Log(_clipRecord);
        }

	    private static void ResetMicrophoneStatus()
	    {
	        _startTimeForWaitBeforeMicrophoneCheck = Time.timeSinceLevelLoad;
            _currentMicrophoneMax = 0;
	    }

	    private static float GetMicrophoneMax()
	    {
	        return _currentMicrophoneMax;
	    }

	    public static bool WasMicrophoneLoadEnough()
	    {
	        return _currentMicrophoneMax > _loadEnough;
	    }

	    public static bool IsMicrophoneLoadEnough()
	    {
	        var currentLevel = MicrophoneLevelMaxAtSample();
            return currentLevel > _loadEnough;
	    }

        public static void CheckMicrophoneMax()
        {

            if (Time.timeSinceLevelLoad < (_startTimeForWaitBeforeMicrophoneCheck + _waitBeforeMicrophoneCheck))
            {
                return;
            }

            var sampleMax = MicrophoneLevelMaxAtSample();
	        if (sampleMax > _currentMicrophoneMax)
	        {
	            _currentMicrophoneMax = sampleMax;
	        }
	    }

	    private static float MicrophoneLevelMaxAtSample()
	    {
	        float levelMax = 0;
	        float[] waveData = new float[_sampleWindow];
	        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);
	        if (micPosition < 0)
	        {
	            return 0;
	        }
	        _clipRecord.GetData(waveData, micPosition);
	        for (int i = 0; i < _sampleWindow; ++i)
	        {
	            float wavePeak = waveData[i] * waveData[i];
	            if (levelMax < wavePeak)
	            {
	                levelMax = wavePeak;
	            }
	        }
	        return levelMax;
	    }


        public static bool IsRecording() {
            
            if (WaitForVoiceApprove._ALWAYS_ONLY_LOAD_ENOUGH)
            {
                return true;
            }

			#if UNITY_IOS && !UNITY_EDITOR
			return iOSSpeechRecognizer._IsRecording();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			return AndroidSpeechRecognizer.IsRecording();
			#endif

            if (Application.isEditor)
            {
                return true;
            }

            return false;
		}

		public static AuthorizationStatus GetAuthorizationStatus() {
			#if UNITY_IOS && !UNITY_EDITOR
			return (AuthorizationStatus)iOSSpeechRecognizer._AuthorizationStatus();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			return (AuthorizationStatus)AndroidSpeechRecognizer.AuthorizationStatus();
			#endif
			return AuthorizationStatus.NotDetermined;
		}

		public static void StopIfRecording() {
			Debug.Log("StopRecording...");
			#if UNITY_IOS && !UNITY_EDITOR
			iOSSpeechRecognizer._StopIfRecording();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidSpeechRecognizer.StopIfRecording();
			#endif
		}

		private static void StartRecording(SpeechRecognitionOptions options) {
			#if UNITY_IOS && !UNITY_EDITOR
			iOSSpeechRecognizer._StartRecording(options.shouldCollectPartialResults);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidSpeechRecognizer.StartRecording(options);
			#endif
		}

		public static void StartRecording(bool shouldCollectPartialResults, bool onlyLoudness = false)
		{
		    ResetMicrophoneStatus();
		    if (onlyLoudness)
		    {
                return;
		    }

            Debug.Log("StartRecording...");
			#if UNITY_IOS && !UNITY_EDITOR
			iOSSpeechRecognizer._StartRecording(shouldCollectPartialResults);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidSpeechRecognizer.StartRecording(shouldCollectPartialResults);
			#endif
		}

		public static void GetSupportedLanguages() {
			#if UNITY_IOS && !UNITY_EDITOR
			iOSSpeechRecognizer.SupportedLanguages();
			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidSpeechRecognizer.GetSupportedLanguages();
			#endif
		}

		public static void SetDetectionLanguage(string languageID) {
			#if UNITY_IOS && !UNITY_EDITOR
			iOSSpeechRecognizer._SetDetectionLanguage(languageID);
			#elif UNITY_ANDROID && !UNITY_EDITOR
			AndroidSpeechRecognizer.SetDetectionLanguage(languageID);
			#endif
		}
		#pragma warning restore CS0162  

		#if UNITY_IOS && !UNITY_EDITOR
		private class iOSSpeechRecognizer {

			[DllImport ("__Internal")]
			internal static extern void _SetDetectionLanguage(string languageID);

			[DllImport ("__Internal")]
			internal static extern string _SupportedLanguages();

			[DllImport ("__Internal")]
			internal static extern void _RequestAccess();

			[DllImport ("__Internal")]
			internal static extern bool _IsRecording();

			[DllImport ("__Internal")]
			internal static extern bool _EngineExists();

			[DllImport ("__Internal")]
			internal static extern int _AuthorizationStatus();

			[DllImport ("__Internal")]
			internal static extern void _StopIfRecording();

			[DllImport ("__Internal")]
			internal static extern void _StartRecording(bool shouldCollectPartialResults);

			public static void SupportedLanguages() {
				string formattedLangs = _SupportedLanguages();
				var listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
				if (listener != null) {
					listener.SupportedLanguagesFetched(formattedLangs);
				}
			}
		}
		#endif

		#if UNITY_ANDROID && !UNITY_EDITOR
		private class AndroidSpeechRecognizer {

			private static string DETECTION_LANGUAGE = null;

			internal static void GetSupportedLanguages() {
				GetAndroidBridge().CallStatic("GetSupportedLanguages");
			}

			internal static void SetDetectionLanguage(string languageID) {
				AndroidSpeechRecognizer.DETECTION_LANGUAGE = languageID;
			}

			internal static void RequestAccess() {
				GetAndroidBridge().CallStatic("RequestAccess");
			}
				
			internal static bool IsRecording() {
				return GetAndroidBridge().CallStatic<bool>("IsRecording");
			}

			internal static bool EngineExists() {
				return GetAndroidBridge().CallStatic<bool>("EngineExists");
			}

			internal static int AuthorizationStatus() {
				return GetAndroidBridge().CallStatic<int>("AuthorizationStatus");
			}

			internal static void StopIfRecording() {
				GetAndroidBridge().CallStatic("StopIfRecording");
			}

			internal static void StartRecording(bool shouldCollectPartialResults) {
				var options = new SpeechRecognitionOptions();
				options.shouldCollectPartialResults = shouldCollectPartialResults;
				StartRecording(options);
			}

			internal static void StartRecording(SpeechRecognitionOptions options) {
				GetAndroidBridge().CallStatic("StartRecording", CreateJavaRecognitionOptionsFrom(options));
			}

			private static AndroidJavaObject CreateJavaRecognitionOptionsFrom(SpeechRecognitionOptions options) {
				var javaOptions = new AndroidJavaObject("kokosoft.unity.speechrecognition.SpeechRecognitionOptions");
				javaOptions.Set<bool>("shouldCollectPartialResults", options.shouldCollectPartialResults);
				javaOptions.Set<string>("languageID", DETECTION_LANGUAGE);
				return javaOptions;
			}

			private static AndroidJavaObject GetAndroidBridge() {
				var bridge = new AndroidJavaClass("kokosoft.unity.speechrecognition.SpeechRecognizerBridge");
				return bridge;
			}
		}
		#endif
	}

}



