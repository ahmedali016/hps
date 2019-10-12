using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HPStask.App_Code;

namespace HPStask.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (allowuser())
            {
                return View();
            }
            return RedirectToAction("Login", "account");
        }

        public ActionResult PatientInfo()
        {
           
            if (allowuser())
            {
                int patientid = int.Parse(Session["user"].ToString());
                PatientOperation op = new PatientOperation();
                var patient= op.displayPatientById(patientid);
                return View(patient);
            }
            return RedirectToAction("Login", "account");
        }
        [HttpPost]
        public ActionResult PatientInfo(ViewModel.patient patient)
        {
            if (allowuser())
            {
                int patientid = int.Parse(Session["user"].ToString());
                PatientOperation op = new PatientOperation();
                patient.userid = patientid;
                op.editPatient(patient);
                var patent = op.displayPatientById(patientid);
                return View(patent);
            }
            return RedirectToAction("Login", "account");
        }

        public ActionResult PatientResult()
        {
            if (allowuser())
            {
                int patientid = int.Parse(Session["user"].ToString());
                PatientOperation op = new PatientOperation();
                var result= op.displayPatientResult(patientid);
                return View(result);
            }
            return RedirectToAction("Login", "account");
        }

        private bool allowuser()
        {
            if (Session["user"] != null)
            {
                int id;int.TryParse(Session["user"].ToString(), out id);
                if (id > 0)
                {
                    LoginOperation op = new LoginOperation();
                    return op.checkUser(id);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}