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
    class QuestionConfigurations : IEntityTypeConfiguration<Question>
    {

        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(x => new { x.Content, x.PollId }).IsUnique();

            builder.Property(x => x.Content).HasMaxLength(1000);

        }
    }
}
