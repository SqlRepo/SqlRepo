namespace SqlRepo
{
    public class DoNothingSqlLogger : ISqlLogger
    {
        public void Log(string sql)
        {
        }
    }
}