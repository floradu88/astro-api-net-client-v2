namespace AstrologyApiClient.Models;

/// <summary>
/// Represents birth data for astrology calculations
/// </summary>
public class BirthData
{
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public int Hour { get; set; }
    public int Min { get; set; }
    public double Lat { get; set; }
    public double Lon { get; set; }
    public double Tzone { get; set; }
}

/// <summary>
/// Represents birth data for composite/synastry calculations (primary person)
/// </summary>
public class PrimaryBirthData
{
    public int PDay { get; set; }
    public int PMonth { get; set; }
    public int PYear { get; set; }
    public int PHour { get; set; }
    public int PMin { get; set; }
    public double PLat { get; set; }
    public double PLon { get; set; }
    public double PTzone { get; set; }
}

/// <summary>
/// Represents birth data for composite/synastry calculations (secondary person)
/// </summary>
public class SecondaryBirthData
{
    public int SDay { get; set; }
    public int SMonth { get; set; }
    public int SYear { get; set; }
    public int SHour { get; set; }
    public int SMin { get; set; }
    public double SLat { get; set; }
    public double SLon { get; set; }
    public double STzone { get; set; }
}

/// <summary>
/// Represents birth data for composite/synastry calculations (both persons)
/// </summary>
public class CompositeBirthData
{
    public PrimaryBirthData Primary { get; set; } = new();
    public SecondaryBirthData Secondary { get; set; } = new();
    public double? Orb { get; set; }
}

/// <summary>
/// Represents birth data with prediction timezone
/// </summary>
public class BirthDataWithPredictionTimezone : BirthData
{
    public double PredictionTimezone { get; set; }
}

/// <summary>
/// Represents tarot prediction request data
/// </summary>
public class TarotPredictionData
{
    public int Love { get; set; }
    public int Career { get; set; }
    public int Finance { get; set; }
}

