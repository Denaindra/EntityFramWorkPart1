using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EFDataAccessLayer.DataAccess;
using EFDataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFramWorkCore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly PeopleContext context;

        public IndexModel(ILogger<IndexModel> logger, PeopleContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void OnGet()
        {
            LoadDumyData();

            var people = context.People
               .Include(a => a.Addresses)
               .Include(e => e.EmailAddresses)
               .Where(x => x.Age >= 18 && x.Age <= 65)
               .ToList();
              // .Where(x => ApprovedAge(x.Age));
        }

        private bool ApprovedAge(int age)
        {
            return (age >= 18 && age <= 65);
        }
        public void LoadDumyData()
        {
            if (!context.People.Any())
            {
                string file = System.IO.File.ReadAllText("sampledata.json");
                var people = JsonSerializer.Deserialize<List<Person>>(file);
                context.AddRange(people);
                context.SaveChanges();
            }
        }
    }
}
