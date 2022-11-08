namespace TriageConfiguration.TriageElements
{
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum OutputTypeEnum
    {
        HtmlImage = 0,
        PngImage = 1,
        JpgImage = 2,
        HtmlText = 3,
        Text = 4
    }
}
