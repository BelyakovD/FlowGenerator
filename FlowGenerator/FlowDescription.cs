using System;
using System.Collections.Generic;
using System.Text;

namespace FlowGenerator
{
    class FlowDescription
    {
        private String id;
        private String destination;
        private String source;
        private bool nonMeasured;
        private double value;
        private double tolerance;
        private double lowerBound;
        private double upperBound;

        public String Id { get => id; set => id = value; }
        public String Destination { get => destination; set => destination = value; }
        public String Source { get => source; set => source = value; }
        public bool NonMeasured { get => nonMeasured; set => nonMeasured = value; }
        public double Value { get => value; set => this.value = value; }
        public double Tolerance { get => tolerance; set => tolerance = value; }
        public double LowerBound { get => lowerBound; set => lowerBound = value; }
        public double UpperBound { get => upperBound; set => upperBound = value; }
    }
}
