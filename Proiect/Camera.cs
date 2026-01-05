namespace ProiectPoo;

public enum StatusCamera
{
    Libera,
    Ocupata,
    InCuratienie,
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
    
}