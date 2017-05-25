using System;

namespace CompetetiveProgramming {
    public class Log {
        static Log() {
            Time = DateTime.UtcNow.Ticks;
        }
        static long Time;
        public static bool debug = true;
        public static void WriteLine(string format, params object[] values) {
            if (debug)
                Console.Error.WriteLine(string.Format(format, values));
        }
        public static void Write(string format, params object[] values) {
            if (debug)
                Console.Error.Write(string.Format(format, values));
        }
        public static void TimeStamp() {
            long t = DateTime.UtcNow.Ticks;
            WriteLine("{0} ms", (t - Time) * .0001);
            Time = t;
        }
    }
}
