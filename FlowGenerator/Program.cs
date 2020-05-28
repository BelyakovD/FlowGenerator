using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlowGenerator
{
    class Program
    {
        static int count;
        static int flowId = 1;

        static string ChangeToDot(double number)
        {
            string num = Convert.ToString(number);
            string result = "";

            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] == ',')
                    result += '.';
                else
                    result += num[i];
            }

            return result;
        }

        static void GetTXT(List<FlowDescription> FlowDescription)
        {
            string filename;
            Console.WriteLine("Введите имя файла:");
            filename = Console.ReadLine();
            StreamWriter streamWriter = new StreamWriter(File.Open(@"" + filename + ".txt", FileMode.CreateNew), Encoding.Unicode);

            streamWriter.WriteLine("{");
            streamWriter.WriteLine("  \"Flows\": [");
            streamWriter.WriteLine("    {");
            streamWriter.WriteLine("      \"Flows\": [");

            for (int i = 0; i < FlowDescription.Count; i++)
            {
                streamWriter.WriteLine("        {");
                streamWriter.WriteLine("          \"Id\": \"" + FlowDescription[i].Id + "\",");
                streamWriter.WriteLine("          \"Destination\": \"" + FlowDescription[i].Destination + "\",");
                streamWriter.WriteLine("          \"Source\": \"" + FlowDescription[i].Source + "\",");
                streamWriter.WriteLine("          \"NonMeasured\": false,");
                streamWriter.WriteLine("          \"Value\": " + ChangeToDot(FlowDescription[i].Value) + ",");
                streamWriter.WriteLine("          \"Tolerance\": " + ChangeToDot(FlowDescription[i].Tolerance) + ",");
                streamWriter.WriteLine("          \"LowerBound\": 0,");
                streamWriter.WriteLine("          \"UpperBound\": 100000");

                if (i < FlowDescription.Count - 1)
                    streamWriter.WriteLine("        },");
                else
                    streamWriter.WriteLine("        }");
            }

            streamWriter.WriteLine("      ],");
            streamWriter.WriteLine("      \"Delta_error\": 0.001");
            streamWriter.WriteLine("    }");
            streamWriter.WriteLine("  ]");
            streamWriter.WriteLine("}");

            streamWriter.Close();
        }

        static FlowDescription AddFlow(double value, int inputnode, int outputnode)
        {
            Random rnd = new Random();
            double tolerance = rnd.NextDouble() * (value / 10);
            double bias = rnd.NextDouble() * tolerance;
            int sign = rnd.Next(0, 2);

            FlowDescription flowDescription = new FlowDescription
            {
                Id = Convert.ToString(flowId)
            };
            if (inputnode != -1)
                flowDescription.Destination = Convert.ToString(inputnode);
            else
                flowDescription.Destination = null;
            if (outputnode != -1)
                flowDescription.Source = Convert.ToString(outputnode);
            else
                flowDescription.Source = null;
            flowDescription.NonMeasured = false;
            if (sign == 1)
            {
                flowDescription.Value = value + bias;
            }
            else
            {
                flowDescription.Value = value - bias;
            }
            flowDescription.Tolerance = tolerance;
            flowDescription.LowerBound = 0;
            flowDescription.UpperBound = 100000;

            flowId++;
            return flowDescription;
        }

        static void Main()
        {
            Console.WriteLine("Введите количество узлов: ");
            count = Convert.ToInt32(Console.ReadLine());

            List<FlowDescription> FlowDescription = new List<FlowDescription>();

            double currentInput = 0, currentOutput = 0;
            Random rnd = new Random();
            double input = rnd.NextDouble() * (999.9 - 10) + 10;
            currentInput += input;

            for (int i = 1; i <= count; i++)
            {
                double newFlow = rnd.NextDouble() * (999.9 - 10) + 10;

                if (newFlow < input)
                {
                    int sign = rnd.Next(-1, 2);
                    if (sign == -1)
                    {
                        currentOutput += newFlow;
                        FlowDescription.Add(AddFlow(newFlow, -1, i));

                    }
                    else if (sign == 1)
                    {
                        currentInput += newFlow;
                        FlowDescription.Add(AddFlow(newFlow, i, -1));
                    }
                }
                else
                {
                    int sign = rnd.Next(0, 2);
                    if (sign == 1)
                    {
                        currentInput += newFlow;
                        FlowDescription.Add(AddFlow(newFlow, i, -1));
                    }
                }

                if (i > 1)
                    FlowDescription.Add(AddFlow(input, i, i - 1));
                else
                    FlowDescription.Add(AddFlow(input, i, -1));

                currentInput -= currentOutput;
                currentOutput = 0;
                input = currentInput;
            }
            FlowDescription.Add(AddFlow(input, -1, count));

            Console.WriteLine("Всего потоков: " + (flowId - 1));
            GetTXT(FlowDescription);
        }
    }
}
