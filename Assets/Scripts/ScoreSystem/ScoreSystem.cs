using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointEventManager
{
    public static event EventHandler<PointEventArgs> OnPointEvent;

    public static void TriggerPointsEvent(PointsMethod pointsMethod, int points, int multiplier = 1)
    {
        OnPointEvent?.Invoke(null, new PointEventArgs(pointsMethod, points, multiplier));
    }
}

public class ScoreSystem : MonoBehaviour
{
    public int Points;
    public int PointsToAdd;
    private bool blockClear;
    private int FinalPoints;

    private void OnEnable()
    {
        PointEventManager.OnPointEvent += HandlePointEvent;
    }

    private void OnDisable()
    {
        PointEventManager.OnPointEvent -= HandlePointEvent;
    }

    private void HandlePointEvent(object sender, PointEventArgs e)
    {
        switch (e.PointsMethod)
        {
            case PointsMethod.Set:
                SetPoints(e.FinalScore);
                break;

            case PointsMethod.Add:
                AddPoints(e.FinalScore);
                break;
        }
    }

    public void SetPoints(int points) 
    {
        //for ui
        Points = points;
        FinalPoints = points;
    }

    public void AddPoints(int addPoints)
    {
        Points += addPoints;
        FinalPoints += addPoints;
    }

    /*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            PointEventManager.TriggerPointsEvent(PointsMethod.Add, PointsToAdd, 1);
            Debug.Log($"Setting Points: {FinalPoints}");
        }
    }
    */
}

