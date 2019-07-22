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
    public Action OnFailureToRecord;
    public Action OnMicrophoneLoadEnoughEvent;

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

        SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();

        if (SpeechRecognizer.ExistsOnDevice()) {

            if (WaitForVoiceApprove._ALWAYS_ONLY_LOAD_ENOUGH)
            {
                listener.OnMicrophoneLoadEnough.AddListener(OnMicrophoneLoadEnough);
                AllowRecording = true;
                return;
            }

			listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			listener.onEndOfSpeech.AddListener(OnEndOfSpeech);

            //Yoav
            listener.OnMicrophoneLoadEnough.AddListener(OnMicrophoneLoadEnough);

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

        if (Application.isEditor)
        {
            //Yoav
            listener.OnMicrophoneLoadEnough.AddListener(OnMicrophoneLoadEnough);
            AllowRecording = true;
        }

    }

    public void OnMicrophoneLoadEnough()
    {
        OnMicrophoneLoadEnoughEvent?.Invoke();
    }

    public void OnFinalResult(string result) {
        resultText.text = result;
        OnFinalResults?.Invoke(result);
    }

	public void OnPartialResult(string result) {
		resultText.text = result;
    }

	public void OnAvailabilityChange(bool available) {
	    AllowRecording = available;

        
        if (Application.isEditor)
        {
            AllowRecording = true;
        }
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
	    OnFailureToRecord?.Invoke();
    }

    public void StartRecording(bool onlyLoudness)
    {
        if (AllowRecording &&
            (!SpeechRecognizer.IsRecording() || Application.isEditor))
        {
            SpeechRecognizer.StartRecording(true, onlyLoudness);
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

    public void StopRecording()
    {
        IsRecording = false;
        if (SpeechRecognizer.IsRecording())
        {
            SpeechRecognizer.StopIfRecording();
        }
    }

}
