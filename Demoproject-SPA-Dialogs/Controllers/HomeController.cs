using Demoproject_SPA_Dialogs.Controllers.REST;
using Demoproject_SPA_Dialogs.Database;
using Demoproject_SPA_Dialogs.Database.Repositories;
using Demoproject_SPA_Dialogs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Serilog;
using System.Globalization;

namespace Demoproject_SPA_Dialogs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly Serilog.Core.Logger logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .WriteTo.Debug()
                            .MinimumLevel.Information()
                            .CreateLogger();


        private readonly DatabaseContext _context;
        private readonly CalculationService CalculationService;
        private readonly CostrecordService CostrecordService;

        private static readonly Dictionary<int, int> CURRENT_CALCULATION_ID_BY_SESSION= new Dictionary<int, int>();

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
            CalculationService = new CalculationService(_context);
            CostrecordService = new CostrecordService(_context);
        }

        public IActionResult Index()
        {
            return View(new CalculationsWithTableModel(_context, getCurrentCalculationIdByUserSession(0)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCalculation(String calcName, String calcDescription)
        {
            Calculation calc = new Calculation();
            calc.Name = calcName;
            calc.Description = calcDescription;
            await CalculationService.AddCalculation(calc);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="session"></param>
        /// <returns>The value or -1 if not found</returns>
        public static int getCurrentCalculationIdByUserSession(int session)
        {
            int value;
            if(CURRENT_CALCULATION_ID_BY_SESSION.TryGetValue(session, out value))
            {
                return value;
            }
            return -1;
        }

        /// <summary>
        /// ...
        /// </summary>
        /// <param name="session"> user related session id</param>
        /// <param name="costId"> Id of the current calculation</param>
        /// <returns>successfully set or add</returns>
        public static bool setCurrentCalculationIdByUserSession(int session, int costId)
        {
            int value;
            if (CURRENT_CALCULATION_ID_BY_SESSION.TryGetValue(session, out value))
            {
                // Replace
                CURRENT_CALCULATION_ID_BY_SESSION[session] = costId;
                return true;
            }
            else
            {
                // Add
                CURRENT_CALCULATION_ID_BY_SESSION.Add(session, costId);
                return true;
            }
        }

        public IActionResult OpenCalculation(int calcId)
        {
            //session is always 0, because there are no different users.
            setCurrentCalculationIdByUserSession(0, calcId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCalculation(int calcId)
        {
            await CalculationService.RemoveCalculationById(calcId);
            return RedirectToAction("Index");
        }

        public IActionResult Calculations()
        {
            IEnumerable<Calculation> calculations = CalculationService.GetAllCalculations();
            return View(calculations);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> CreateNewCostrecord()
        {
            Costrecord costrecord = new Costrecord();
            costrecord.Name = "New cost";
            costrecord.Period = PeriodType.month;
            costrecord.Value = 0.0f;
            // For this demo the session is always 0
            costrecord.CalcId = CURRENT_CALCULATION_ID_BY_SESSION[0];
            await CostrecordService.AddCostrecord(costrecord);
            return RedirectToAction("Index");
        }

        protected EasyEventRequest? parseEasyEventRequest(Stream requestbody)
        {
            //requestbody.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(requestbody).ReadToEndAsync().Result;
            logger.Debug(json);
            JsonDocument obj = JsonDocument.Parse(json);
            return new EasyEventRequest(obj);
        }

        [HttpPost]
        public string OnCostNameChanged(int calcId)
        {
            try
            {
                EasyEventRequest? easyEventRequest = parseEasyEventRequest(Request.Body);
                if (easyEventRequest != null)
                {
                    int costId = int.Parse(easyEventRequest.values["costId"]);
                    string costName = easyEventRequest.values["newValue"];

                    Task task = CostrecordService.UpdateCostrecordNameById(costId, costName);
                    task.Wait();
                    // If update failed throw exception
                    Exception? pex = task.Exception;
                    if (pex != null)
                    {
                        throw pex;
                    }

                    return new EasyEventResponse(true, "").ToString();
                }
                else
                {
                    throw new Exception("OnRemoveCostrecord: Internal error while parsing.");
                }
            }
            catch (Exception ex)
            {
                return new EasyEventResponse(false, ex.Message).ToString();
            }
        }

        [HttpPost]
        public string OnCostPeriodChanged(int calcId)
        {

            try
            {
                EasyEventRequest? easyEventRequest = parseEasyEventRequest(Request.Body);
                if (easyEventRequest != null)
                {
                    int costId = int.Parse(easyEventRequest.values["costId"]);

                    string periodName = easyEventRequest.values["newValue"];
                    PeriodType costPeriod;
                    
                    if (!PeriodType.TryParse(periodName, out costPeriod))
                    {
                        throw new Exception("Found no PeriodType for \"" + periodName + "\"");
                    }

                    Task task = CostrecordService.UpdateCostrecordPeriodById(costId, costPeriod);
                    task.Wait();
                    // If update failed throw exception
                    Exception? pex = task.Exception;
                    if (pex != null)
                    {
                        throw pex;
                    }

                    return new EasyEventResponse(true, "").ToString();
                }
                else
                {
                    throw new Exception("OnRemoveCostrecord: Internal error while parsing.");
                }
            }
            catch (Exception ex)
            {
                return new EasyEventResponse(false, ex.Message).ToString();
            }
        }

        [HttpPost]
        public string OnCostValueChanged(int calcId)
        {

            try
            {
                EasyEventRequest? easyEventRequest = parseEasyEventRequest(Request.Body);
                if (easyEventRequest != null)
                {
                    int costId = int.Parse(easyEventRequest.values["costId"]);
                    string value = easyEventRequest.values["newValue"];
                    value = value.Replace(",", "."); // German comma definition for decimal numbers convertation
                    float costValue;
                    if(!float.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out costValue))
                    {
                        throw new Exception("Failed to parse value: \"" + value + "\"");
                    }

                    Task task = CostrecordService.UpdateCostrecordValueById(costId, costValue);
                    task.Wait();
                    // If update failed throw exception
                    Exception? pex = task.Exception;
                    if (pex != null)
                    {
                        throw pex;
                    }

                    return new EasyEventResponse(true, "").ToString();
                }
                else
                {
                    throw new Exception("OnRemoveCostrecord: Internal error while parsing.");
                }
            }
            catch (Exception ex)
            {
                return new EasyEventResponse(false, ex.Message).ToString();
            }
        }

        [HttpPost]
        public string OnRemoveCostrecord()
        {
            try
            {
                EasyEventRequest? easyEventRequest = parseEasyEventRequest(Request.Body);
                if (easyEventRequest != null)
                {
                    int costId = int.Parse(easyEventRequest.values["costId"]);

                    Task task = CostrecordService.RemoveCostrecordById(costId);
                    task.Wait();
                    // If remove failed throw exception
                    Exception? pex = task.Exception;
                    if(pex != null)
                    {
                        throw pex;
                    }

                    return new EasyEventResponse(true, "").ToString();
                }
                else
                {
                    throw new Exception("OnRemoveCostrecord: Internal error while parsing.");
                }
            } catch (Exception ex)
            {
                return new EasyEventResponse(false, ex.Message).ToString();
            }
        }
        

        public string OnCostNameChanged()
        {
            //TODO implement db-logic
            return new EasyEventResponse(true, "").ToString();
        }

        public string OnCostPeriodChanged()
        {
            return new EasyEventResponse(true, "").ToString();
        }

        public string OnCostValueChanged()
        {
            return new EasyEventResponse(true, "").ToString();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
