using Evaluation.Models;
using Evaluation.Models.BL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Evaluation.Controllers
{
    public class EvaController : Controller
    {
        // GET: Eva
        public ActionResult Index()
        {
            List<DataPoint> dataPoints = new List<DataPoint>{
                new DataPoint(10, 22),
                new DataPoint(20, 36),
                new DataPoint(30, 42),
                new DataPoint(40, 51),
                new DataPoint(50, 46),
            };

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        #region report user table


/*
        public ActionResult Report()
        {
            //Print THe Bill


            LocalReport localRpt = new LocalReport();
            localRpt.ReportPath = Server.MapPath("~/Reports/Report1.rdlc");

            ReportParameter[] parameters = new ReportParameter[]
            {
                new ReportParameter("CrsPrice",Session["PriceValue"].ToString()),
                new ReportParameter("CrsName",Session["CourseName"].ToString()),
                new ReportParameter("UserName",Session["AName"].ToString())
        }; localRpt.SetParameters(parameters);
            //localRpt.DataSources.Add(rds);
            string mimeType;
            string enconding;
            string fileNameExtension = "pdf";

            string[] streams;
            Warning[] warning;
            byte[] renderedByte;

            renderedByte = localRpt.Render("PDF", "", out mimeType, out enconding, out fileNameExtension
                , out streams, out warning);
            Response.AddHeader("content-disposition", "attachment;filename=wasl.pdf");
            return File(renderedByte, fileNameExtension);
            //End of Printing
        }

    */

        #endregion

        #region userm
        public ActionResult UserEva()
        {
            return View();
        }

        #endregion

        #region LogOut
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("LogIn");
        }
       #endregion

        #region LogIn
        [HttpGet]
        public ActionResult LogIn()
        {
            

            return View();

        }


        [HttpPost]
        public ActionResult LogIn(string username, string pwd)
        {
            UserInfo u = new UserInfo();

            if (UserBL.CheckExistentUser(username, pwd, u))
            {
                ViewBag.UserId = u.userId;
                Session["uname"] = u.username;
                Session["userId"] = u.userId;

                if (u.authId == 71)
                    return RedirectToAction($"createUser");

                return RedirectToAction($"user");
            }
            return View();
        }
        #endregion

        #region CourseManagement
        [HttpGet]
        public ActionResult CourseManagement()
        {

            return View(CourseBL.DisplayAll());
        }
        #endregion

        #region addCourse
        [HttpGet]
        public ActionResult AddCourse()
        {
            ViewBag.getLev = CourseBL.GetAllLevel();
            ViewBag.getDoc = CourseBL.GetAllDoctors("دكتور");
            ViewBag.getAssistant = CourseBL.GetAllDoctors("معيد");
            ViewBag.getSem = CourseBL.GetAllSemesters();
            ViewBag.getSpe = CourseBL.GetAllSpecializations(); ;
            return View();
        }
        [HttpPost]
        public ActionResult AddCourse(Course c, CrsRelation cr , HttpPostedFileBase file)
        {
            var path = "";
            string ImageName = "";
             //for checking uploaded
            if (Path.GetExtension(file.FileName).ToLower() == ".jpg"
                   || Path.GetExtension(file.FileName).ToLower() == ".png"
                    || Path.GetExtension(file.FileName).ToLower() == ".gif"
                     || Path.GetExtension(file.FileName).ToLower() == ".jpeg")
            {

                ImageName = file.FileName;
                path = Path.Combine(Server.MapPath("~/Images/"), file.FileName);

                file.SaveAs(path);

                ViewBag.UploadSuccess = true;

            }
            cr.cid = c.cid;
            CourseBL.InsertToCrs(c, ImageName);
            CourseBL.InsertToCrsRelation(cr);
            Session["CourseID"] = c.cid;

            return RedirectToAction("AddCourse");
        }



        #endregion

        #region editCourse
        [HttpGet]
        public ActionResult EditCourse(int id)
        {


            ViewBag.Model = CourseBL.getCourseById(id);
            ViewBag.getLev = CourseBL.GetAllLevel();
            ViewBag.getDoc = CourseBL.GetAllDoctors("دكتور");
            ViewBag.getAssistant = CourseBL.GetAllDoctors("معيد");
            ViewBag.getSem = CourseBL.GetAllSemesters();
            ViewBag.getSpe = CourseBL.GetAllSpecializations(); 
            return View();
        }
        [HttpPost]
        public ActionResult EditCourse(Course c, CrsRelation cr, HttpPostedFileBase file)
        {
            var path = "";
            string ImageName = "";

            //for checking uploaded
            if (file != null && (Path.GetExtension(file.FileName).ToLower() == ".jpg"
                   || Path.GetExtension(file.FileName).ToLower() == ".png"
                    || Path.GetExtension(file.FileName).ToLower() == ".gif"
                     || Path.GetExtension(file.FileName).ToLower() == ".jpeg"))
            {

                ImageName = file.FileName;
                path = Path.Combine(Server.MapPath("~/Images/"), file.FileName);
                file.SaveAs(path);
                c.img = file.FileName;
                ViewBag.UploadSuccess = true;

            }
            CourseBL.UpdateCourse(c);
            CourseBL.UpdateCrsRelation(cr);

            

            
            return RedirectToAction("EditCourse");
        }


        #endregion

        #region deactiveCourse

        public ActionResult DeactiveCourse(int cid , int active)
        {
            CourseBL.DeactiveCourse(cid,active);
            return RedirectToAction("CourseManagement");
        }
       
      

        #endregion

        #region createUsers

        public class RandomGenerator
        {
            // Generate a random number between two numbers

            public int RandomNumber(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }

            // Generate a random string with a given size  
            public  string RandomString(int size, bool lowerCase)
            {
                StringBuilder builder = new StringBuilder();
                Random random = new Random();
                char ch;
                for (int i = 0; i < size; i++)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                    builder.Append(ch);
                }
                if (lowerCase)
                    return builder.ToString().ToLower();
                return builder.ToString();
            }

            // Generate a random password  
            public string RandomPassword()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(RandomString(4, true));
                builder.Append(RandomNumber(1000, 9999));
                builder.Append(RandomString(2, false));
                return builder.ToString();
            }
        }

        

        [HttpGet]
        public ActionResult createUser()
        {
           

            return View(UserBL.GetAllUsers(70));
        }

        [HttpPost]
        public ActionResult createUser(UserInfo u)
        {
            //int rand = generator.RandomNumber(5, 100);
            // string str = generator.RandomString(10, false);
            RandomGenerator generator = new RandomGenerator();
            bool hasPass = true;
            bool hasName = true;
            string uName;
            string pwd;
           int maxId= UserBL.GetMaxId();
           //int count = 20;
            List<UserInfo> uinfo = new List<UserInfo>();
            for(int i=maxId+1;i<=maxId+u.Count;)
            {
                
                uName= generator.RandomString(10, false);
                 pwd= generator.RandomPassword();

                hasName = uinfo.Any(ww => ww.username == uName);
                hasPass = uinfo.Any(ww => ww.pwd == pwd);
                if (!hasPass&&!hasName){

                    uinfo.Add(
                             new UserInfo
                             {
                                 userId = i,
                                 username =uName,
                                 pwd = pwd

                             });
                   
                    u.userId = i;
                    u.username = uName;
                    u.pwd = pwd;
                    UserBL.InsertToUserInfo(u);
                    i++;
                }
                hasName = true;
                hasPass = true;
                
                

            }
            //return View();
            return RedirectToAction("createUser");
        }

        #endregion

        #region createAdmin

        [HttpGet]
        public ActionResult createAdmin()
        {


            return View();
        }

        [HttpPost]
        public ActionResult createAdmin(UserInfo u)
        {

            UserBL.CreateAdmin(u);
          
           return RedirectToAction("createAdmin");
        }

        #endregion

        #region userManagement
        [HttpGet]
        public ActionResult UserManagement()
        {


            return View(UserBL.GetAllUsers(70));
        }


        #region deactiveuser

        public ActionResult deActive(int userId, int active)
        {
            UserBL.deActive(userId , active);
            return RedirectToAction("createUser");
        }



        #endregion
        #endregion

        #region adminManagement
        [HttpGet]
        public ActionResult AdminManagement()
        {

            return View(UserBL.GetAllUsers(71));
        }


        #region deactiveadmin

        public ActionResult deActiveAdmin(int userId, int active)
        {
            UserBL.deActive(userId, active);
            return RedirectToAction("AdminManagement");
        }



        #endregion
        #endregion

        #region changeadminpass
        [HttpGet]
        public ActionResult changePass()
        {

            return View(UserBL.getUserPassById(int.Parse(Session["UserId"].ToString())));

        }

        [HttpPost]
        public ActionResult changePass(UserInfo U)
        {
            UserBL.changeAdminPwd(U.userId,U.pwd);
            return RedirectToAction("changePass");
        }


        #endregion

        #region sectionManagement
        [HttpGet]
        public ActionResult sectionManagement()
        {

            return View(SectionBL.DisplayAllSectionsToManag());
        }

        #region deactiveSection

        [HttpGet]
        public ActionResult deActiveSec(int secId, int active)
        {
            SectionBL.deActiveSec(secId, active);

            return RedirectToAction("sectionManagement");
        }


        [HttpGet]
        public ActionResult deActiveQues(int quesId, int active)
        {
            SectionBL.deActiveQues(quesId, active);

            return RedirectToAction("questionManagement");
        }
        #endregion


        #region addSection
        [HttpGet]
        public ActionResult AddSec()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult AddSec(Models.Section s)
        {
            
          
            SectionBL.InsertToSec(s);
           
            Session["SecId"] = s.secId;

            return RedirectToAction("AddSec");
        }




        #endregion

        #region editSection
        public ActionResult secManagement()
        {
            return View(SectionBL.DisplayAllSections());
        }

        [HttpPost]
        public ActionResult EditSec(int secId, string sec)
        {
            SectionBL.UpdateSec(secId, sec);
            return RedirectToAction("secManagment");
        }

        #endregion

        #region addQues
        [HttpGet]
        public ActionResult addQues()
        {
            ViewBag.getSec = SectionBL.DisplayAllSectionsToManag();

            return View();
        }
        [HttpPost]
        public ActionResult addQues(Question q)
        {

            SectionBL.InsertToQues(q);

            Session["quesId"] = q.quesId;

            return RedirectToAction("addQues");
        }


        #endregion

        #endregion

       
        public ActionResult questionManagement(int id=-1)
        {
          
            ViewBag.getSec = SectionBL.DisplayAllSectionsThatHaveQuestion();
            ViewBag.getQues = SectionBL.DisplayAllQuestions(id);
            return View();
        }
        

        [HttpPost]
        public ActionResult EditQues(int quesId, string ques)
        {
           SectionBL.UpdateQues(quesId,ques);
            return RedirectToAction("questionManagment");
        }

       
       




        public ActionResult questionManag()
        {

            return View(SectionBL.DisplayAllQuestionsOfSpecificSection(1));
        }
        [HttpPost]
        public ActionResult saveuser(int id, string ques)
        {
           

            //Update data to database 
            SectionBL.UpdateQues(id, ques);
           
            return RedirectToAction("questionManag");
        }
       

       

        public JsonResult ChangeUser(Question q)
        {
            // Update model to your db  
            string message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);
        }



        #region user
        [HttpGet]
        public ActionResult user(int id=-1 )
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LogIn");
            }
            else
            {

                ViewBag.getCrs = CourseBL.DisplayAll();
                ViewBag.Model = CourseBL.getCourseById(id);
                ViewBag.Doc = CourseBL.getInsByCid(id, "دكتور");
                ViewBag.Assistant = CourseBL.getInsByCid(id, "معيد");
                ViewBag.getSpe = CourseBL.GetSpecializationsByCid(id);
                ViewBag.getSec = SectionBL.DisplayAllSections();

                ViewBag.getQues = SectionBL.DisplayAllQuestions();
                ViewBag.SelectedItemDDL = id;
                // UserInfo u = new UserInfo();
                //Session["UserId"] = u.userId;
                ViewBag.UserId = Session["userId"];

                return View();
            }
        }
        [HttpPost]
        public ActionResult user(int [] rating, int[] qust, Course c,Question q)
        {
            
            if (Session["UserId"] == null)
            {
                return RedirectToAction("LogIn");
            }
            else {
                //c.cid = ViewBag.Model;
                //q.quesId = ViewBag.getQues;
                //q.secID = ViewBag.getSec;
                int uid = int.Parse(Session["userId"].ToString());
               // int rate = 1;
                
                for (int i = 0; i < qust.Length; i++)
                {
                    UserBL.InsertSubmitTbl(uid,c.cid , qust[i], rating[i]);

                }

            }


            //  ViewBag.Model = CourseBL.getCourseById(id);
            return RedirectToAction("user");
        }

        #endregion

        [HttpGet]
        public ActionResult rate()
        {

              return View(UserBL.GetEvaOfQues(6,1337));
        }



        public ActionResult Rating(int cid = -1)
        {
            courseId = cid;
            ViewBag.getCrs = CourseBL.DisplayAll();
            ViewBag.getRateQues = submitBL.getRateQuestionVal(cid);
            ViewBag.getTitlesForCourse = submitBL.getTitlesForCourse(cid);
            ViewBag.getSumittionCount = submitBL.getSumittionCount(cid);

            return View();
        }

        static int courseId=-1;

        [HttpGet]
        public ActionResult rateQuesTable(int cid=-1)
        {   
            ViewBag.getCrs = CourseBL.DisplayAll();
            ViewBag.getRateQues = submitBL.getRateQuestionVal(cid);
            ViewBag.getTitlesForCourse = submitBL.getTitlesForCourse(cid);
            ViewBag.getSumittionCount = submitBL.getSumittionCount(cid);
            return View();
        }



        [HttpGet]
        public ActionResult GetData()
        {
            submitBL sBLobj = new submitBL();
           List<submit> sObj = new List<submit>();
            CountofRates crates = new CountofRates();

            sObj= sBLobj.getRateValue(courseId);

            foreach (var value in sObj)
            {
                if (value.rateId == 5)
                    crates.Distinct = value.ratevalue;
                else
                    if (value.rateId == 4)
                    crates.VeryGood = value.ratevalue;
                else
                    if (value.rateId == 3)
                    crates.Good = value.ratevalue;
                else
                    if (value.rateId == 2)
                    crates.Normal = value.ratevalue;
                else
                    if (value.rateId == 1)
                    crates.Bad = value.ratevalue;
            }

            return Json(crates, JsonRequestBehavior.AllowGet);
        }


        #region userDataPdf

        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 100f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
               pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Grid.pdf");
            }
        }
        #endregion



    }








    public class CountofRates
    {
        public int Distinct { get; set; }
        public int VeryGood { get; set; }
        public int Good { get; set; }
        public int Normal { get; set; }
        public int Bad { get; set; }




    }
}