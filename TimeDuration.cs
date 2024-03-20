namespace TimeStatistic
{
    public class TimeDuration
    {

        private long initialSeconds = 0;
        public int Seconds;
        public int Minutes;
        public int Hours;
        public int Days;
        public int Weeks;

        public TimeDuration(long seconds)
        {
            initialSeconds = seconds;
            Seconds = (int)(seconds % 60);
            Minutes = (int)(seconds / 60);
            Hours = Minutes / 60;
            Days = Hours / 24;
            Weeks = Days / 7;
        }

        public TimeDuration AddSeconds(long seconds)
        {
            initialSeconds += seconds;
            Seconds = (int)(seconds % 60);
            Minutes = (int)(seconds / 60);
            Hours = Minutes / 60;
            Days = Hours / 24;
            Weeks = Days / 7;

            return this;
        }

        public override string ToString()
        {
            return $"{Weeks} недель, {Days} дней, {Hours} часов, {Minutes} минут, {Seconds} секунд.";
        }

        public int ToInt32()
        {
            return (int)initialSeconds;
        }

    }

    public struct PlayerTime
    {
        public TimeDuration Time;
        public string Username;

        public PlayerTime(TimeDuration time, string nick)
        {
            Time = time;
            Username = nick;
        }
    }
}
