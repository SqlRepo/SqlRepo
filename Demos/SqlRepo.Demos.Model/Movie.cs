using System;
using System.Collections.Generic;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Movie : Entity<int>
    {
        public Movie()
        {
            this.Cast = new List<Cast>();
            this.Ratings = new List<Rating>();
            this.Reviews = new List<Review>();
        }

        public IList<Cast> Cast { get; set; }
        public int ClassificationId { get; set; }
        public Classification Classification { get; set; }
        public Director Director { get; set; }
        public int DirectorId { get; set; }
        public IList<Rating> Ratings { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IList<Review> Reviews { get; set; }
        public string Synopsis { get; set; }
        public string Title { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public int StudioId { get; set; }
        public Studio Studio { get; set; }
        public int RunTimeMinutes { get; set; }
        public double BudgetDollars { get; set; }
        public double BoxOfficeDollars { get; set; }
        public int OscarNominations { get; set; }
        public int OscarWins { get; set; }
    }
}