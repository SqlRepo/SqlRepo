﻿using System;
using SqlRepo.Model;

namespace GettingStartedStatic
{
    public class ToDo : Entity<int>
    {
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Task { get; set; }
    }
}