using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using Hotel.Reservation.Model;
namespace Hotel.Reservation.Files;

public class RezervareFisier
{
    private readonly string _caleFisier = "rezervari.json";
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = {new JsonStringEnumConverter()}
    };

    public void SalveazaRezervari(List<Rezervare> rezervari)
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(rezervari, _options);
            File.WriteAllText(_caleFisier, jsonString);

        }
        catch (IOException e)
        {
            MessageBox.Show($"Eroare la scrierea in fisierul de rezervari : {e.Message}");
        }
    }

    public List<Rezervare> IncarcaRezervari()
    {
        if (!File.Exists(_caleFisier))
            return new List<Rezervare>();
        try
        {
            string jsonString = File.ReadAllText(_caleFisier);
            return  JsonSerializer.Deserialize<List<Rezervare>>(jsonString, _options);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Eroare la incarcarea rezervarilor: {e.Message}");
            return new List<Rezervare>();
        }
    }
}