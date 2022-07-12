
namespace TribalWarsBot
{
    internal class Village
    {
        public int Iron { get; set; }
        public int Wood { get; set; }
        public int Stone { get; set; }
        public int YieldHour { get; set; } = 0;
        public int Coordx { get; set; }
        public int Coordy { get; set; }
        public int Points { get; set; }
        public int OwnerID { get; set; }
        public bool Barbaric { get; set; } = true;
        public string? Name { get; set; } = null;
        public Village(int iron, int wood, int stone, int X, int Y, int owner, string name)
        {
            this.Iron = iron;
            this.Wood = wood;
            this.Stone = stone;
            this.Coordx = X;
            this.Coordy = Y;
            this.OwnerID = owner;
            this.Barbaric = false;
            this.Name = name;
            ComputeYield();
        }
        public Village(int X, int Y)
        {
            this.Coordx = X;
            this.Coordy = Y;
        }
        public Village(string name, int X, int Y)
        {
            this.Name = name;
            this.Coordx = X;
            this.Coordy = Y;
        }
        
        public void ComputeYield()
        {
            this.YieldHour +=
                 TWConst.VILLAGE_YIELDS[Iron]
               + TWConst.VILLAGE_YIELDS[Wood]
               + TWConst.VILLAGE_YIELDS[Stone];
        }
        public double ComputeDistanceFrom(int X, int Y)
        {
            return Math.Sqrt(Math.Pow(X - this.Coordx, 2) + Math.Pow(Y - this.Coordy, 2));
        }
        public double ComputeDistanceFrom(Village village)
        {
            return Math.Sqrt(Math.Pow(village.Coordx - this.Coordx, 2) + Math.Pow(village.Coordy - this.Coordy, 2));
        }
    }
}
