using Hotel.Room.Model;
using Hotel.Reservation.Model;

namespace Hotel.Customer.Services;

public interface ICustomerService
{
    Camera CautaCameraDupaNumar(int numar);
    List<Camera> CautaCameraDupaStatus(StatusCamera status);
    void AdaugaRezervare(Rezervare rez);
}