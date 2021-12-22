namespace LeagueTierLevels
{
    class Summoner
    {
        private bool m_FreshBlood;
        private bool m_HotStreak;
        private bool m_Inactive;
        private string m_LeagueId;
        private int m_LeaguePoints;
        private int m_Losses;
        private string m_Rank;
        private string m_SummonerId;
        private string m_SummonerName;
        private string m_Tier;
        private bool m_Veteran;
        private int m_Wins;
        private string m_AccountId;
        private string m_Puuid;
        private long m_RevisionDate;
        private long m_SummonerLevel;

        public string SummonerId { get { return m_SummonerId; } }
        public long Level { get { return m_SummonerLevel; } }

        public Summoner(string summonerId)
        {
            this.m_SummonerId = summonerId;
        }


        public void AddLeagueEntryData(MingweiSamuel.Camille.LeagueV4.LeagueEntry player)
        {
            this.m_FreshBlood = player.FreshBlood;
            this.m_HotStreak = player.HotStreak;
            this.m_Inactive = player.Inactive;
            this.m_LeagueId = player.LeagueId;
            this.m_LeaguePoints = player.LeaguePoints;
            this.m_Losses = player.Losses;
            this.m_Rank = player.Rank;
            this.m_SummonerName = player.SummonerName;
            this.m_Tier = player.Tier;
            this.m_Veteran = player.Veteran;
            this.m_Wins = player.Wins;
        }

        public void AddSummonerData(MingweiSamuel.Camille.SummonerV4.Summoner summoner)
        {
            this.m_AccountId = summoner.AccountId;
            this.m_Puuid = summoner.Puuid;
            this.m_RevisionDate = summoner.RevisionDate;
            this.m_SummonerLevel = summoner.SummonerLevel;
        }



    }
}
