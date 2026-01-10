using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hotel.Room.Model;
namespace Hotel.Room.Files;

public class CameraFisier
{
    private readonly string _caleFisier = "camere.json";

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
            using (FileStream fs = new FileStream(_caleFisier, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(jsonString);
                    writer.Flush();
                    fs.Flush(); 
                }
                
            }
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
    public List<Camera> IncarcaCamereDisponibile()
    {
        if (!File.Exists(_caleFisier))
            return new List<Camera>();
        try
        {
            string jsonString = File.ReadAllText(_caleFisier);
            List<Camera> camere = JsonSerializer.Deserialize<List<Camera>>(jsonString, _options);
            return camere.Where(u => u.StatusCamera == StatusCamera.Libera).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Eroare la incarcarea JSON: {ex.Message}");
            return new List<Camera>();
        }
    }
}