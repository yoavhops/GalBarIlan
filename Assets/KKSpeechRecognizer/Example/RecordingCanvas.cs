using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KKSpeech;
using TMPro;

public class RecordingCanvas : MonoBehaviour {

    public bool SetEnglish = true;

    public TextMeshProUGUI resultText;
    public Image ShowRecordingImage;
    public bool AllowRecording = false;
    private bool _isRecording;

    public Action<string> OnFinalResults;
    public Action OnVoiceEndOfSpeech;

    public bool IsRecording
    {
        get
        {
            return _isRecording;
        }
        set
        {
            _isRecording = value;
            ShowRecordingImage.enabled = _isRecording;
        }
    }

    void Start()
    {
        IsRecording = false;
        resultText.text = "";

        if (SpeechRecognizer.ExistsOnDevice()) {
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
			listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
		    AllowRecording = false;
		    SpeechRecognizer.RequestAccess();

            if (SetEnglish)
            {
                SpeechRecognizer.GetSupportedLanguages();
                SpeechRecognizer.SetDetectionLanguage("en-US");
            }

        } else {
			resultText.text = "Sorry, but this device doesn't support speech recognition";
		    AllowRecording = false;

		}

	}

	public void OnFinalResult(string result) {
        resultText.text = result;
        if (OnFinalResults != null)
	    {
	        OnFinalResults(result);
	    }
	}

	public void OnPartialResult(string result) {
		resultText.text = result;
    }

	public void OnAvailabilityChange(bool available) {
	    AllowRecording = available;
		if (!available) {
			resultText.text = "Speech Recognition not available";
		}
	}

	public void OnAuthorizationStatusFetched(AuthorizationStatus status) {
		switch (status) {
		case AuthorizationStatus.Authorized:
		    AllowRecording = true;
			break;
		default:
		    AllowRecording = false;
			resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
			break;
		}
	}

	public void OnEndOfSpeech()
	{
	    IsRecording = false;
	}

	public void OnError(string error)
	{
        Debug.LogError(error);
	    IsRecording = false;
	    OnVoiceEndOfSpeech();
    }

    public void StartRecording()
    {
        if (AllowRecording &&
            !SpeechRecognizer.IsRecording())
        {
            SpeechRecognizer.StartRecording(true);
            IsRecording = true;
        }
    }

    public void OnStartRecordingPressed() {
		if (SpeechRecognizer.IsRecording()) {
			SpeechRecognizer.StopIfRecording();

		    IsRecording = false;

        } else {
			SpeechRecognizer.StartRecording(true);

		    IsRecording = true;
		}
	}

    public void Stop()
    {
        if (SpeechRecognizer.IsRecording())
        {
            SpeechRecognizer.StopIfRecording();

            IsRecording = false;
        }
    }

}
