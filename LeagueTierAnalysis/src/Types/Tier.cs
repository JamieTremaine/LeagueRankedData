using System;
using System.Linq;

namespace LeagueTierLevels
{
    class Tier
    {
        private string m_TierName;
        private Division[] m_Divisions;
        private double m_AverageLevel;

        public Division[] Divisions { get { return m_Divisions; } }
        public string Name { get { return m_TierName; } }

        public Tier(string name, Division[] divisions)
        {
            this.m_TierName = name;
            this.m_Divisions = divisions;
        }

        public Tier(string name)
        {
            this.m_TierName = name;
            this.m_Divisions = new Division[4];
        }

        public void AddDivision(Division division)
        {
            for (int i = 0; i < m_Divisions.Length; i++)
            {
                if (m_Divisions[i] == null)
                {
                    m_Divisions[i] = division;
                    return;
                }
            }
        }

        public double GetAverageLevel()
        {
            if (this.m_AverageLevel != default)
            {
                return m_AverageLevel;
            }
            else
            {
                double[] averageOfDivision = new double[DivisionInfo.TOTALDIVISIONS];
                for (int i = 0; i < m_Divisions.Length; i++)
                {                   
                    averageOfDivision[i] = m_Divisions[i].GetAverageLevel();
                }

                this.m_AverageLevel = averageOfDivision.Average();
                return this.m_AverageLevel;
            }
        }
    }
}
