using Govy.Domain.Entities;
using Govy.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Govy.Portal.Controllers
{
    public class LogController : Controller
    {
        LogService _logService;
        public LogController()
        {
            _logService = new LogService();
        }
        public ActionResult Index()
        {
            var logs = _logService.GetAllLog();
            return View(logs);
        }
        public ActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Criar(Log _log)
        {
            if (ModelState.IsValid)
            {
                _logService.CriarLog(_log);
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Editar(string Id)
        {
            var log = _logService.GetLogById(Id);
            return View(log);
        }
        [HttpPost]
        public ActionResult Editar(Log _log)
        {
            if (ModelState.IsValid)
            {
                _logService.AtualizarLog(_log);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpGet]
        public ActionResult Delete(string Id)
        {
            var log = _logService.GetLogById(Id);
            return View(log);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string Id)
        {
            _logService.DeletarLog(Id);
            return RedirectToAction("Index");
        }
    }
}