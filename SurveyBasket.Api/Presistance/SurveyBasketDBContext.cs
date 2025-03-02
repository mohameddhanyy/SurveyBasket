using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Presistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Api.Presistance
{
    public class SurveyBasketDBContext(DbContextOptions<SurveyBasketDBContext> options):DbContext(options)
    {
        public DbSet<Poll> Polls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
