using Hotel.Customer.Model;
using Hotel.Reservation.Admin;
using Hotel.Reservation.Model;
using System.Collections.Generic;
using Hotel.Room.Model;

namespace Hotel.Room.Customer
{
    public class CustomerRezervare
    {
        private Hotel.Customer.Model.Customer _customer;
        private AdministrareRezervari _adminRezervari;

        public CustomerRezervare(Hotel.Customer.Model.Customer customer, AdministrareRezervari adminRezervari)
        {
            _customer = customer;
            _adminRezervari = adminRezervari;
        }

        public void RezervaCamera(Camera camera, DateOnly dataSosire, DateOnly dataPlecare, int numarPersoane)
        {
            Rezervare rezervare = new Rezervare(camera, _customer, dataSosire, dataPlecare, numarPersoane);

            _adminRezervari.AdaugaRezervare(rezervare);
        }
        
        public void AnuleazaRezervare(Rezervare rezervare)
        {
            _adminRezervari.ModificaStatus(rezervare, StatusRezervare.Anulata);
        }

        public List<Rezervare> RezervariActive()
        {
            List<Rezervare> rezultat = new List<Rezervare>();

            foreach (Rezervare r in _adminRezervari.AfisareRezervari())
            {
                if (r.PersoanaRezervare == _customer &&
                    r.StatusRezervare == StatusRezervare.Activa)
                {
                    rezultat.Add(r);
                }
            }

            return rezultat;
        }
        
        public List<Rezervare> IstoricSejururi()
        {
            List<Rezervare> rezultat = new List<Rezervare>();

            foreach (Rezervare r in _adminRezervari.AfisareRezervari())
            {
                if (r.PersoanaRezervare == _customer &&
                    r.StatusRezervare == StatusRezervare.Finalizata)
                {
                    rezultat.Add(r);
                }
            }

            return rezultat;
        }
    }
}