using System;

namespace LeagueTierLevels
{
    static public class DivisionInfo
    {
        public static readonly int TOTALDIVISIONS = Enum.GetNames(typeof(MingweiSamuel.Camille.Enums.Division)).Length - 1; //-1 to remove tier 5
        public const MingweiSamuel.Camille.Enums.Division ONE = MingweiSamuel.Camille.Enums.Division.I;
        public const MingweiSamuel.Camille.Enums.Division TWO = MingweiSamuel.Camille.Enums.Division.II;
        public const MingweiSamuel.Camille.Enums.Division THREE = MingweiSamuel.Camille.Enums.Division.III;
        public const MingweiSamuel.Camille.Enums.Division FOUR = MingweiSamuel.Camille.Enums.Division.IV;
    }


}
