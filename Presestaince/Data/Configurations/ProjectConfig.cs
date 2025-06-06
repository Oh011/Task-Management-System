using Domain.Entities.ProjectModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {

            builder.Property(P => P.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(P => P.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");


            builder.Property(P => P.Status).HasConversion(Status => Status.ToString(),

                S => Enum.Parse<ProjectStatus>(S));



            builder.HasMany(p => p.ProjectTasks).WithOne(TP => TP.Project).HasForeignKey(TP => TP.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(P => P.CreatedByUser).WithMany(U => U.CreatedProjects).HasForeignKey(P => P.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasMany(P => P.ProjectUsers).WithOne(PU => PU.Project).HasForeignKey(PU => PU.ProjectId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
