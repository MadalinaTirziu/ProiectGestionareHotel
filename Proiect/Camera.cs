namespace Hotel.Room.Model;

public enum StatusCamera
{
    Libera,
    Ocupata,
    In_Curatenie,
    Indisponibila

}

public class Camera
{
    public int Numar { get; set; }
    public StatusCamera StatusCamera { get; set; }

    public Camera()
    {
        
    }
    public Camera(int numar, StatusCamera statusInitial = StatusCamera.Libera)
    {
        Numar = numar;
        StatusCamera = statusInitial;
    }

    public void ModificareStatus(StatusCamera status)
    {
        StatusCamera = status;
    }
    
    
}