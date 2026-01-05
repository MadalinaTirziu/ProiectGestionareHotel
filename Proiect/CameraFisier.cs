namespace ProiectPoo;

public class CameraFisier
{
    private readonly string _caleFisier = "camere.txt";

    public void SalveazaCamere(List<Camera> camere)
    {
        try
        {
            var linii = camere.Select(c => c.ToFileFormat());
            File.WriteAllLines(_caleFisier, linii);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Eroare la scrierea in fisier:  {ex.Message}");
        }
    }

    public List<Camera> IncarcaCamere()
    {
        if (!File.Exists(_caleFisier))
            return new List<Camera>();
        try
        {
            var linii = File.ReadAllLines(_caleFisier);
            return linii.Select(l => ParseCamera(l)).ToList();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private Camera ParseCamera(string linie)
    {
        var parti = linie.Split(';');
        return new Camera(int.Parse(parti[0]), (StatusCamera)Enum.Parse(typeof(StatusCamera), parti[1]));
    }
}