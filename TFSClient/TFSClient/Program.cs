using System;

namespace TFSClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = new ExecuteQuery();


            var result = test.RunGetBugsQueryUsingClientLib().Result;
        }
    }
}
