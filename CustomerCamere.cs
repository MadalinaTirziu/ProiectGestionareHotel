using Hotel.Room.Model;
using System.Windows;
namespace Hotel.Room.Customer;

public class CustomerCamere
{
    private List<Camera> _camere;

    public CustomerCamere(List<Camera> camere)
    {
        _camere = camere;
    }

    public void AfiseazaCamereLibere()
    {
        List<Camera> camereLibere = new List<Camera>();
        
        foreach (Camera camera in _camere)
        {
            if (camera.StatusCamera == StatusCamera.Libera)
            {
                camereLibere.Add(camera);
            }
        }

        if (camereLibere.Count == 0)
        {
            MessageBox.Show("Nu există camere disponibile.");
            return;
        }

        string mesaj = "Camere disponibile:\n\n";

        foreach (Camera camera in camereLibere)
        {
            mesaj += $"• Camera {camera.Numar}\n";
        }

        MessageBox.Show(mesaj, "Camere libere");
    }
}