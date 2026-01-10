using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hotel.Customers.Services;
using Hotel.EditSelectedReservation;
using Hotel.Room.Admin;
using Hotel.Room.Model;
using Hotel.Security;

namespace Hotel.Reservation.Admin;
using Hotel.Reservation.Model;
using Hotel.Reservation.Files;

public class AdministrareRezervari : ICustomerService
{
    private List<Rezervare> _rezervari;
    private readonly RezervareFisier _fisier;
    private readonly AdministrareCamere _adminCamere;
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };
    public AdministrareRezervari()
    {
        _fisier = new RezervareFisier();
        _adminCamere = new AdministrareCamere();
        _rezervari = _fisier.IncarcaRezervari();
        ActualizareStatusRezervare();
    }

    public bool VerificaDisponibilitate(int nrCamera, DateOnly dataSosire, DateOnly dataPlecare)
    {
        bool ok = true;
        if(Session.CurrentFrame.Content is EditSelectedReservationPage)
        {
            foreach (var r in _rezervari)
            {
                if (r.CameraRezervata.Numar == nrCamera && r.StatusRezervare == StatusRezervare.Activa &&
                (r.DataPlecare > dataSosire && r.DataSosire < dataPlecare) && !r.PersoanaRezervare.Equals(Session.CurrentUser) )
                    ok = false; 
            }
        }
        else
        {
            foreach (var r in _rezervari)
            {
                if (r.CameraRezervata.Numar == nrCamera && r.StatusRezervare == StatusRezervare.Activa &&
                    (r.DataPlecare > dataSosire && r.DataSosire < dataPlecare) )
                    ok = false; 
            }
        }
        return ok;
    }

    public void AdaugaRezervare(Rezervare noua, bool edit)
    {
        _rezervari = _fisier.IncarcaRezervari();
        var cameraReala = _adminCamere.AfisareCamere().FirstOrDefault(c => c.Numar == noua.CameraRezervata.Numar);
        if (cameraReala == null)
        {
            Console.WriteLine("Eroare: Camera nu există!");
            return;
        }

        if (cameraReala.StatusCamera != StatusCamera.Libera && !edit)
        {
            Console.WriteLine($"Eroare: Camera {cameraReala.Numar} este {cameraReala.StatusCamera}!");
            return; 
        }

        if (VerificaDisponibilitate(cameraReala.Numar, noua.DataSosire, noua.DataPlecare))
        {
            cameraReala.StatusCamera = StatusCamera.Ocupata; 
            noua.CameraRezervata = cameraReala; 
            _rezervari.Add(noua);
            _fisier.SalveazaRezervari(_rezervari);
            _adminCamere.SetareStatusCamera(cameraReala.Numar, StatusCamera.Ocupata);
            ActualizareStatusRezervare();
            Console.WriteLine("Rezervare salvată cu succes!");
        }
        else 
            Console.WriteLine("Camera este deja rezervată în acea perioadă!");
    }

    public void StergereRezervare(Rezervare rezervare)
    {
        try
        {
            
            _rezervari = _fisier.IncarcaRezervari();
            if(_rezervari.Find(res => res.Equals(rezervare))!= null)
            { _rezervari.Find(res => res.Equals(rezervare)).StatusRezervare = StatusRezervare.Anulata;}
            _fisier.SalveazaRezervari(_rezervari);
        }
        catch (IOException e)
        {
            Console.WriteLine($"Eroare la salvarea JSON: {e.Message}");
        }
    }
    public void ActualizareStatusRezervare()
    {
        DateOnly azi = DateOnly.FromDateTime(DateTime.Now);
        _rezervari = _fisier.IncarcaRezervari();
        bool modificari = false;
        foreach (var r in _rezervari)
        {
            Console.WriteLine($"{r.CameraRezervata.Numar}");
            if (r.StatusRezervare == StatusRezervare.Activa && azi > r.DataPlecare)
            {
                r.StatusRezervare = StatusRezervare.Finalizata;
                modificari = true;
            }
            if (r.StatusRezervare == StatusRezervare.Finalizata)
            {
                StatusCamera statusVechi = r.CameraRezervata.StatusCamera;
                if (azi == r.DataPlecare.AddDays(1))
                {
                    r.CameraRezervata.StatusCamera = StatusCamera.In_Curatenie;
                    _adminCamere.SetareStatusCamera(r.CameraRezervata.Numar, StatusCamera.In_Curatenie);
                }
                else if (azi >= r.DataPlecare.AddDays(2))
                {
                    r.CameraRezervata.StatusCamera = StatusCamera.Libera;
                    _adminCamere.SetareStatusCamera(r.CameraRezervata.Numar, StatusCamera.Libera);
                }

                if (statusVechi != r.CameraRezervata.StatusCamera)
                    modificari = true;
            }
            
        }
        if (modificari)
            _fisier.SalveazaRezervari(_rezervari);
    }
    
    public List<Rezervare> AfisareRezervari()
    {
        return _rezervari;
    }

    public void ModificaStatus(Rezervare rezervare, StatusRezervare statusNou)
    {
        Rezervare rezervareGasita = null;
        foreach (var r in _rezervari)
        {
            if (rezervare.CameraRezervata.Numar == r.CameraRezervata.Numar
                && rezervare.DataSosire == r.DataSosire)
            {
                rezervareGasita = r;
                
                break;
            }
        }
        if (rezervareGasita != null)
        {
            rezervareGasita.ModificareStatus(statusNou);
            AdministrareCamere ar = new AdministrareCamere();
            if (statusNou == StatusRezervare.Activa)
            {
                ar.SetareStatusCamera(rezervareGasita.CameraRezervata.Numar,StatusCamera.Ocupata);
                rezervareGasita.CameraRezervata.StatusCamera = StatusCamera.Ocupata;
            }
            else if (statusNou == StatusRezervare.Anulata)
            {
                ar.SetareStatusCamera(rezervareGasita.CameraRezervata.Numar,StatusCamera.Libera);
                rezervareGasita.CameraRezervata.StatusCamera = StatusCamera.Libera;
            }
            _fisier.SalveazaRezervari(_rezervari);
        }
    }

    public Camera CautaCameraDupaNumar(int numar)
    {
        return _adminCamere.AfisareCamere().FirstOrDefault(c => c.Numar == numar);
    }

    public List<Camera> CautaCameraDupaStatus(StatusCamera status)
    {
        return _adminCamere.AfisareCamere().Where(c => c.StatusCamera == status).ToList();
    }

    public void AnulareRezervare(Rezervare rez)
    {
        ModificaStatus(rez, StatusRezervare.Anulata);
    }
}