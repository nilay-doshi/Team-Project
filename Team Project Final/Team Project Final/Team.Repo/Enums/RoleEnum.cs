namespace Team.Repo.Enums
{
    public enum RoleEnum
    {
        #region Values
        User,
        Player,
        Captain,
        Coach = 5,
        #endregion
    }

    public enum CountEnum
    {
        #region Values
        FlagCount,
        TotalPlayers = 14,
        TotalCaptain = 1,
        CaptainMaxCount = 10,
        CoachMaxCount = 15,
        #endregion
    }
}
