using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hotel.Room.Model;
namespace Hotel.Room.Files;

public class CameraFisier
{
    private readonly string _caleFisier = "C:\\Users\\Madalina\\OneDrive\\Desktop\\c#\\Proiect\\Proiect\\camere.json";

    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    public void SalveazaCamere(List<Camera> camere)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(camere, _options);
            File.WriteAllText(_caleFisier, jsonString);
        }
        catch (IOException e)
        {
            Console.WriteLine($"Eroare la salvarea JSON: {e.Message}");
        }
    }

    public List<Camera> IncarcaCamere()
    {
        if (!File.Exists(_caleFisier))
            return new List<Camera>();
        try
        {
            string jsonString = File.ReadAllText(_caleFisier);
            return JsonSerializer.Deserialize<List<Camera>>(jsonString, _options);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la incarcarea JSON: {ex.Message}");
            return new List<Camera>();
        }
    }
}