
namespace TribalWarsBot
{
    public class Job
    {
        public string TaskID { get; set; } = "";
        public TASK_TYPE TaskType { get; set; }
        public int TimeoutTime { get; set; } = 0;
        public bool Run { get; set; } = true;
        protected void SleepTimetout()
        {
            Thread.Sleep(TimeoutTime*1000);
        }
    }
    public enum TASK_TYPE
    {
        SCAVENGE = 0,
        FARMING = 1
    }
}
