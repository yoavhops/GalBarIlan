using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VoiceAnswers
{
    public string SuccessString;
    public int World;
    public int Part;
    private bool _hasFailedYet = false;
    public bool HasFailedYet { get { return _hasFailedYet; }}

    public VoiceAnswers(int worldIndex, int partIndex, string answer, bool shouldCheckFailure)
    {
        World = worldIndex;
        Part = partIndex;
        SuccessString = answer;
        if (!shouldCheckFailure)
        {
            MarkAsFailed();
        }
    }

    public void MarkAsFailed()
    {
        _hasFailedYet = true;
    }
}

public static class VoiceAnswersMediator 
{
    private static string _failString = "";
    private static Dictionary<(int,int), VoiceAnswers> IndexToAnswers;

    public static void Init()
    {     
        IndexToAnswers = new Dictionary<(int, int), VoiceAnswers>();      
    }

    public static void CreateVoiceAnswer(int worldIndex, int partIndex, string answer, bool checkFailure)
    {
        var voiceAnswer = new VoiceAnswers(worldIndex, partIndex, answer, checkFailure);
        AddVoiceAnswerToDictionary(voiceAnswer);
    }

    private static void AddVoiceAnswerToDictionary(VoiceAnswers voiceAnswer)
    {
        IndexToAnswers.Add((voiceAnswer.World, voiceAnswer.Part), voiceAnswer);
    }

    public static string GetVoiceAnswer(int worldNumber, int partNumber)
    {
        var voiceAnswer = IndexToAnswers[(worldNumber, partNumber)];

        if (voiceAnswer.HasFailedYet)
        {
            return voiceAnswer.SuccessString;

        }

        voiceAnswer.MarkAsFailed();
        return _failString;
    }
}
