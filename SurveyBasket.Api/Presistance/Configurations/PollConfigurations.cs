using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Api.Presistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Api.Presistance.Configrations
{
    class PollConfigurations : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(100);

            builder.Property(x => x.Summary)
                .HasMaxLength(1500);

            builder.HasIndex(x => x.Title)
                .IsUnique();
        }
    }
}
