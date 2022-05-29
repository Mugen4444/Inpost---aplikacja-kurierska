using Microsoft.AspNetCore.Mvc;
using inpost;
using mvc.Models;
using mvc.Models.Magazyn;
using System.Diagnostics;


namespace mvc.Controllers
{
    public class MagazynController : Controller
    {
        private readonly PaczkaViewModel _paczkaViewModel;
        private readonly ErrorViewModel _errorViewModel;
        private readonly ILogger<MagazynController> _logger;

        public MagazynController(PaczkaViewModel paczkaViewModel, ErrorViewModel errorViewModel, ILogger<MagazynController> logger)
        {
            _paczkaViewModel = paczkaViewModel;
            _errorViewModel = errorViewModel;
            _logger = logger;
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
            try
            {
                _paczkaViewModel.WyslijPaczke(paczka);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _errorViewModel.Message = ex.Message;
                return RedirectToAction("Error");
            }
        }

        // POST MagazynController/UsunPaczke
        [HttpPost]
        public IActionResult UsunPaczke(int id)
        {
            _paczkaViewModel.Id = id;

            try
            {
                _paczkaViewModel.UsunPaczke();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _errorViewModel.Message = ex.Message;
                return RedirectToAction("Error");
            }
        }

        public IActionResult OdswiezPaczke(int id)
        {
            _paczkaViewModel.Id = id;

            return View(_paczkaViewModel);
        }

        [HttpPost]
        public IActionResult OdswiezPaczke([FromRoute] int id, Paczka paczka)
        {
            try
            {
                _paczkaViewModel.OdswiezPaczke(id, paczka);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _errorViewModel.Message = ex.Message;
                return RedirectToAction("Error");
            }
        }

        public IActionResult Error()
        {
            _logger.LogError(_errorViewModel.Message);

            string msg = _errorViewModel.Message!;

            _errorViewModel.Message = null;

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = msg
            });
        }
    }
}
