using System;

namespace KafkaLib.Messages
{
    public class RegisterFlight
    {
        public DateTime DepartureDate { get; set; }
        public string FlightNumber { get; set; }
    }
}