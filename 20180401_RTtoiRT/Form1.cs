using System;
using System.Windows.Forms;

namespace _20180401_RTtoiRT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Load counters, string arrays and define all variables
        private int counter = 0;
        private string destinationFile;
        private string lineWithGU;
        private string line;
        private string[] lineWithRT;
        private double RT;
        private double GU;
        private string GUstring;
        private string numberLines;
        private double firstPartExpEqnDbl;
        private double secondPartExpEqnDbl;
        private char stringSeparators = '"';
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Hide();
            label4.Show();
            // when upload button is clicked, open the text file as a stream
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                    System.IO.StreamReader(openFileDialog1.FileName);
                destinationFile = openFileDialog1.FileName + "GU.mzML";
                // go through line by line
                // Getting the writer ready to add lines from both if and else to output file
                System.IO.StreamWriter writer = new System.IO.StreamWriter(destinationFile);
                    while ((line = sr.ReadLine()) != null)
                    {
                    // find lines containing the scan start time phrase
                    if (line.Contains("scan start time"))
                    {
                        // split the line containing this by "
                        lineWithRT = line.Split(stringSeparators);
                        // After the 7th ", that's where the RT is in seconds, so convert it from string array into double for maths operations
                        RT = double.Parse(lineWithRT[7]);
                        // Print the RT just to be sure
                        System.Console.WriteLine(RT);
                        // From the filled text boxes, start transforming the rt to gu value based on equation parameters
                        firstPartExpEqnDbl = double.Parse(firstPartExpEqn.Text);
                        secondPartExpEqnDbl = double.Parse(secondPartExpEqn.Text);
                        RT = RT - secondPartExpEqnDbl;
                        RT = RT / firstPartExpEqnDbl;
                        // Perform the exponential function, finishing completion to GU
                        GU = Math.Exp(RT);
                        // Print GU to be sure.                       System.Console.WriteLine(GU);
                        GUstring = GU.ToString("R");
                        // Put the GU value back into the line array
                        lineWithRT[7] = GUstring;
                        counter++;
                        // Turn the array with GU back into a normal line and print to check
                        lineWithGU = string.Join("\"", lineWithRT);
                        // Write the line with GU instead of RT to a new text file
                        System.Console.WriteLine(lineWithGU);
                        {
                            writer.WriteLine(lineWithGU);
                        }
                    }
                    else
                        // Write the normal line into a new text file with no modification
                        writer.WriteLine(line);
                }
                System.Console.WriteLine("There were {0} lines containing scan start time.", counter);
                numberLines = counter.ToString();
                label5.Show();
                lines.Text = numberLines;
                // Required to save the text file
                writer.Close();
                lines.Show();
                button2.Show();
                label4.Hide();
            }
        }
    }
}
