using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace family_icecream.Controllers
{
    [Route("[controller]")]
    public class FamilyController : Controller
    {
        protected string jsonUrl = "./json_file/高雄市.json";
        protected string iceUrl = "./json_file/高雄市店家冰品.json";
        public static FamilyIce result = new FamilyIce();
        /*[Route("Hey")]
        public string Hey()
        {
            return "Hey";
        }

        [HttpGet("GetFlaver/{id}")]
        public FamilyIce GetIceFromFlaver(string input)
        {

            return result;
        }
        */
        [HttpGet("StoreNameList")]
        public FamilyStoreDB GetAllStoreList()
        {
            FamilyStoreDB familyObj = CreateFamilyDB();

            return familyObj;
        }
        [HttpPost("AddIceFlaver")]
        public List<FamilyIce> AddIceFlaver([FromBody] List<FamilyIce> iceText)
        {

            List<FamilyIce> _resultList = iceText;
            List<FamilyIce> _InitOrigin = CreateFamilyIceDB();
            FamilyStoreDB familyObj = CreateFamilyDB();
            Boolean Ok = Verity_storyName(iceText);

            //如果找到店家就取代，如果沒找到店家就插入新的 (前提是有那家店)
            if (Ok == false)
            {
                return null;
            }
            for(int i = 0; i < _resultList.Count; i++)
            {
                bool NewData = true;
                int index = 0;
                foreach(var data in _InitOrigin)
                {
                    if(data.storeName == _resultList[i].storeName)
                    {
                        NewData = false;
                        _InitOrigin[index].flavor = _resultList[i].flavor;
                        break;
                    }
                    index++;
                }
                if (NewData)
                {
                    _InitOrigin.Add(_resultList[i]);
                }

            }
            
            
            //純粹寫入...json檔格式範例
            /*
             [
                {
                  "Name": Name,
                  "Machine":Machine,
                  "Flaver":Flaver
                }
             ]
             */


            string result = JsonSerializer.Serialize(_InitOrigin);
            System.IO.File.WriteAllText(iceUrl,result);
            
            
            return _resultList;
        }

        [HttpGet("GetIceStore")]
        public List<FamilyOutResult> GetIceStore(string keyValue)
        {
            string result = "";
            List<FamilyOutResult> outResult = new List<FamilyOutResult>();
            
            using (StreamReader sr = new StreamReader(iceUrl))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result = result + line;
                }
            }
            List<FamilyIce> FamilyObj = JsonSerializer.Deserialize<List<FamilyIce>>(result);
            for (int i = 0; i < FamilyObj.Count; i++)
            {
                if (FamilyObj[i].flavor.IndexOf(keyValue) > -1 )
                {
                    outResult.Add(new FamilyOutResult { storeName = FamilyObj[i].storeName });
                }
                
            }

            
            
            return outResult;
        }

        [HttpGet("GetIceStore/all")]
        public List<FamilyIce> GetIceStoreAll()
        {
            string result = "";

            using (StreamReader sr = new StreamReader(iceUrl))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result = result + line;
                }
            }
            List<FamilyIce> FamilyObj = JsonSerializer.Deserialize<List<FamilyIce>>(result);


            return FamilyObj;
        }


        //建立FamilyDB
        protected FamilyStoreDB CreateFamilyDB()
        {
            string result = "";
            using (StreamReader sr = new StreamReader(jsonUrl))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result = result + line;
                }
            }
            FamilyStoreDB FamilyObj = JsonSerializer.Deserialize<FamilyStoreDB>(result);
            return FamilyObj;
        }

        //建立IceDB
        protected List<FamilyIce> CreateFamilyIceDB()
        {
            string result = "";
            using (StreamReader sr = new StreamReader(iceUrl))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    result = result + line;
                }
            }
            List<FamilyIce> FamilyIceObj = JsonSerializer.Deserialize<List<FamilyIce>>(result);
            return FamilyIceObj;
        }

        //驗證是否存在店家
        protected Boolean Verity_storyName(List<FamilyIce> data)
        {

            Boolean result = false;
            FamilyStoreDB familObj = CreateFamilyDB();
            int jsonStoreCnt = 0;//如果數的量和全家找到現有量一樣就代表合法
            
            for(int i = 0; i < data.Count; i++)
            {
                if(familObj.stores.Find(item => item.NAME == data[i].storeName)!=null)
                {
                    jsonStoreCnt += 1;
                    if(jsonStoreCnt == data.Count)
                    {
                        return true;
                    }
                } 
            }


            
            return false;
        }


        //建立modal 型態
        public class FamilyOutResult
        {
            public string storeName { get; set; }
        }

        

        
    }
}

