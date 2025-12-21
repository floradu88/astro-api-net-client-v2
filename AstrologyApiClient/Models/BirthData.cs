namespace AstrologyApiClient.Models;

/// <summary>
/// Represents birth data for astrology calculations
/// </summary>
public class BirthData
{
    /// <summary>
    /// Day of birth (1-31)
    /// </summary>
    public int Day { get; set; }
    
    /// <summary>
    /// Month of birth (1-12)
    /// </summary>
    public int Month { get; set; }
    
    /// <summary>
    /// Year of birth
    /// </summary>
    public int Year { get; set; }
    
    /// <summary>
    /// Hour of birth (0-23)
    /// </summary>
    public int Hour { get; set; }
    
    /// <summary>
    /// Minute of birth (0-59)
    /// </summary>
    public int Min { get; set; }
    
    /// <summary>
    /// Latitude of birth location
    /// </summary>
    public double Lat { get; set; }
    
    /// <summary>
    /// Longitude of birth location
    /// </summary>
    public double Lon { get; set; }
    
    /// <summary>
    /// Timezone offset from UTC
    /// </summary>
    public double Tzone { get; set; }
}

/// <summary>
/// Represents birth data for composite/synastry calculations (primary person)
/// </summary>
public class PrimaryBirthData
{
    /// <summary>
    /// Day of birth for primary person (1-31)
    /// </summary>
    public int PDay { get; set; }
    
    /// <summary>
    /// Month of birth for primary person (1-12)
    /// </summary>
    public int PMonth { get; set; }
    
    /// <summary>
    /// Year of birth for primary person
    /// </summary>
    public int PYear { get; set; }
    
    /// <summary>
    /// Hour of birth for primary person (0-23)
    /// </summary>
    public int PHour { get; set; }
    
    /// <summary>
    /// Minute of birth for primary person (0-59)
    /// </summary>
    public int PMin { get; set; }
    
    /// <summary>
    /// Latitude of birth location for primary person
    /// </summary>
    public double PLat { get; set; }
    
    /// <summary>
    /// Longitude of birth location for primary person
    /// </summary>
    public double PLon { get; set; }
    
    /// <summary>
    /// Timezone offset from UTC for primary person
    /// </summary>
    public double PTzone { get; set; }
}

/// <summary>
/// Represents birth data for composite/synastry calculations (secondary person)
/// </summary>
public class SecondaryBirthData
{
    /// <summary>
    /// Day of birth for secondary person (1-31)
    /// </summary>
    public int SDay { get; set; }
    
    /// <summary>
    /// Month of birth for secondary person (1-12)
    /// </summary>
    public int SMonth { get; set; }
    
    /// <summary>
    /// Year of birth for secondary person
    /// </summary>
    public int SYear { get; set; }
    
    /// <summary>
    /// Hour of birth for secondary person (0-23)
    /// </summary>
    public int SHour { get; set; }
    
    /// <summary>
    /// Minute of birth for secondary person (0-59)
    /// </summary>
    public int SMin { get; set; }
    
    /// <summary>
    /// Latitude of birth location for secondary person
    /// </summary>
    public double SLat { get; set; }
    
    /// <summary>
    /// Longitude of birth location for secondary person
    /// </summary>
    public double SLon { get; set; }
    
    /// <summary>
    /// Timezone offset from UTC for secondary person
    /// </summary>
    public double STzone { get; set; }
}

/// <summary>
/// Represents birth data for composite/synastry calculations (both persons)
/// </summary>
public class CompositeBirthData
{
    /// <summary>
    /// Birth data for the primary person
    /// </summary>
    public PrimaryBirthData Primary { get; set; } = new();
    
    /// <summary>
    /// Birth data for the secondary person
    /// </summary>
    public SecondaryBirthData Secondary { get; set; } = new();
    
    /// <summary>
    /// Optional orb value for aspect calculations
    /// </summary>
    public double? Orb { get; set; }
}

/// <summary>
/// Represents birth data with prediction timezone
/// </summary>
public class BirthDataWithPredictionTimezone : BirthData
{
    /// <summary>
    /// Timezone for prediction calculations
    /// </summary>
    public double PredictionTimezone { get; set; }
}

/// <summary>
/// Represents tarot prediction request data
/// </summary>
public class TarotPredictionData
{
    /// <summary>
    /// Love score (0-100)
    /// </summary>
    public int Love { get; set; }
    
    /// <summary>
    /// Career score (0-100)
    /// </summary>
    public int Career { get; set; }
    
    /// <summary>
    /// Finance score (0-100)
    /// </summary>
    public int Finance { get; set; }
}

