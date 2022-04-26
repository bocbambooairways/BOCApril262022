using BOC.Areas.B_DCS.Models;
using BOC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;

namespace BOC.Areas.B_DCS.Controllers
{

   
    [Area("B-DCS")]
    public class FlightController : Controller
    {

        private IHostingEnvironment Environment;
        public UriConfig UriConfig { get; }
        public FlightController(Microsoft.Extensions.Options.IOptions<UriConfig> _UriConfig, IHostingEnvironment Environment)
        {
            UriConfig = _UriConfig.Value;
            this.Environment = Environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult M_Index(string Airport)
        {
            if(!string.IsNullOrEmpty(Airport))  
            ViewBag.Airport = Airport;  
            return View();  
        }
        public IActionResult M_FlightList()
        {
            
            string uri_Get_B_DCS_AirportList = UriConfig.uri_B_DCS_AirportList;


            IEnumerable<DCS_AirportList> Get_B_DCS_AirportList = new DataAPI().GetListAPIWithoutParams<DCS_AirportList>(UriConfig.uri_B_DCS_AirportList,
                                                                      HttpContext.Session.GetString("Token"), System.Net.Http.HttpMethod.Post, false, "Data").Result;

            return View(Get_B_DCS_AirportList);
        }
        public IActionResult M_SearchList()
        {
            return View();
        }
        [HttpPost]  
        public IActionResult M_SearchList(string Airports,string FlightDate)
        {
            string uri_DCS_Flight_Get = UriConfig.uri_B_DCS_SearchList;
            List<DCS_Flight_Get> DCS_Get_Search_List = new DataAPI().GetListOjectAPI<DCS_Flight_Get>(uri_DCS_Flight_Get,
                                                          HttpContext.Session.GetString("Token"),
                                                          HttpMethod.Post, false, "Data",
                                                          "FlightDate",
                                                           (FlightDate!=null? FlightDate:string.Empty),
                                                          "Airports",
                                                         (Airports!=null?Airports:string.Empty)).Result;

            return View(DCS_Get_Search_List);
        }
        public IActionResult M_Passenger(string FlightID)
        {
            string _FlightID = ReturnValueFromString(FlightID, " ");
            HttpContext.Session.SetString("FlightID",_FlightID);
            return View();
        }
        public IActionResult M_PassengerList()
        {
           
            return View();
        }

        [HttpPost]
        public IActionResult M_PassengerList(string ifly_Pax_ID,
                                              string PNR,
                                              string KeySearch,
                                              string PassengerName)
        {

            string uri_DCS_Passenger_Get = UriConfig.uri_B_DCS_Passenger_Get;
            List<DCS_Passenger_Get> DCS_Get_Passenger_List = new DataAPI().GetListOjectAPI<DCS_Passenger_Get>(uri_DCS_Passenger_Get,
                                                          HttpContext.Session.GetString("Token"),
                                                          HttpMethod.Post, false, "Data",
                                                          "FlightID",
                                                          (HttpContext.Session.GetString("FlightID") != null ? 
                                                           HttpContext.Session.GetString("FlightID").ToString() : "124853"),
                                                          "ifly_Pax_ID",
                                                           (ifly_Pax_ID != null ? ifly_Pax_ID : "0"),
                                                           "PNR",
                                                           (PNR != null ? PNR : string.Empty),
                                                           "KeySearch",
                                                           (KeySearch!=null? KeySearch:string.Empty),
                                                           "PassengerName",
                                                           (PassengerName != null ? PassengerName : string.Empty)).Result;

            return View(DCS_Get_Passenger_List);
        }
        public IActionResult M_Comment(string ifly_Pax_ID)
        {
            string _ifly_Pax_ID = ReturnValueFromString(ifly_Pax_ID, " ");
            HttpContext.Session.SetString("ifly_Pax_ID", _ifly_Pax_ID);
            return View();
        }
        [HttpPost]
        public IActionResult M_Comment()
        {
            ViewBag.NewComment = "New";
            return View();
        }
        public string ReturnValueFromString(string _pattern, string _char)
        {
            string result = string.Empty;
            if (_pattern != null)
                for (int i = 0; i < _pattern.Length; i++)
                {
                    if (_pattern[i].ToString() == _char)
                    {
                        result = _pattern.Substring(i + 1, _pattern.Length - (i + 1)).ToString();
                    }
                }
            return result;
        }
    }
}
