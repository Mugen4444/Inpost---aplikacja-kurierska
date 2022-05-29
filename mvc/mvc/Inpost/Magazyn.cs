namespace mvc.Inpost
{
    public class Magazyn
    {
        private readonly List<Paczka> paczki;
        private readonly List<Klient> klienci;

        public Magazyn()
        {
            paczki = new List<Paczka>();
            klienci = new List<Klient>();
        }

        // Create
        public void DodajPaczke(Paczka paczka)
        {
            paczka.Id = paczki.Count;

            while (paczki.Any(p => p.Id == paczka.Id))
            {
                paczka.Id++;
            }

            var uzupelniona = UzupelnijNadawceIOdbiorce(paczka);

            paczki.Add(uzupelniona);
        }

        // Read
        public IEnumerable<Paczka> Paczki => paczki;

        // Read (id)
        public Paczka Paczka(int id)
        {
            var paczka = paczki.Find(p => p.Id == id);

            if (paczka is null)
                throw new Exception($"Nie znaleziono paczki z id: {id}");

            return paczka;
        }

        // Update
        public void OdswiezPaczke(int id, Paczka odswiezona)
        {
            var paczka = paczki.Find(p => p.Id == id);

            if (paczka is null)
                throw new Exception($"Nie znaleziono paczki z id: {id}");

            odswiezona.Id = paczka.Id;

            var uzupelniona = UzupelnijNadawceIOdbiorce(odswiezona);

            paczki.Remove(paczka);
            paczki.Add(uzupelniona);
        }

        // Delete
        public void UsunPaczke(int id)
        {
            var paczka = paczki.Find(p => p.Id == id);

            if (paczka is null)
                throw new Exception($"Nie znaleziono paczki z id: {id}");

            paczki.Remove(paczka);
        }

        private Paczka UzupelnijNadawceIOdbiorce(Paczka paczka)
        {
            var nadawca = klienci.Find(k => k.Equals(paczka.Nadawca));
            var odbiorca = klienci.Find(k => k.Equals(paczka.Odbiorca));

            if (nadawca is not null)
                paczka.Nadawca = nadawca;
            else
            {
                paczka.Nadawca!.Id = klienci.Count;
                klienci.Add(paczka.Nadawca);
            }


            if (odbiorca is not null)
                paczka.Odbiorca = odbiorca;
            else
            {
                paczka.Odbiorca!.Id = klienci.Count;
                klienci.Add(paczka.Odbiorca);
            }

            return paczka;
        }
    }
}
