using mvc.Inpost;

namespace mvc.Models.Magazyn
{
    public class PaczkaViewModel
    {
        private readonly Inpost.Magazyn _magazyn;

        public PaczkaViewModel(Inpost.Magazyn magazyn)
        {
            if (magazyn is null)
                throw new ApplicationException("Wystąpił błąd aplikacji");

            _magazyn = magazyn;
        }

        public IEnumerable<Paczka> Paczki => _magazyn.Paczki;

        public void OdswiezPaczke(int id, Paczka odswiezona) =>
            _magazyn.OdswiezPaczke(id, odswiezona);

        public int Id { get; set; }

        public Paczka Paczka => _magazyn.Paczka(Id);

        public void UsunPaczke() => _magazyn.UsunPaczke(Id);

        public void WyslijPaczke(Paczka paczka) => _magazyn.DodajPaczke(paczka);
    }
}
