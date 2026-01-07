using Hotel.Room.Model;
using System.Windows;
namespace Hotel.Room.Customer;
using Hotel.Customer.Services;
public class CustomerCamere
{
    private readonly ICustomerService _service;

    public CustomerCamere(ICustomerService service)
    {
        _service = service;
    }

    public Camera CautaDupaNumar(int numar)
    {
        return _service.CautaCameraDupaNumar(numar);
    }

    public List<Camera> CautaDupaStatus(StatusCamera status)
    {
        return _service.CautaCameraDupaStatus(status);
    }

    public void AfiseazaCamereLibere()
    {
        List<Camera> camereLibere = _service.CautaCameraDupaStatus(StatusCamera.Libera);

        if (camereLibere.Count == 0)
        {
            MessageBox.Show("Nu există camere disponibile.");
            return;
        }

        MessageBox.Show("Camere disponibile:\n");

        foreach (Camera c in camereLibere)
        {
            MessageBox.Show($"• Camera {c.Numar}");
        }
    }
}