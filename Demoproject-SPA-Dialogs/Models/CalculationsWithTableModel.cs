using Demoproject_SPA_Dialogs.Database;
using Demoproject_SPA_Dialogs.Database.Repositories;
using System.Collections.ObjectModel;

namespace Demoproject_SPA_Dialogs.Models
{
    /***
     * Makes the Repository/Service querys before loading the view.
     */
    public class CalculationsWithTableModel
    {
        private DatabaseContext _databaseContext;
        private CalculationService CalculationService;
        private CostrecordService CostrecordService;

        public string NewName { get; set; } = string.Empty;
        public string NewDescription { get; set; } = string.Empty;

        public int currentCalculationId { get; set; } = -1;
        public string currentCalculationName { get; set; } = "";

        public IEnumerable<Calculation> Calculations { get; set; }

        public IEnumerable<Costrecord> CurrentCostrecords { get; set; }

        public Dictionary<string, float> CurrentCostsSumByPeriodName { get; set; }

        public CalculationsWithTableModel(DatabaseContext databaseContext, int currentCalculationId) 
        {
            _databaseContext = databaseContext;
            this.currentCalculationId = currentCalculationId;
            CalculationService = new CalculationService(databaseContext);
            CostrecordService = new CostrecordService(databaseContext);

            Calculations = CalculationService.GetAllCalculations();
            foreach(Calculation calculation in Calculations)
            {
                if(calculation.Id == currentCalculationId)
                {
                    currentCalculationName = calculation.Name;
                    break;
                }
            }
            CurrentCostrecords = CostrecordService.GetCostsForCalculation(currentCalculationId);

            calculateCurrentCostsSums();

        }

        /// <summary>
        /// Sum up the costs for the current calculation in the period of the year and calculate 
        /// the result for each individual period of time (enum PeriodType).
        /// </summary>
        protected void calculateCurrentCostsSums()
        {
            // Calculate the result sum for the costs for a year and divide for the selectable options of period of time
            CurrentCostsSumByPeriodName = new Dictionary<string, float>();
            float sumForYear = 0.0f;
            foreach (Costrecord cost in CurrentCostrecords)
            {
                if (cost.Period == PeriodType.year)
                {
                    sumForYear += cost.Value;
                }
                else if (cost.Period == PeriodType.month)
                {
                    sumForYear += cost.Value * 12;
                }
                else if (cost.Period == PeriodType.week)
                {
                    sumForYear += cost.Value * 52;
                }
                else if (cost.Period == PeriodType.day)
                {
                    sumForYear += cost.Value * 365;
                }
                else if (cost.Period == PeriodType.dayOfFiveWeek)
                {
                    sumForYear += cost.Value * 5 * 52;
                }
                else if (cost.Period == PeriodType.dayOfFourWeek)
                {
                    sumForYear += cost.Value * 4 * 52;
                }
            }
            CurrentCostsSumByPeriodName.Add(PeriodType.year.ToString(), sumForYear);
            CurrentCostsSumByPeriodName.Add(PeriodType.month.ToString(), sumForYear / 12.0f);
            CurrentCostsSumByPeriodName.Add(PeriodType.week.ToString(), sumForYear / 52.0f);
            CurrentCostsSumByPeriodName.Add(PeriodType.day.ToString(), sumForYear / 365.0f);
            CurrentCostsSumByPeriodName.Add(PeriodType.dayOfFiveWeek.ToString(), sumForYear / 52.0f / 5.0f);
            CurrentCostsSumByPeriodName.Add(PeriodType.dayOfFourWeek.ToString(), sumForYear / 52.0f / 4.0f);
        }

    }
}
