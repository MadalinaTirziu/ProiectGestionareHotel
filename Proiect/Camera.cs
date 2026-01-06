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
    public int Numar { get; private set; }
    public StatusCamera StatusCamera { get; private set; }

    public Camera(int numar, StatusCamera statusInitial = StatusCamera.Libera)
    {
        Numar = numar;
        StatusCamera = statusInitial;
    }

    public void ModificareStatus(StatusCamera status)
    {
        StatusCamera = status;
    }

    public string ToFileFormat()
    {
        return $"{Numar};{StatusCamera}";
    }
    
}