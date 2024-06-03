using Microsoft.EntityFrameworkCore;

namespace Demoproject_SPA_Dialogs.Database.Repositories
{
    public class CalculationService
    {
        public DatabaseContext dbCtx;

        public CalculationService(DatabaseContext dbContext)
        {
            dbCtx = dbContext;
        }

        public async Task AddCalculation(Calculation calculation)
        {
            // DB   INSERT or UPDATE
            await dbCtx.Calculations.AddAsync(calculation);
            await dbCtx.SaveChangesAsync();
        }

        public async Task UpdateCalculation(Calculation calculation)
        {
            //      UPDATE
            await dbCtx.Calculations.Where(co => co.Id == calculation.Id).ExecuteUpdateAsync(
                setters => setters.
                SetProperty(o => o.Creation, calculation.Creation).
                SetProperty(o => o.Name, calculation.Name).
                SetProperty(o => o.Description, calculation.Description));
        }

        public async Task RemoveCalculationById(long calculationId)
        {
            await dbCtx.Calculations.Where(co => co.Id == calculationId).ExecuteDeleteAsync();
        }

        public IEnumerable<Calculation> GetCalculationsById(long calculationId)
        {
            return dbCtx.Calculations.FromSql($"SELECT * FROM demo1.dbo.Calculation where Id = {calculationId}").ToList();
        }

        public IEnumerable<Calculation> GetCalculationsPaged(long offset, long count)
        {
            return dbCtx.Calculations.FromSql($"SELECT * FROM demo1.dbo.Calculation order by Creation DESC OFFSET {offset} ROWS FETCH NEXT {count} ROWS ONLY").ToList();
        }

        public IEnumerable<Calculation> GetAllCalculations()
        {
            return dbCtx.Calculations.FromSql($"SELECT * FROM demo1.dbo.Calculation order by Creation DESC").ToList();
        }

    }
}
