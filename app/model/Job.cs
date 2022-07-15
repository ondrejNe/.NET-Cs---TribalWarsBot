
namespace TribalWarsBot
{
    /** Basic class for every Job - data for Task */
    public class Job
    {
        public string TaskID { get; set; } = "";
        public TASK_TYPE TaskType { get; set; }
        public int TimeoutTime { get; set; } = 0;
        public bool Run { get; set; } = true;
        /** Task suspension timeout */
        protected void SleepTimetout()
        {
            Task.Delay(TimeoutTime * 1000).Wait();
        }
    }
    public enum TASK_TYPE
    {
        SCAVENGE = 0,
        FARMING = 1
    }
}
