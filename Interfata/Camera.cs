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
    
    public override bool Equals(object? other)
    {
        if(other == null) return false;
        Camera OtherCamera = other as Camera;
        if (OtherCamera == null) return false;
        return Numar == OtherCamera.Numar && StatusCamera == OtherCamera.StatusCamera;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Numar, (int)StatusCamera);
    }
}