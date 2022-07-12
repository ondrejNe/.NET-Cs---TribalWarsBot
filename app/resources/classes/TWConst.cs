
namespace TribalWarsBot
{
    public static class TWConst
    {
        /** RESOURCE YIELDS */
        public static readonly int[] VILLAGE_YIELDS =
            {  5,   30,   35,   41,   47,
              55,   64,   74,   86,  100,
             117,  136,  158,  184,  214,
             249,  289,  337,  391,  455,
             530,  616,  717,  833,  969,
            1127, 1311, 1525, 1774, 2063,
            2400};

        /** SCAVENGE HAUL MULTIPLIER */
        public static readonly double[] HAUL_MULTIPLIER = { 7.5, 3, 1.5, 1 };

        /** UNIT CAPACITY ENUM */
        public static readonly int[] UNIT_CAPACITIES = { 25, 15, 10, 10, 80, 50, 50 };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", 
            "CA1069:Enums values should not be duplicated", 
            Justification = "Some units have equal capacity")]
        public enum UNIT_CAPACITY
        {
            SPEAR = 25,
            SWORD = 15,
            AXE = 10,
            ARCHER = 10,

            LIGHT = 80,
            MARCHER = 50,
            HEAVY = 50
        }
        public const int UNIT_CAPACITY_LENGHT = 7;

        /** UNIT ENUM */
        public enum UNIT_SCAVENGE 
        {
            SPEAR = 0,
            SWORD = 1,
            AXE = 2,
            ARCHER = 3,

            LIGHT = 4,
            MARCHER = 5,
            HEAVY = 6
        }
        public enum UNIT
        {
            PEAR = 0,
            SWORD = 1,
            AXE = 2,
            ARCHER = 3,

            SPY = 4,
            LIGHT = 5,
            MARCHER = 6,
            HEAVY = 7,

            RAM = 8,
            CATAPULT = 9
        }

        /** UNIT POP ENUM */
        public const int UNIT_TOTAL = 10;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", 
            "CA1069:Enums values should not be duplicated", 
            Justification = "Foot units have same pop")]
        public enum UNIT_POPULATION
        {
            SPEAR = 1,
            SWORD = 1,
            AXE = 1,
            ARCHER = 1,

            SPY = 2,
            LIGHT = 4,
            MARCHER = 5,
            HEAVY = 6,

            RAM = 7,
            CATAPULT = 8
        }
    }
}
