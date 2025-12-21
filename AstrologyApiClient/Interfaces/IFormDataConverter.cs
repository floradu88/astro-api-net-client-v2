using AstrologyApiClient.Models;

namespace AstrologyApiClient.Interfaces;

/// <summary>
/// Interface for converting model objects to form data dictionaries
/// </summary>
public interface IFormDataConverter
{
    /// <summary>
    /// Converts BirthData to form data dictionary
    /// </summary>
    /// <param name="data">Birth data to convert</param>
    /// <returns>Dictionary containing form data</returns>
    Dictionary<string, string> Convert(BirthData data);
    
    /// <summary>
    /// Converts CompositeBirthData to form data dictionary using standard format (p_ prefix for primary)
    /// </summary>
    /// <param name="data">Composite birth data to convert</param>
    /// <returns>Dictionary containing form data</returns>
    Dictionary<string, string> Convert(CompositeBirthData data);
    
    /// <summary>
    /// Converts BirthDataWithPredictionTimezone to form data dictionary
    /// </summary>
    /// <param name="data">Birth data with prediction timezone to convert</param>
    /// <returns>Dictionary containing form data</returns>
    Dictionary<string, string> Convert(BirthDataWithPredictionTimezone data);
    
    /// <summary>
    /// Converts TarotPredictionData to form data dictionary
    /// </summary>
    /// <param name="data">Tarot prediction data to convert</param>
    /// <returns>Dictionary containing form data</returns>
    Dictionary<string, string> Convert(TarotPredictionData data);
    
    /// <summary>
    /// Converts CompositeBirthData to hybrid format for general report endpoints
    /// Primary person uses simple format (day, month, year) instead of (p_day, p_month, p_year)
    /// Secondary person uses s_ prefix format (s_day, s_month, s_year)
    /// </summary>
    Dictionary<string, string> ConvertToHybridFormat(CompositeBirthData data);
    
    /// <summary>
    /// Converts two BirthData objects to hybrid format for general report endpoints
    /// Primary person uses simple format (day, month, year) instead of (p_day, p_month, p_year)
    /// Secondary person uses s_ prefix format (s_day, s_month, s_year)
    /// </summary>
    /// <param name="primary">Birth data for the primary person</param>
    /// <param name="secondary">Birth data for the secondary person</param>
    /// <param name="orb">Optional orb value for aspect calculations</param>
    Dictionary<string, string> ConvertToHybridFormat(BirthData primary, BirthData secondary, double? orb = null);
}

