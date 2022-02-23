namespace HomePress.Core.Data
{
    public class VideoCompact
    {
        public string? VideoId { get; set; }
        public string? Thumbnail { get; set; }
        public string? Dimention1080 { get; set; }
        public string? Dimention720 { get; set; }
        public string? Dimention480 { get; set; }
        public double? DurationInSeconds { get; set; }
        public string? Caption { get; set; }

        public string? GetHighestQuality()
        {
            return Dimention1080 ?? Dimention720 ?? Dimention480;
        }

        public string? GetLowestQuality()
        {
            return Dimention480 ?? Dimention720 ?? Dimention1080;
        }
    }
}
