namespace Hotel.Reservation.Model;
using Hotel.Room.Model;
using Hotel.Customers.Model;

public enum StatusRezervare
{
    Activa,
    Anulata,
    Finalizata
}

public class Rezervare
{
    public Camera CameraRezervata { get; set; }
    public Customer PersoanaRezervare { get; set; } 
    public DateOnly DataSosire { get; set; }
    public DateOnly DataPlecare  { get; set; }
    public int NumarPersoane { get; set; }
    public StatusRezervare StatusRezervare { get; set; }

    public Rezervare()
    {
        
    }

    public Rezervare(Camera camera, Customer persoanaRezervare, DateOnly dataSosire, DateOnly dataPlecare,
        int numarPersoane, StatusRezervare statusRezervare = StatusRezervare.Activa)
    {
        CameraRezervata = camera;
        PersoanaRezervare = persoanaRezervare;
        DataSosire = dataSosire;
        DataPlecare = dataPlecare;
        NumarPersoane = numarPersoane;
        StatusRezervare = statusRezervare;
    }

    public void ModificareStatus(StatusRezervare status)
    {
        StatusRezervare = status;
    }
}