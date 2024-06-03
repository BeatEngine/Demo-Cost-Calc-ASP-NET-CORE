using Demoproject_SPA_Dialogs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
public class Costrecord
{
    public class CalculationConfiguration : IEntityTypeConfiguration<Calculation>
    {
        public void Configure(EntityTypeBuilder<Calculation> builder)
        {
            // Primary Key
            builder.HasKey(u => u.Id);

            // Foreign Key Relation
            //builder.HasOne(ca => ca.Costrecords).WithOne().OnDelete(DeleteBehavior.NoAction);
        }
    }
    public long Id { get; set; }    
    public long CalcId { get; set; }

    public string? Name { get; set; }
    public PeriodType Period { get; set; }
    public float Value { get; set; }

}
