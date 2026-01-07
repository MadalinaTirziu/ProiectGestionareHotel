using Hotel.Reservation.Admin;
using Hotel.Room.Model;
using Hotel.Reservation.Model;

namespace Hotel.Admin.Model;
using Hotel.Users.Model;
using Hotel.Room.Admin;

public class Admin : User
{
    private readonly AdministrareCamere _adminCamere;
    private readonly AdministrareRezervari _adminRezervari;
    public Admin(string username, string password, AdministrareCamere adminCamere, AdministrareRezervari administrareRezervari) : base(username, password)
    {
        _adminCamere = adminCamere;
        _adminRezervari = administrareRezervari;
    }

    public void AdaugareCamera(int numar)
    {
        _adminCamere.AdaugaCamera(numar);
    }

    public void ModificareStatusCamera(int numar, StatusCamera status)
    {
        _adminCamere.SetareStatusCamera(numar, status);
    }

    public void EliminareCamera(int numar)
    {
        _adminCamere.StergeCamera(numar);
    }

    public void VizualizareCamere()
    {
        var camere =  _adminCamere.AfisareCamere();
        foreach (var cam in camere)
        {
            Console.WriteLine($"Camera : {cam.Numar} | Status : {cam.StatusCamera}");
        }
    }

    public void AdaugaRezervare(Rezervare rez)
    {
        _adminRezervari.AdaugaRezervare(rez);
    }

    public void ModificaStatusRezervare(Rezervare rez, StatusRezervare status)
    {
        _adminRezervari.ModificaStatus(rez, status);
    }

    public void AfisareRezervari()
    {
        var rezervari = _adminRezervari.AfisareRezervari();
        foreach (var rez in rezervari)
        {
            Console.WriteLine($"Camera rezervata : {rez.CameraRezervata}" +
                              $"Persoana : {rez.PersoanaRezervare}" +
                              $"Data sosire : {rez.DataSosire}" +
                              $"Data plecare :  {rez.DataPlecare}" +
                              $"Numar persoane :  {rez.NumarPersoane}" +
                              $"Status : {rez.StatusRezervare}");
        }
    }
    public override UserRole Role => UserRole.Admin;
}