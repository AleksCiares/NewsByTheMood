namespace NewsByTheMood.Services.PositifityRater.Implement
{
    public class WordTonal
    {
        public string Term { get; set; }
        public string Tag { get; set; }
        public double Value { get; set; }
        public double Pstv { get; set; }
        public double Ngtv { get; set; }
        public double Neut { get; set; }
        public double Dunno { get; set; }
        public double PstvNgtvDisagreementRatio { get; set; }
    }
}
