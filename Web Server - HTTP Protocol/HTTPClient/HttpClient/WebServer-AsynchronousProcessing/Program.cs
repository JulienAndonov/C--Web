using System;
using System.Threading;

namespace WebServer_AsynchronousProcessing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Object myLock1 = new Object();
            Object myLock2 = new Object();

            Thread thread1 = new Thread(() =>
            {
                Thread.Sleep(1000);
                lock (myLock1)
                {
                    Thread.Sleep(1000);
                    lock (myLock2)
                    {

                    }
                }
            });

            Thread thread2 = new Thread(() =>
            {
                lock (myLock2)
                {
                    Thread.Sleep(1000);
                    lock (myLock1)
                    {
                        Thread.Sleep(1000);
                    }
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

        }

    }
}
