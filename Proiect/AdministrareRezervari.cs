using Hotel.Customer.Services;
using Hotel.Room.Admin;
using Hotel.Room.Model;

namespace Hotel.Reservation.Admin;
using Hotel.Reservation.Model;
using Hotel.Reservation.Files;

public class AdministrareRezervari : ICustomerService
{
    private List<Rezervare> _rezervari;
    private readonly RezervareFisier _fisier;
    private readonly AdministrareCamere _adminCamere;

    public AdministrareRezervari(RezervareFisier fisier,  AdministrareCamere adminCamere)
    {
        _fisier = fisier;
        _adminCamere = adminCamere;
        _rezervari = _fisier.IncarcaRezervari();
        ActualizareStatusRezervare();
    }

    private bool VerificaDisponibilitate(int nrCamera, DateOnly dataSosire, DateOnly dataPlecare)
    {
        bool ok = true;
        foreach (var r in _rezervari)
        {
            if (r.CameraRezervata.Numar == nrCamera && r.StatusRezervare == StatusRezervare.Activa &&
                (r.DataPlecare > dataSosire && r.DataSosire < dataPlecare))
                ok = false;
        }

        return ok;
    }

    public void AdaugaRezervare(Rezervare noua)
    {
        var cameraReala = _adminCamere.AfisareCamere().FirstOrDefault(c => c.Numar == noua.CameraRezervata.Numar);
        if (cameraReala == null)
        {
            Console.WriteLine("Eroare: Camera nu există!");
            return;
        }

        if (cameraReala.StatusCamera != StatusCamera.Libera)
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
            Console.WriteLine("Rezervare salvată cu succes!");
        }
        else 
            Console.WriteLine("Camera este deja rezervată în acea perioadă!");
    }

    public void ActualizareStatusRezervare()
    {
        DateOnly azi = DateOnly.FromDateTime(DateTime.Now);
        bool modificari = false;
        foreach (var r in _rezervari)
        {
            if (r.StatusRezervare == StatusRezervare.Activa && azi > r.DataPlecare)
            {
                r.StatusRezervare = StatusRezervare.Finalizata;
                modificari = true;
            }
            if (r.StatusRezervare == StatusRezervare.Finalizata)
            {
                StatusCamera statusVechi = r.CameraRezervata.StatusCamera;
                if (azi == r.DataPlecare.AddDays(1))
                    r.CameraRezervata.StatusCamera = StatusCamera.In_Curatenie;
                else if (azi >= r.DataPlecare.AddDays(2))
                    r.CameraRezervata.StatusCamera = StatusCamera.Libera;
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
            
            if (statusNou == StatusRezervare.Activa)
            {
                rezervareGasita.CameraRezervata.StatusCamera = StatusCamera.Ocupata;
            }
            else if (statusNou == StatusRezervare.Anulata)
            {
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
}