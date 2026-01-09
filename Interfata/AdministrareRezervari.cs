using System.ComponentModel;
using Hotel.Room.Admin;
using Hotel.Room.Model;

namespace Hotel.Reservation.Admin;
using Hotel.Reservation.Model;
using Hotel.Reservation.Files;

public class AdministrareRezervari 
{
    private List<Rezervare> _rezervari;
    private readonly RezervareFisier _fisier = new RezervareFisier();
    private AdministrareCamere _ac = new AdministrareCamere();
    public AdministrareRezervari()
    {
        
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
        if (VerificaDisponibilitate(noua.CameraRezervata.Numar, noua.DataSosire, noua.DataPlecare))
        {
            _ac.SetareStatusCamera(noua.CameraRezervata.Numar,StatusCamera.Ocupata);
            _rezervari.Add(noua);
            _fisier.SalveazaRezervari(_rezervari);
        }
        else 
            Console.WriteLine("Camera e ocupata!");
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
}