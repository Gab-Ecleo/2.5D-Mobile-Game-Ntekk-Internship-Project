using System;

public enum PointsMethod
{
    Add,
    Set
}

public class PointEventArgs : EventArgs
{
    public PointsMethod PointsMethod;
    public int Points;
    public int Multiplier;
    public int FinalScore;

    public PointEventArgs(PointsMethod pointsMethod, int points, int multiplier = 1)
    {
        PointsMethod = pointsMethod;
        Points = points;
        Multiplier = multiplier;
        FinalScore = points * multiplier;
    }
}
