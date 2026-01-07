using Hotel.Customer.Model;
using Hotel.Reservation.Admin;
using Hotel.Reservation.Model;
using System.Collections.Generic;
using Hotel.Room.Model;
using Hotel.Customer.Services;
namespace Hotel.Room.Customer
{
    public class CustomerRezervare
    {
        private Hotel.Customer.Model.Customer _customer;
        private readonly ICustomerService _service;

        public CustomerRezervare(Hotel.Customer.Model.Customer customer,  ICustomerService service)
        {
            _customer = customer;
            _service = service;
        }

        public void RezervaCamera(Camera camera, DateOnly dataSosire, DateOnly dataPlecare, int numarPersoane)
        {
            Rezervare rezervare = new Rezervare(camera, _customer, dataSosire, dataPlecare, numarPersoane);

            _service.AdaugaRezervare(rezervare);
        }
        
        public void AnuleazaRezervare(Rezervare rezervare)
        {
            _service.AnulareRezervare(rezervare);
        }

        public List<Rezervare> RezervariActive()
        {
            List<Rezervare> rezultat = new List<Rezervare>();

            if (_service is AdministrareRezervari admin)
            {
                foreach (Rezervare r in admin.AfisareRezervari())
                {
                    if (r.PersoanaRezervare == _customer &&
                        r.StatusRezervare == StatusRezervare.Activa)
                    {
                        rezultat.Add(r);
                    }
                }
            }

            return rezultat;
        }
        
        public List<Rezervare> IstoricSejururi()
        {
            List<Rezervare> rezultat = new List<Rezervare>();

            if (_service is AdministrareRezervari admin)
            {
                foreach (Rezervare r in admin.AfisareRezervari())
                {
                    if (r.PersoanaRezervare == _customer && r.StatusRezervare == StatusRezervare.Finalizata)
                    {
                        rezultat.Add(r);
                    }
                }
            }

            return rezultat;
        }
    }
}