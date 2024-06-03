
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class Calculation
{

    /***
     * EntityFrameworkCore Database-Table-Relation mapping
     */
    public class CalculationConfiguration : IEntityTypeConfiguration<Calculation>
    {
        public void Configure(EntityTypeBuilder<Calculation> builder)
        {
            // Primary Key
            builder.HasKey(ca => ca.Id);

            // Foreign Key of Related Table and Delete-Cascade (all child records)
            builder.HasMany(ca => ca.Costrecords)
                .WithOne()
                .HasForeignKey(co => co.CalcId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    // Is Increment(1,1)
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // Has a default GETDATE()
    public DateTime? Creation { get; set; } = null;

    public virtual List<Costrecord> Costrecords { get; private set; }
}
