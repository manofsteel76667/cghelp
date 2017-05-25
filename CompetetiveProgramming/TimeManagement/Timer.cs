using System;

namespace CompetetiveProgramming.TimeManagement {
    /**
 * @author Manwe
 *
 * Time management class in order to measure elapsed time and avoid time outs
 * The Timer class uses the System.nanoTime() method to get the time. It is not executed in a separated thread, you must use the timeCheck method in order to verify the timeout has not been reached during the execution of your computation
 */
public class Timer {
    private long startTime = 0;
    private long timeout=0;

    /**
     * @return
     *  the number of ticks between last time the timer has been started and now
     *  Will return DateTime.UtcNow.Ticks if the timer has never been started
     */
    public long currentTimeTakenInTicks() {
        return ((DateTime.UtcNow.Ticks - startTime));
    }

    /**
     * Start the timer. 
     * If the timer is already started, will simply define the timeout as now + duration
     * A call to this method is mandatory if you want the timeCheck method to throws timeout exceptions
     * 
     * @param durationInMilliseconds
     * 		The duration in milliseconds from now until which the timeCheck method will throws Timeoutexceptions
     */
    public void startTimer(double durationInMilliseconds) {
        startTime = DateTime.UtcNow.Ticks;
        timeout = startTime+(long)(durationInMilliseconds*10000);
    }

    /**
     * Verify if the timeout has been reached. If yes, throws a TimeoutException
     * will not throw anything if the timer has never been started.
     * @throws TimeoutException
     */
    public void timeCheck()  {
        if (startTime > 0 && DateTime.UtcNow.Ticks > timeout) {
            throw new TimeoutException();
        }
    }
}
}
