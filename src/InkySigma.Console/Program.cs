using System;
using System.Diagnostics;
namespace InkySigma.Console
{
    public class Program
    {
        public void Main(string[] args)
        {
            var webHost = Process.Start("dnx", "web");
        }
    }
}