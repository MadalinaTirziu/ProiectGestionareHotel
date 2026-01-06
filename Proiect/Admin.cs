using Hotel.Room.Model;

namespace Hotel.Admin.Model;
using Hotel.Users.Model;
using Hotel.Room.Admin;

public class Admin : User
{
    private readonly AdministrareCamere _adminCamere;
    public Admin(string username, string password, AdministrareCamere adminCamere) : base(username, password)
    {
        _adminCamere = adminCamere;
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
    public override UserRole Role => UserRole.Admin;
}