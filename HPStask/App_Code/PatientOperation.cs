using HPStask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace HPStask.App_Code
{
    public class PatientOperation
    {
        HPSDBEntities hp = new HPSDBEntities();
        public bool editPatient(ViewModel.patient patient)
        {
            var user = hp.Patients.FirstOrDefault(c => c.id == patient.userid);
            if (user != null)
            {
                LoginOperation op = new LoginOperation();
                MD5 md = MD5.Create();
                if (patient.pass!=string.Empty && patient.pass!=null)
                {
                    var hash = op.GetMd5Hash(md, patient.pass);
                    if (user.pass != hash)
                    {
                        user.pass = hash;
                    }
                }
                
                user.birthDate = DateTime.Parse(patient.birthdate);
                if(patient.phone!=string.Empty && patient.phone!=null)
                user.phone = patient.phone;                
                hp.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public ViewModel.patient displayPatientById(int patientId)
        {
            var user = hp.Patients.Where(c => c.id == patientId).ToList();
            if (user.Count > 0)
            {
                var userInfo = user.Select(c => new ViewModel.patient { patientName=c.patientName, birthdate = c.birthDate.ToShortDateString(), phone = c.phone, user = c.userName,mail=c.mail });
                return userInfo.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        public ViewModel.patientResult displayPatientResult(int patientId)
        {
            var res = hp.PatientResults.Where(c=>c.id==patientId).Select(c => new ViewModel.patientResult
            {
                patient = c.Patients.patientName,
                result = c.result,
                resultDate = c.resultDate
            }).FirstOrDefault();
            return res;
        }
    }
}