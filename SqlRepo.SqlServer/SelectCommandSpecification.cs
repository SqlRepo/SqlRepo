using System.Text;
            
            const string ClauseTemplate = "SELECT {0}";

            string top = Top.HasValue ? $"TOP ({Top}) " : string.Empty;

            return string.Format(ClauseTemplate, string.IsNullOrEmpty(selections) ? "*" : selections);
