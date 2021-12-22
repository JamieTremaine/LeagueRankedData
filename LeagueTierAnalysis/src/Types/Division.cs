using System.Linq;
using System;

namespace LeagueTierLevels
{
    class Division 
    {
        private MingweiSamuel.Camille.Enums.Division m_DivisionNumber;
        private Summoner[] m_players;
        private double m_AverageLevel;

        public MingweiSamuel.Camille.Enums.Division DivisionNumber { get { return m_DivisionNumber; } }

        public int NumPlayersInDivision
        {
            get { return m_players.Length; }
        }

        public Summoner[] Players
        {
            get { return m_players; }
        }

        public Division(MingweiSamuel.Camille.Enums.Division divisionNumber)
        {
            this.m_DivisionNumber = divisionNumber;
            this.m_players = new Summoner[0];
        }
        public Division(MingweiSamuel.Camille.Enums.Division divisionNumber, Summoner[] players)
        {
            this.m_DivisionNumber = divisionNumber;
            this.m_players = players;
        }

        public double GetAverageLevel()
        {
            if (this.m_AverageLevel != 0)
            {
                return m_AverageLevel;
            }
            else
            {
                long[] summonerLevels = new long[m_players.Length];
                for (int i = 0; i < m_players.Length; i++)
                {
                    summonerLevels[i] = m_players[i].Level;
                }

                this.m_AverageLevel = Math.Round(summonerLevels.Average());
                return this.m_AverageLevel;
            }
        }
    }
}
