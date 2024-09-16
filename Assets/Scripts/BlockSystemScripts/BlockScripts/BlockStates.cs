namespace BlockSystemScripts.BlockScripts
{
    public enum BlockType
    {
        None,
        Blue,
        Green,
        Yellow,
        Heavy,
        PowerUp
    }

    public enum BlockState
    {
        Falling,
        Landed,
        CanPickUp
    }

    public enum PlacementScoreState
    {
        ScoreNotYetTriggered,
        ScoreTriggered
    }
}