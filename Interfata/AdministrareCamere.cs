using System.Collections.Immutable;
using Hotel.Room.Model;
using Hotel.Room.Files;
namespace Hotel.Room.Admin;

public class AdministrareCamere
{
    private List<Camera> _camere;
    private readonly CameraFisier _fisier= new CameraFisier();

    public AdministrareCamere()
    {
        _camere = _fisier.IncarcaCamere();
    }

    public void AdaugaCamera(int numar)
    {
        if (_camere.Any(c => c.Numar == numar))
        {
            Console.WriteLine($"Eroare: Camera {numar} exista deja.");
            return;
        }
        _camere.Add(new Camera(numar));
        _fisier.SalveazaCamere(_camere);
        Console.WriteLine($"Camera {numar} a fost creata.");
    }

    public void SetareStatusCamera(int numar, StatusCamera statusNou)
    {
        Camera cameraGasita = null;
        foreach (var c in _camere)
        {
            if (c.Numar == numar)
            {
                cameraGasita = c;
                break;
            }
        }

        if (cameraGasita != null)
        {
            cameraGasita.ModificareStatus(statusNou);
            _fisier.SalveazaCamere(_camere);
        }
    }

    public void StergeCamera(int numar)
    {
        Camera cameraGasita = null;
        foreach (var c in _camere)
        {
            if (c.Numar == numar) 
            {
                cameraGasita = c;
                break;
            }
        }

        if (cameraGasita != null)
        {
            _camere.Remove(cameraGasita);
            _fisier.SalveazaCamere(_camere);
            Console.WriteLine($"Camera {numar} a fost eliminată din sistem.");
        }
    }

    public List<Camera> AfisareCamere()
    {
        return _camere;
    }   
    
}