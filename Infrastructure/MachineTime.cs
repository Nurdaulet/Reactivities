using System;
using Application.Interfaces;

namespace Infrastructure
{
    public class MachineTime : IDateTime
    {
        public int CurrentYear => DateTime.Now.Year;

        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
