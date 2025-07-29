using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GA
{
    public partial class Form1 : Form
    {
        private static Random rnd = new Random();
        public int PopülasyonBoyutu;
        double ÇaprazlamaOranı;
        double MutasyonOranı;
        int JenarasyonSayısı;

        public Form1()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PopülasyonBoyutu = int.Parse(textBox1.Text); 
            if (double.TryParse(textBox2.Text, out ÇaprazlamaOranı))
            {
                if (ÇaprazlamaOranı < 0 || ÇaprazlamaOranı > 1)
                {
                    MessageBox.Show("Çaprazlama oranı 0 ile 1 arasında olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir sayı giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (double.TryParse(textBox3.Text, out MutasyonOranı))
            {
                if (MutasyonOranı < 0 || MutasyonOranı > 1)
                {
                    MessageBox.Show("Mutasyon oranı 0 ile 1 arasında olmalıdır!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir sayı giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }
            JenarasyonSayısı = int.Parse(textBox5.Text);

            List<string> popülasyon = PopülasyonOluştur(PopülasyonBoyutu);
            Kromozom kromozom = new Kromozom();
            kromozom.KromozomUzunluğu(kromozom.max_x, kromozom.min_x, kromozom.max_y, kromozom.min_y);

            double enIyiFitness = double.MinValue;
            string enIyiKromozom = "";

            chart1.Series.Clear();
            var series = new Series("En İyi Fitness");
            series.ChartType = SeriesChartType.Point;
            chart1.Series.Add(series);

            for (int j = 0; j < JenarasyonSayısı; j++)
            {
                popülasyon = TurnuvaSecim(popülasyon, PopülasyonBoyutu, kromozom);

                popülasyon = Çaprazlama(popülasyon, kromozom);

                popülasyon = Mutasyon(popülasyon, kromozom);

                foreach (var kromozomBit in popülasyon)
                {
                    string bitsX = kromozomBit.Substring(0, kromozom.GetNumberOfBitsX());
                    string bitsY = kromozomBit.Substring(kromozom.GetNumberOfBitsX(), kromozom.GetNumberOfBitsY());

                    int decimal_value_x = Convert.ToInt32(bitsX, 2);
                    int decimal_value_y = Convert.ToInt32(bitsY, 2);

                    var (x, y) = DeğişkenDeğeri(kromozom.GetNumberOfBitsX(), kromozom.GetNumberOfBitsY(), kromozom.max_x, kromozom.min_x, kromozom.max_y, kromozom.min_y, decimal_value_x, decimal_value_y);

                    double fitness = Fitness(x, y);

                    if (fitness > enIyiFitness)
                    {
                        enIyiFitness = fitness;
                        enIyiKromozom = kromozomBit;
                    }
                }

                label6.Text = $"En iyi çözüm: {enIyiKromozom} - Fitness: {enIyiFitness}";

                chart1.Series["En İyi Fitness"].Points.AddXY(j + 1, enIyiFitness);
                if (j == JenarasyonSayısı - 1) 
                {
                    listBox1.Items.Clear(); 
                    foreach (var kromozomBit in popülasyon)
                    {
                        listBox1.Items.Add(kromozomBit);
                    }
                }
            }
        }

        public static double Fitness(double DeğişkenDeğeriX, double DeğişkenDeğeriY)
        {
            return 100 * Math.Sqrt(Math.Abs(DeğişkenDeğeriY - 0.01 * DeğişkenDeğeriX * DeğişkenDeğeriX)) + 0.01 * Math.Abs(DeğişkenDeğeriX + 10);
        }

        public class Kromozom
        {
            public double max_x = -5;
            public double min_x = -15;
            public double max_y = 3;
            public double min_y = -3;
            public int numberOfbitsx;
            public int numberOfbitsy;
            public int numberOfbits;

            public void KromozomUzunluğu(double max_x, double min_x, double max_y, double min_y)
            {
                numberOfbitsx = (int)Math.Floor(Math.Log2((max_x - min_x) * 1000)) + 1;
                numberOfbitsy = (int)Math.Floor(Math.Log2((max_y - min_y) * 1000)) + 1;
                numberOfbits = numberOfbitsx + numberOfbitsy;
            }

            public string KromozomÜret()
            {
                string bitsX = RastgeleBit(numberOfbitsx);
                string bitsY = RastgeleBit(numberOfbitsy);
                return bitsX + bitsY;
            }
            
            private string RastgeleBit(int numBits)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < numBits; i++)
                {
                    sb.Append(Form1.rnd.Next(0, 2)); 
                }
                return sb.ToString();
            }

            public int GetNumberOfBitsX() => numberOfbitsx;
            public int GetNumberOfBitsY() => numberOfbitsy;
        }

        public List<string> PopülasyonOluştur(int PopülasyonBoyutu)
        {
            Kromozom kromozom = new Kromozom();
            kromozom.KromozomUzunluğu(kromozom.max_x, kromozom.min_x, kromozom.max_y, kromozom.min_y); 

            List<string> popülasyon = new List<string>(); 

            for (int i = 0; i < PopülasyonBoyutu; i++)
            {
                string kromozomBit = kromozom.KromozomÜret();
                popülasyon.Add(kromozomBit); 
            }

            return popülasyon;
        }

        public void DecimalValue(string kromozomBit, int numberOfbitsx, int numberOfbitsy)
        {
            string bitsX = kromozomBit.Substring(0, numberOfbitsx); 
            string bitsY = kromozomBit.Substring(numberOfbitsx, numberOfbitsy); 

            int decimal_value_x = Convert.ToInt32(bitsX, 2); 
            int decimal_value_y = Convert.ToInt32(bitsY, 2); 
        }

        public static (double, double) DeğişkenDeğeri(int numberOfbitsx, int numberOfbitsy, double max_x, double min_x, double max_y, double min_y, int decimal_value_x, int decimal_value_y)
        {
            double DeğişkenDeğeriX = min_x + (decimal_value_x / (Math.Pow(2, numberOfbitsx) - 1)) * (max_x - min_x);
            double DeğişkenDeğeriY = min_y + (decimal_value_y / (Math.Pow(2, numberOfbitsy) - 1)) * (max_y - min_y);

            return (DeğişkenDeğeriX, DeğişkenDeğeriY);
        }

        public static List<string> TurnuvaSecim(List<string> popülasyon, int PopülasyonBoyutu, Kromozom kromozom)
        {
            List<string> yeniPopulasyon = new List<string>();

            for (int j = 0; j < PopülasyonBoyutu; j++)
            {
                int a = Form1.rnd.Next(0, popülasyon.Count);
                int b = Form1.rnd.Next(0, popülasyon.Count);

                int numberOfbitsx = kromozom.GetNumberOfBitsX();
                int numberOfbitsy = kromozom.GetNumberOfBitsY();

                int decimal_value_x_a = Convert.ToInt32(popülasyon[a].Substring(0, numberOfbitsx), 2);
                int decimal_value_y_a = Convert.ToInt32(popülasyon[a].Substring(numberOfbitsx, numberOfbitsy), 2);

                int decimal_value_x_b = Convert.ToInt32(popülasyon[b].Substring(0, numberOfbitsx), 2);
                int decimal_value_y_b = Convert.ToInt32(popülasyon[b].Substring(numberOfbitsx, numberOfbitsy), 2);

                var (xA, yA) = DeğişkenDeğeri(numberOfbitsx, numberOfbitsy, kromozom.max_x, kromozom.min_x, kromozom.max_y, kromozom.min_y, decimal_value_x_a, decimal_value_y_a);
                var (xB, yB) = DeğişkenDeğeri(numberOfbitsx, numberOfbitsy, kromozom.max_x, kromozom.min_x, kromozom.max_y, kromozom.min_y, decimal_value_x_b, decimal_value_y_b);

                double fitnessA = Fitness(xA, yA);
                double fitnessB = Fitness(xB, yB);

                if (fitnessA > fitnessB)
                    yeniPopulasyon.Add(popülasyon[a]);
                else
                    yeniPopulasyon.Add(popülasyon[b]);
            }
            return yeniPopulasyon;
        }

        public List<string> Çaprazlama(List<string> populasyon, Kromozom kromozom)
        {
            List<string> yeniPopulasyon = new List<string>();
            double caprazlamaOrani = ÇaprazlamaOranı;

            for (int i = 0; i < populasyon.Count; i += 2)
            {
                double rho = Form1.rnd.NextDouble();
                if (rho < caprazlamaOrani)
                {
                    if (i + 1 < populasyon.Count)
                    {
                        string kromozomA = populasyon[i];
                        string kromozomB = populasyon[i + 1];

                        int crossoverPoint = Form1.rnd.Next(1, kromozomA.Length - 1);

                        string yeniKromozomA = kromozomA.Substring(0, crossoverPoint) + kromozomB.Substring(crossoverPoint);
                        string yeniKromozomB = kromozomB.Substring(0, crossoverPoint) + kromozomA.Substring(crossoverPoint);

                        yeniPopulasyon.Add(yeniKromozomA);
                        yeniPopulasyon.Add(yeniKromozomB);
                    }
                }
                else
                {
                    yeniPopulasyon.Add(populasyon[i]);
                    if (i + 1 < populasyon.Count)
                    {
                        yeniPopulasyon.Add(populasyon[i + 1]);
                    }
                }
            }
            return yeniPopulasyon;
        }

        public List<string> Mutasyon(List<string> popülasyon, Kromozom kromozom)
        {
            List<string> yeniPopulasyon = new List<string>();
            double mutasyonOranı = MutasyonOranı;

            foreach (var kromozomBit in popülasyon)
            {
                char[] yeniKromozom = kromozomBit.ToCharArray();

                for (int i = 0; i < yeniKromozom.Length; i++)
                {
                    if (Form1.rnd.NextDouble() < mutasyonOranı)
                    {
                        yeniKromozom[i] = (yeniKromozom[i] == '0') ? '1' : '0';
                    }
                }
                yeniPopulasyon.Add(new string(yeniKromozom));
            }

            return yeniPopulasyon;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}