using System.Windows.Forms;

namespace FuzzyLogic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = 10;
            numericUpDown1.DecimalPlaces = 1; 
            numericUpDown1.Increment = 0.1M;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Minimum = 0;
            numericUpDown2.Maximum = 10;
            numericUpDown2.DecimalPlaces = 1; 
            numericUpDown2.Increment = 0.1M;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown3.Minimum = 0;
            numericUpDown3.Maximum = 10;
            numericUpDown3.DecimalPlaces = 1;
            numericUpDown3.Increment = 0.1M;
        }
        double TriangularMembership(double x, double a, double b, double c)
        {
            if (x <= a)
                return 0;
            else if (a <= x && x <= b)
                return (x - a) / (b - a);
            else if (b <= x && x <= c)
                return (c - x) / (c - b);
            else if (x >= c)
                return 0;
            else
                return 0;
        }

        double TrapezoidalMembership(double x, double a, double b, double c, double d)
        {
            if (x <= a || x >= d)
                return 0;
            else if (a <= x && x <= b)
                return (x - a) / (b - a);
            else if (b <= x && x <= c)
                return 1;
            else if ((c <= x && x <= d))
                return (d - x) / (d - c);
            else
                return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double hassaslik = (double)numericUpDown1.Value;
            double miktar = (double)numericUpDown2.Value;
            double kirlilik = (double)numericUpDown3.Value;

            double hassaslik_saglam = TrapezoidalMembership(hassaslik, -4, -1.5, 2, 4);
            double hassaslik_orta = TriangularMembership(hassaslik, 3, 5, 7);
            double hassaslik_hassas = TrapezoidalMembership(hassaslik, 5.5, 8, 12.5, 14);

            double miktar_kucuk = TrapezoidalMembership(miktar, -4, -1.5, 2, 4);
            double miktar_orta = TriangularMembership(miktar, 3, 5, 7);
            double miktar_buyuk = TrapezoidalMembership(miktar, 5.5, 8, 12.5, 14);

            double kirlilik_kucuk = TrapezoidalMembership(kirlilik, -4.5, -2.5, 2, 4.5);
            double kirlilik_orta = TriangularMembership(kirlilik, 3, 5, 7);
            double kirlilik_buyuk = TrapezoidalMembership(kirlilik, 5.5, 8, 12.5, 15);

            MessageBox.Show(
                $"Hassaslýk:\nSaðlam: {hassaslik_saglam:F2}, Orta: {hassaslik_orta:F2}, Hassas: {hassaslik_hassas:F2}\n\n" +
                $"Miktar:\nKüçük: {miktar_kucuk:F2}, Orta: {miktar_orta:F2}, Büyük: {miktar_buyuk:F2}\n\n" +
                $"Kirlilik:\nKüçük: {kirlilik_kucuk:F2}, Orta: {kirlilik_orta:F2}, Büyük: {kirlilik_buyuk:F2}"
            );

            List<(double agirlik, string bulanýkGirisler, string donus, string sure, string deterjan)> kurallar = new List<(double, string, string, string, string)>();

            void Ekle(double hassaslik_deg, double miktar_deg, double kirlilik_deg, string donus, string sure, string deterjan)
            {
                string hassaslik_label = "Hassaslýk";
                string miktar_label = "Miktar";
                string kirlilik_label = "Kirlilik";

                double agirlik = Math.Min(hassaslik_deg, Math.Min(miktar_deg, kirlilik_deg));
                if (agirlik > 0)
                {
                    kurallar.Add((agirlik, $"{hassaslik_label} ({hassaslik_deg:F2}), {miktar_label} ({miktar_deg:F2}), {kirlilik_label} ({kirlilik_deg:F2})", donus, sure, deterjan));
                }
            }

            Ekle(hassaslik_hassas, miktar_kucuk, kirlilik_kucuk, "hassas", "kisa", "cok_az");
            Ekle(hassaslik_hassas, miktar_kucuk, kirlilik_orta, "normal_hassas", "kisa", "az");
            Ekle(hassaslik_hassas, miktar_kucuk, kirlilik_buyuk, "orta", "normal_kisa", "orta");

            Ekle(hassaslik_hassas, miktar_orta, kirlilik_kucuk, "hassas", "kisa", "orta");
            Ekle(hassaslik_hassas, miktar_orta, kirlilik_orta, "normal_hassas", "normal_kisa", "orta");
            Ekle(hassaslik_hassas, miktar_orta, kirlilik_buyuk, "orta", "orta", "fazla");

            Ekle(hassaslik_hassas, miktar_buyuk, kirlilik_kucuk, "normal_hassas", "normal_kisa", "orta");
            Ekle(hassaslik_hassas, miktar_buyuk, kirlilik_orta, "normal_hassas", "orta", "fazla");
            Ekle(hassaslik_hassas, miktar_buyuk, kirlilik_buyuk, "orta", "normal_uzun", "fazla");

            Ekle(hassaslik_orta, miktar_kucuk, kirlilik_kucuk, "normal_hassas", "normal_kisa", "az");
            Ekle(hassaslik_orta, miktar_kucuk, kirlilik_orta, "orta", "kisa", "orta");
            Ekle(hassaslik_orta, miktar_kucuk, kirlilik_buyuk, "normal_güçlü", "orta", "fazla");

            Ekle(hassaslik_orta, miktar_orta, kirlilik_kucuk, "normal_hassas", "normal_kisa", "orta");
            Ekle(hassaslik_orta, miktar_orta, kirlilik_orta, "orta", "orta", "orta");
            Ekle(hassaslik_orta, miktar_orta, kirlilik_buyuk, "hassas", "uzun", "fazla");

            Ekle(hassaslik_orta, miktar_buyuk, kirlilik_kucuk, "hassas", "orta", "orta");
            Ekle(hassaslik_orta, miktar_buyuk, kirlilik_orta, "hassas", "normal_uzun", "fazla");
            Ekle(hassaslik_orta, miktar_buyuk, kirlilik_buyuk, "hassas", "uzun", "cok_fazla");

            Ekle(hassaslik_saglam, miktar_kucuk, kirlilik_kucuk, "orta", "orta", "az");
            Ekle(hassaslik_saglam, miktar_kucuk, kirlilik_orta, "normal_güçlü", "orta", "orta");
            Ekle(hassaslik_saglam, miktar_kucuk, kirlilik_buyuk, "güçlü", "normal_uzun", "fazla");

            Ekle(hassaslik_saglam, miktar_orta, kirlilik_kucuk, "orta", "orta", "orta");
            Ekle(hassaslik_saglam, miktar_orta, kirlilik_orta, "normal_güçlü", "normal_uzun", "orta");
            Ekle(hassaslik_saglam, miktar_orta, kirlilik_buyuk, "güçlü", "orta", "cok_fazla");

            Ekle(hassaslik_saglam, miktar_buyuk, kirlilik_kucuk, "normal_güçlü", "normal_uzun", "fazla");
            Ekle(hassaslik_saglam, miktar_buyuk, kirlilik_orta, "normal_güçlü", "uzun", "fazla");
            Ekle(hassaslik_saglam, miktar_buyuk, kirlilik_buyuk, "güçlü", "uzun", "cok_fazla");

            Dictionary<string, double> DönüsHiziMerkez = new Dictionary<string, double>
            {
                { "hassas", 0.5 },
                { "normal_hassas", 2.75 },
                { "orta", 5 },
                { "normal_güçlü", 7.25 },
                { "güçlü", 9.5 }
            };

            Dictionary<string, double> SureMerkez = new Dictionary<string, double>
            {
                { "kisa", 22.3 },
                { "normal_kisa", 39.9 },
                { "orta", 57.5 },
                { "normal_uzun", 75.1 },
                { "uzun", 92.7 }
            };

            Dictionary<string, double> DeterjanMerkez = new Dictionary<string, double>
            {
                { "cok_az", 10 },
                { "az", 85 },
                { "orta", 150 },
                { "fazla", 215 },
                { "cok_fazla", 290 }
            };

            double toplam_agirlik_donus = 0, toplam_donus = 0;
            double toplam_agirlik_sure = 0, toplam_sure = 0;
            double toplam_agirlik_deterjan = 0, toplam_deterjan = 0;

            foreach (var kural in kurallar)
            {
                double agirlik = kural.Item1;

                toplam_donus += agirlik * DönüsHiziMerkez[kural.Item3];
                toplam_agirlik_donus += agirlik;

                toplam_sure += agirlik * SureMerkez[kural.Item4];
                toplam_agirlik_sure += agirlik;

                toplam_deterjan += agirlik * DeterjanMerkez[kural.Item5];
                toplam_agirlik_deterjan += agirlik;
            }

            double donusHiziSonuc = toplam_donus / toplam_agirlik_donus;
            double sureSonuc = toplam_sure / toplam_agirlik_sure;
            double deterjanSonuc = toplam_deterjan / toplam_agirlik_deterjan;

            string tetiklenenKurallar = "";
            foreach (var kural in kurallar)
            {
                tetiklenenKurallar +=
                    $"- Giriþler: {kural.Item2} , Mamdani Deðeri (Aðýrlýk): {kural.Item1:F2}\n" +
                    $"  Çýktýlar: Dönüþ: {kural.Item3}, Süre: {kural.Item4}, Deterjan: {kural.Item5}\n\n";
            }

            MessageBox.Show(
                $"Dönüþ Hýzý: {donusHiziSonuc:F2}\n" +
                $"Süre: {sureSonuc:F2}\n" +
                $"Deterjan Miktarý: {deterjanSonuc:F2}\n\n" +
                $"Tetiklenen {kurallar.Count} kural gösterildi." +
                tetiklenenKurallar
            ); 

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
