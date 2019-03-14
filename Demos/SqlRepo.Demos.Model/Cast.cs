using System;
using SqlRepo.Model;

namespace SqlRepo.Demos.Model
{
    public class Cast : Entity<int>
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public string CharacterName { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}