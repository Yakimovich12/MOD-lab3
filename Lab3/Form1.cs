using System;
using System.Globalization;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IFormatProvider formatProvider = new NumberFormatInfo { NumberDecimalSeparator = "." };

                double pi1 = double.Parse(textBox3.Text, formatProvider);

                if (pi1 < 0 || pi1 > 1)
                {
                    throw new ArgumentOutOfRangeException("π1 cannot be less than 0 or greater then 1");
                }

                double pi2 = Convert.ToDouble(textBox4.Text, formatProvider);

                if (pi1 < 0 || pi1 > 1)
                {
                    throw new ArgumentOutOfRangeException("π2 cannot be less than 0 or greater then 1");
                }

                double[][] matrix = QueuingSystem.QueuingSystem.GenerateMatrix(pi1, pi2);

                double[] resultProbabilities = Matrix.Matrix.GaussianMethod(matrix);

                double pPF = QueuingSystem.QueuingSystem.GetProbabilityOfFailure(resultProbabilities, pi1, pi2);

                double aThroughtput = QueuingSystem.QueuingSystem.GetAbsolutlyThroughtput(resultProbabilities, pi1, pi2);

                double aveNumOfApInQueue = QueuingSystem.QueuingSystem.GetAverageQueueLength(resultProbabilities, pi1, pi2);

                double aveNumOfAp = QueuingSystem.QueuingSystem.GetAverageNumberOfApplications(resultProbabilities, pi1, pi2);

                double pipe1BusyFactor = QueuingSystem.QueuingSystem.GetPipe1BusyProbability(resultProbabilities, pi1, pi2);
                double pipe2BusyFactor = QueuingSystem.QueuingSystem.GetPipe2BusyProbability(resultProbabilities, pi1, pi2);
                double pL = QueuingSystem.QueuingSystem.GetProbabilityOfLocking(resultProbabilities, pi1, pi2);

                textBox1.Text = $"Относительная пропускная способность: {1 - pPF}\r\n";
                textBox1.Text += $"Абсолютнаяя пропускная способность: {aThroughtput}\r\n";
                textBox1.Text += $"Вероятность отказа: {pPF}\r\n";
                textBox1.Text += $"Вероятность блокировки канала: {pL}\r\n";
                textBox1.Text += $"Средняя длина очереди заявок: {aveNumOfApInQueue}\r\n";
                textBox1.Text += $"Средняя количество заявок, обрабатываемых системой: {aveNumOfAp}\r\n";
                textBox1.Text += $"Среднее время пребывания заявки в очереди: {aveNumOfApInQueue / aThroughtput}\r\n";
                textBox1.Text += $"Среднее время пребывания заявки в системе: {aveNumOfAp / aThroughtput}\r\n";
                textBox1.Text += $"Коэффициент нагрузки первого канала: {pipe1BusyFactor}\r\n";
                textBox1.Text += $"Коэффициент нагрузки второго канала: {pipe2BusyFactor}\r\n";
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                IFormatProvider formatProvider = new NumberFormatInfo { NumberDecimalSeparator = "." };

                double pi1 = double.Parse(textBox5.Text, formatProvider);

                if (pi1 < 0 || pi1 > 1)
                {
                    throw new ArgumentOutOfRangeException("π1 cannot be less than 0 or greater then 1");
                }

                double pi2 = Convert.ToDouble(textBox6.Text, formatProvider);

                if (pi1 < 0 || pi1 > 1)
                {
                    throw new ArgumentOutOfRangeException("π2 cannot be less than 0 or greater then 1");
                }

                int number = 1000000;

                var parameters = QueuingSystem.QueuingSystem.EmulateQueuingSystem(number, pi1, pi2);

                textBox2.Text = $"Относительная пропускная способность: {parameters[0]}\r\n";
                textBox2.Text += $"Абсолютнаяя пропускная способность: {parameters[1]}\r\n";
                textBox2.Text += $"Вероятность отказа: {parameters[2]}\r\n";
                textBox2.Text += $"Вероятность блокировки канала: {parameters[3]}\r\n";
                textBox2.Text += $"Средняя длина очереди заявок: {parameters[4]}\r\n";
                textBox2.Text += $"Средняя количество заявок, обрабатываемых системой: {parameters[5]}\r\n";
                textBox2.Text += $"Среднее время пребывания заявки в очереди: {parameters[6]}\r\n";
                textBox2.Text += $"Среднее время пребывания заявки в системе: {parameters[7]}\r\n";
                textBox2.Text += $"Коэффициент нагрузки первого канала: {parameters[8]}\r\n";
                textBox2.Text += $"Коэффициент нагрузки второго канала: {parameters[9]}\r\n";
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error: " + exc.Message);
            }
        }
    }
}
