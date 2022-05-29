using Microsoft.AspNetCore.Mvc;
using mvc.Inpost;
using mvc.Models.Magazyn;

namespace mvc.Controllers
{
    public class MagazynController : Controller
    {
        private readonly PaczkaViewModel _paczkaViewModel;

        public MagazynController(PaczkaViewModel paczkaViewModel)
        {
            _paczkaViewModel = paczkaViewModel;
        }


        // GET MagazynController/
        public IActionResult Index()
        {
            return View(_paczkaViewModel);
        }

        // GET MagazynController/Paczka/5
        public IActionResult Paczka(int id)
        {
            _paczkaViewModel.Id = id;

            return View(_paczkaViewModel);
        }

        // GET MagazynCotroller/WyslijPaczke
        public IActionResult WyslijPaczke()
        {
            return View();
        }

        // POST MagazynCotroller/WyslijPaczke
        [HttpPost]
        public IActionResult WyslijPaczke(Paczka paczka)
        {
            _paczkaViewModel.WyslijPaczke(paczka);

            return RedirectToAction("Index");
        }

        // POST MagazynController/UsunPaczke
        [HttpPost]
        public IActionResult UsunPaczke(int id)
        {
            _paczkaViewModel.Id = id;

            _paczkaViewModel.UsunPaczke();

            return RedirectToAction("Index");
        }

        public IActionResult OdswiezPaczke(int id)
        {
            _paczkaViewModel.Id = id;

            return View(_paczkaViewModel);
        }

        [HttpPost]
        public IActionResult OdswiezPaczke([FromRoute] int id, Paczka paczka)
        {
            _paczkaViewModel.OdswiezPaczke(id, paczka);

            return RedirectToAction("Index");
        }
    }
}
