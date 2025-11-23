using AstrologyApiClient.Models;

namespace AstrologyApiClient.Interfaces;

/// <summary>
/// Interface for converting model objects to form data dictionaries
/// </summary>
public interface IFormDataConverter
{
    Dictionary<string, string> Convert(BirthData data);
    Dictionary<string, string> Convert(CompositeBirthData data);
    Dictionary<string, string> Convert(BirthDataWithPredictionTimezone data);
    Dictionary<string, string> Convert(TarotPredictionData data);
}

