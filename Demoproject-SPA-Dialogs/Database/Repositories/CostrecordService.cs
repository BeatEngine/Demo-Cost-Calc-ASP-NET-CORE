using Microsoft.EntityFrameworkCore;

namespace Demoproject_SPA_Dialogs.Database.Repositories
{
    public class CostrecordService
    {
        public DatabaseContext dbCtx;

        public CostrecordService(DatabaseContext dbContext)
        {
            dbCtx = dbContext;
        }

        public async Task AddCostrecord(Costrecord costrecord)
        {
            // DB   INSERT or UPDATE
            await dbCtx.Costrecords.AddAsync(costrecord);
            await dbCtx.SaveChangesAsync();
        }

        public async Task UpdateCostrecord(Costrecord costrecord)
        {
            // DB   UPDATE
            await dbCtx.Costrecords.Where(co => co.Id == costrecord.Id).ExecuteUpdateAsync(
                setters => setters.
                SetProperty(o => o.Period, costrecord.Period).
                SetProperty(o => o.Name, costrecord.Name).
                SetProperty(o => o.Value, costrecord.Value));
        }

        public async Task UpdateCostrecordNameById(long costrecordId, string costrecordName)
        {
            // DB   UPDATE
             await dbCtx.Costrecords.Where(co => co.Id == costrecordId).ExecuteUpdateAsync(
                setters => setters.
                SetProperty(o => o.Name, costrecordName));
        }

        public async Task UpdateCostrecordPeriodById(long costrecordId,PeriodType costrecordPeriod)
        {
            // DB   UPDATE
            await dbCtx.Costrecords.Where(co => co.Id == costrecordId).ExecuteUpdateAsync(
                setters => setters.
                SetProperty(o => o.Period, costrecordPeriod));
        }

        public async Task UpdateCostrecordValueById(long costrecordId, float costrecordValue)
        {
            // DB   UPDATE
            await dbCtx.Costrecords.Where(co => co.Id == costrecordId).ExecuteUpdateAsync(
                setters => setters.
                SetProperty(o => o.Value, costrecordValue));
        }

        public async Task RemoveCostrecordById(long costrecordId)
        {
            await dbCtx.Costrecords.Where(co => co.Id == costrecordId).ExecuteDeleteAsync();
        }

        public IEnumerable<Costrecord> GetCostsForCalculation(long calculationId)
        {
            return dbCtx.Costrecords.FromSql($"SELECT * FROM demo1.dbo.Costrecord where CalcId = {calculationId}").ToList();
        }

        public IEnumerable<Costrecord> GetCostsById(long costrecordId)
        {
            return dbCtx.Costrecords.FromSql($"SELECT * FROM demo1.dbo.Costrecord where Id = {costrecordId}").ToList();
        }
    }
}
