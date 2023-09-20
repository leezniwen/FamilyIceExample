using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using family_icecream.Contents;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace family_icecream.Controllers
{
    [Route("[controller]")]
    public class WebController : Controller
    {

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            List<FamilyIce> result = new List<FamilyIce>();
            APIFunc ApiFunc = new APIFunc();
            result = ApiFunc.GetData();
            if ( TempData["UpdateData"] != null)
            {
                TempData["UpdateData"] = TempData["UpdateData"].ToString();
            }
            return View("Index",result);
        }

        //傳入店家參數並且取得他的冰種
        [HttpGet("Update")]
        public IActionResult Update(FamilyIce ice )
        {
            
            return View("Update" , ice);
        }

        [HttpPost("Update")]
        public IActionResult Update(string id , string flaver)
        {
            string result = "";
            List<FamilyIce> iceList = new List<FamilyIce>();
            FamilyIce ice = new FamilyIce { storeName = id, flavor = flaver };
            iceList.Add(ice);
            APIFunc ApiFunc = new APIFunc();
            result = ApiFunc.SendStoreName(iceList);
            if (result == "")
            {
                TempData["UpdateData"] = "此更新資料不存在";
            }
            return RedirectToAction("Index", "Web");
        }
        
        [HttpGet("Create")]
        public IActionResult Create()
        {
            //傳入店家的店
            List<string> result = new List<string>();
            APIFunc ApiFunc = new APIFunc();
            result = ApiFunc.GetAllStoreName();
            return View("Create",result);
        }

        [HttpPost("Create")]
        public IActionResult Create(FamilyIce ice)
        {
            string result = "";
            List<FamilyIce> iceList = new List<FamilyIce>();
            iceList.Add(ice);
            APIFunc ApiFunc = new APIFunc();
            result = ApiFunc.SendStoreName(iceList);
            return RedirectToAction("Index", "Web");
        }
    }
}

