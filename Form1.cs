/****************************************************************************
**					      SAKARYA ÜNİVERSİTESİ
**				BİLGİSAYAR VE BİLİŞİM BİLİMLERİ FAKÜLTESİ
**				     BİLGİSAYAR MÜHENDİSLİĞİ BÖLÜMÜ
**				    NESNEYE DAYALI PROGRAMLAMA DERSİ
**					     2023-2024 BAHAR DÖNEMİ
**	
**				ÖDEV NUMARASI: BAHAR DÖNEMİ- 2. Ödev, 2.soru
**				ÖĞRENCİ ADI: NİSA NUR GÜREL
**				ÖĞRENCİ NUMARASI: G221210079
**              DERSİN ALINDIĞI GRUP: 2.ÖĞRETİM/C
****************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace nedp2
{
    public partial class Form1 : Form
    {
        private List<GeometrikSekil> sekiller = new List<GeometrikSekil>();
        private Random random = new Random();
        private bool carpismaVar = false;
        private GeometrikSekil[] nesneler;
        public Form1()
        {
            InitializeComponent();
            random = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
        }
        public abstract class GeometrikSekil
        {
            public int Boyut { get; set; } // 2 boyutlu için 2, 3 boyutlu için 3
            public abstract void Ciz(Graphics g);
            public abstract bool Kapsar(Point point); // Şekil belirli bir noktayı kapsıyor mu kontrol eder
        }

        public class Nokta : GeometrikSekil
        {
            public Nokta(Point konum)
            {
                Konum = konum;
                Boyut = 2;
            }

            public Point Konum { get; set; }

            public override void Ciz(Graphics g)
            {
                g.FillEllipse(Brushes.Black, Konum.X - 2, Konum.Y - 2, 5, 5);
            }

            public override bool Kapsar(Point point)
            {
                // Nokta sadece kendi konumunu kapsar
                return Konum == point;
            }
        }

        public class Cember : GeometrikSekil
        {
            public Cember(Point merkez, int yaricap)
            {
                Merkez = merkez;
                Yaricap = yaricap;
                Boyut = 2;
            }

            public Point Merkez { get; set; }
            public int Yaricap { get; set; }

            public override void Ciz(Graphics g)
            {
                g.DrawEllipse(Pens.Black, Merkez.X - Yaricap, Merkez.Y - Yaricap, Yaricap * 2, Yaricap * 2);
            }

            public override bool Kapsar(Point point)
            {
                // Cember, merkezi ile nokta arasındaki mesafeyi yarıçap ile karşılaştırır
                double mesafe = Math.Sqrt(Math.Pow(Merkez.X - point.X, 2) + Math.Pow(Merkez.Y - point.Y, 2));
                return mesafe <= Yaricap;
            }
        }

        public class Dikdortgen : GeometrikSekil
        {
            public Dikdortgen(Rectangle dikdortgenAlan)
            {
                DikdortgenAlan = dikdortgenAlan;
                Boyut = 2;
            }

            public Rectangle DikdortgenAlan { get; set; }

            public override void Ciz(Graphics g)
            {
                g.DrawRectangle(Pens.Black, DikdortgenAlan);
            }

            public override bool Kapsar(Point point)
            {
                // Dikdörtgen, içinde bir nokta varsa true döner
                return DikdortgenAlan.Contains(point);
            }
        }

        public class Kure : GeometrikSekil
        {
            public Point Merkez { get; set; }
            public int Yaricap { get; set; }

            public Kure(Point merkez, int yaricap)
            {
                Merkez = merkez;
                Yaricap = yaricap;
                Boyut = 3; // Küre 3 boyutlu bir şekil olduğu için boyutu 3 olarak belirtiyoruz
            }

            public override void Ciz(Graphics g)
            {
                // Küreyi çiz
                g.DrawEllipse(Pens.Black, Merkez.X - Yaricap, Merkez.Y - Yaricap, Yaricap * 2, Yaricap * 2);
            }

            public override bool Kapsar(Point point)
            {
                // Kürenin merkezi ile nokta arasındaki mesafeyi yarıçap ile karşılaştırır
                double mesafe = Math.Sqrt(Math.Pow(Merkez.X - point.X, 2) + Math.Pow(Merkez.Y - point.Y, 2));
                return mesafe <= Yaricap;
            }
        }

        public class Yuzey : GeometrikSekil
        {
            public Rectangle Alan { get; set; }

            public Yuzey(Rectangle alan)
            {
                Alan = alan;
                Boyut = 2; // Yüzey 2 boyutlu bir şekil olduğu için boyutu 2 olarak belirtiyoruz
            }

            public override void Ciz(Graphics g)
            {
                // Yüzeyi çiz
                g.FillRectangle(Brushes.LightGray, Alan);
                g.DrawRectangle(Pens.Black, Alan);
            }

            public override bool Kapsar(Point point)
            {
                return Alan.Contains(point);
            }
        }

        public class Silindir : GeometrikSekil
        {
            public Rectangle Taban { get; set; }
            public int Yukseklik { get; set; }

            public Silindir(Rectangle taban, int yukseklik)
            {
                Taban = taban;
                Yukseklik = yukseklik;
                Boyut = 3; // Silindir 3 boyutlu bir şekil olduğu için boyutu 3 olarak belirtiyoruz
            }

            public override void Ciz(Graphics g)
            {
                // Silindirin tabanını ve tavanını çiz
                g.DrawEllipse(Pens.Black, Taban);
                Rectangle tavan = new Rectangle(Taban.X, Taban.Y - Yukseklik, Taban.Width, Yukseklik);
                g.DrawEllipse(Pens.Black, tavan);

                // Yan yüzeyleri çiz
                g.DrawLine(Pens.Black, Taban.Left, Taban.Top, tavan.Left, tavan.Top);
                g.DrawLine(Pens.Black, Taban.Right, Taban.Top, tavan.Right, tavan.Top);
                g.DrawLine(Pens.Black, Taban.Left, Taban.Bottom, tavan.Left, tavan.Bottom);
                g.DrawLine(Pens.Black, Taban.Right, Taban.Bottom, tavan.Right, tavan.Bottom);
            }

            public override bool Kapsar(Point point)
            {
                // Silindir, tabanını veya tavanını kapsar
                if (Taban.Contains(point) || new Rectangle(Taban.X, Taban.Y - Yukseklik, Taban.Width, Yukseklik).Contains(point))
                    return true;

                // Yan yüzeyleri kontrol et
                if (point.X >= Taban.Left && point.X <= Taban.Right)
                {
                    int y1 = Taban.Y;
                    int y2 = Taban.Y - Yukseklik;
                    return point.Y >= Math.Min(y1, y2) && point.Y <= Math.Max(y1, y2);
                }

                return false;
            }
        }

        public class DikdortgenPrizma : GeometrikSekil
        {
            public Rectangle Taban { get; set; }
            public int Yukseklik { get; set; }

            public DikdortgenPrizma(Rectangle taban, int yukseklik)
            {
                Taban = taban;
                Yukseklik = yukseklik;
                Boyut = 3; // Dikdörtgen Prizma 3 boyutlu bir şekil olduğu için boyutu 3 olarak belirtiyoruz
            }

            public override void Ciz(Graphics g)
            {
                // Dikdörtgen prizmanın tabanını ve üst yüzeyini çiz
                g.DrawRectangle(Pens.Black, Taban);
                Rectangle tavan = new Rectangle(Taban.X, Taban.Y - Yukseklik, Taban.Width, Yukseklik);
                g.DrawRectangle(Pens.Black, tavan);

                // Yan yüzeyleri çiz
                g.DrawLine(Pens.Black, Taban.Left, Taban.Top, tavan.Left, tavan.Top);
                g.DrawLine(Pens.Black, Taban.Right, Taban.Top, tavan.Right, tavan.Top);
                g.DrawLine(Pens.Black, Taban.Left, Taban.Bottom, tavan.Left, tavan.Bottom);
                g.DrawLine(Pens.Black, Taban.Right, Taban.Bottom, tavan.Right, tavan.Bottom);
            }

            public override bool Kapsar(Point point)
            {
                // Dikdörtgen prizma, tabanını veya tavanını kapsar
                if (Taban.Contains(point) || new Rectangle(Taban.X, Taban.Y - Yukseklik, Taban.Width, Yukseklik).Contains(point))
                    return true;

                // Yan yüzeyleri kontrol et
                if (point.X >= Taban.Left && point.X <= Taban.Right)
                {
                    int y1 = Taban.Y;
                    int y2 = Taban.Y - Yukseklik;
                    return point.Y >= Math.Min(y1, y2) && point.Y <= Math.Max(y1, y2);
                }

                return false;
            }
        }

        public class Dortgen : GeometrikSekil
        {
            public Point[] Noktalar { get; set; }

            public Dortgen(Point[] noktalar)
            {
                Noktalar = noktalar;
                Boyut = 2; // Dörtgen 2 boyutlu bir şekil olduğu için boyutu 2 olarak belirtiyoruz
            }

            public override void Ciz(Graphics g)
            {
                g.DrawPolygon(Pens.Black, Noktalar);
            }

            public override bool Kapsar(Point point)
            {
                // Dörtgenin içindeki bir nokta ise true döner
                return IsPointInPolygon(Noktalar, point);
            }

            // Nokta dörtgenin içinde mi kontrol eder
            private bool IsPointInPolygon(Point[] polygon, Point point)
            {
                int i, j;
                bool c = false;
                for (i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
                {
                    if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                        (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                        c = !c;
                }
                return c;
            }
        }

        public static class CarpismaDenetimi
        {
            public static bool NoktaDortgen(Nokta nokta, Dikdortgen dikdortgen)
            {
                return nokta.Konum.X >= dikdortgen.DikdortgenAlan.Left &&
                       nokta.Konum.X <= dikdortgen.DikdortgenAlan.Right &&
                       nokta.Konum.Y >= dikdortgen.DikdortgenAlan.Top &&
                       nokta.Konum.Y <= dikdortgen.DikdortgenAlan.Bottom;
            }

            public static bool NoktaCember(Nokta nokta, Cember cember)
            {
                int dx = nokta.Konum.X - cember.Merkez.X;
                int dy = nokta.Konum.Y - cember.Merkez.Y;
                double mesafe = Math.Sqrt(dx * dx + dy * dy);
                return mesafe <= cember.Yaricap;
            }

            public static bool DikdortgenDikdortgen(Dikdortgen dikdortgen1, Dikdortgen dikdortgen2)
            {
                return dikdortgen1.DikdortgenAlan.IntersectsWith(dikdortgen2.DikdortgenAlan);
            }

            public static bool DikdortgenCember(Dikdortgen dikdortgen, Cember cember)
            {
                int closestX = Math.Max(dikdortgen.DikdortgenAlan.Left, Math.Min(cember.Merkez.X, dikdortgen.DikdortgenAlan.Right));
                int closestY = Math.Max(dikdortgen.DikdortgenAlan.Top, Math.Min(cember.Merkez.Y, dikdortgen.DikdortgenAlan.Bottom));
                int dx = closestX - cember.Merkez.X;
                int dy = closestY - cember.Merkez.Y;
                double mesafe = Math.Sqrt(dx * dx + dy * dy);
                return mesafe <= cember.Yaricap;
            }

            public static bool CemberCember(Cember cember1, Cember cember2)
            {
                int dx = cember1.Merkez.X - cember2.Merkez.X;
                int dy = cember1.Merkez.Y - cember2.Merkez.Y;
                double mesafe = Math.Sqrt(dx * dx + dy * dy);
                return mesafe <= cember1.Yaricap + cember2.Yaricap;
            }

            public static bool NoktaKure(Nokta nokta, Kure kure)
            {
                int dx = nokta.Konum.X - kure.Merkez.X;
                int dy = nokta.Konum.Y - kure.Merkez.Y;
                double mesafe = Math.Sqrt(dx * dx + dy * dy);
                return mesafe <= kure.Yaricap;
            }

            public static bool NoktaSilindir(Nokta nokta, Silindir silindir)
            {
                double mesafe = Math.Sqrt((nokta.Konum.X - silindir.Taban.Left) * (nokta.Konum.X - silindir.Taban.Left) + (nokta.Konum.Y - silindir.Taban.Top) * (nokta.Konum.Y - silindir.Taban.Top));
                return mesafe <= silindir.Taban.Width / 2;
            }

            public static bool SilindirSilindir(Silindir silindir1, Silindir silindir2)
            {
                double mesafe = Math.Sqrt((silindir1.Taban.Left - silindir2.Taban.Left) * (silindir1.Taban.Left - silindir2.Taban.Left) + (silindir1.Taban.Top - silindir2.Taban.Top) * (silindir1.Taban.Top - silindir2.Taban.Top));
                return mesafe <= silindir1.Taban.Width / 2 + silindir2.Taban.Width / 2;
            }

            public static bool KureKure(Kure kure1, Kure kure2)
            {
                int dx = kure1.Merkez.X - kure2.Merkez.X;
                int dy = kure1.Merkez.Y - kure2.Merkez.Y;
                double mesafe = Math.Sqrt(dx * dx + dy * dy);
                return mesafe <= kure1.Yaricap + kure2.Yaricap;
            }

            public static bool KureSilindir(Kure kure, Silindir silindir)
            {
                double mesafe = Math.Sqrt((kure.Merkez.X - silindir.Taban.Left) * (kure.Merkez.X - silindir.Taban.Left) + (kure.Merkez.Y - silindir.Taban.Top) * (kure.Merkez.Y - silindir.Taban.Top));
                return mesafe <= kure.Yaricap + silindir.Taban.Width / 2;
            }

            public static bool YuzeyKure(Yuzey yuzey, Kure kure)
            {
                return yuzey.Alan.IntersectsWith(new Rectangle(kure.Merkez.X - kure.Yaricap, kure.Merkez.Y - kure.Yaricap, kure.Yaricap * 2, kure.Yaricap * 2));
            }

            public static bool YuzeySilindir(Yuzey yuzey, Silindir silindir)
            {
                return yuzey.Alan.IntersectsWith(silindir.Taban);
            }

            public static bool YuzeyDikdortgenPrizma(Yuzey yuzey, DikdortgenPrizma prizma)
            {
                return yuzey.Alan.IntersectsWith(prizma.Taban);
            }

            public static bool KureDikdortgenPrizma(Kure kure, DikdortgenPrizma prizma)
            {
                return false; // Örnek amaçlı false döndürüldü
            }

            public static bool DikdortgenPrizmaDikdortgenPrizma(DikdortgenPrizma prizma1, DikdortgenPrizma prizma2)
            {
                return prizma1.Taban.IntersectsWith(prizma2.Taban);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Paneli temizle
            e.Graphics.Clear(panel1.BackColor);

            // Nesneleri panel üzerine çiz
            foreach (var sekil in sekiller)
            {
                sekil.Ciz(e.Graphics);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sekiller.Add(new Nokta(new Point(random.Next(panel1.Width - 5), random.Next(panel1.Height - 5))));
            panel1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sekiller.Add(new Cember(new Point(random.Next(panel1.Width), random.Next(panel1.Height)), random.Next(20, Math.Min(panel1.Width, panel1.Height) / 2)));
            panel1.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sekiller.Add(new Dikdortgen(new Rectangle(random.Next(panel1.Width - 80), random.Next(panel1.Height - 80), random.Next(20, 80), random.Next(20, 80))));
            panel1.Refresh();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sekiller.Add(new Kure(new Point(random.Next(panel1.Width), random.Next(panel1.Height)), random.Next(20, Math.Min(panel1.Width, panel1.Height) / 2)));
            panel1.Refresh();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sekiller.Add(new Yuzey(new Rectangle(random.Next(panel1.Width - 100), random.Next(panel1.Height - 100), random.Next(50, 100), random.Next(50, 100))));
            panel1.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sekiller.Add(new Silindir(new Rectangle(random.Next(panel1.Width - 50), random.Next(panel1.Height - 50), random.Next(20, 50), random.Next(20, 50)), random.Next(50, 100)));
            panel1.Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sekiller.Add(new DikdortgenPrizma(new Rectangle(random.Next(panel1.Width - 80), random.Next(panel1.Height - 80), random.Next(20, 80), random.Next(20, 80)), random.Next(50, 100)));
            panel1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Point[] noktalar = { new Point(random.Next(panel1.Width), random.Next(panel1.Height)),
                         new Point(random.Next(panel1.Width), random.Next(panel1.Height)),
                         new Point(random.Next(panel1.Width), random.Next(panel1.Height)),
                         new Point(random.Next(panel1.Width), random.Next(panel1.Height)) };

            sekiller.Add(new Dortgen(noktalar));
            panel1.Refresh();
        }

        private GeometrikSekil secilenSekil;

        private void button9_Click(object sender, EventArgs e)
        {
            if (sekiller.Count < 2)
            {
                MessageBox.Show("Lütfen en az 2 nesne ekleyin.");
                return;
            }
            else if (sekiller.Count > 2)
            {
                MessageBox.Show("Sadece 2 nesne ekleyebilirsiniz.");
                sekiller.Clear();
                panel1.Invalidate();
                return;
            }

            // İlk iki nesneyi al ve çarpışmayı kontrol et
            GeometrikSekil sekil1 = sekiller[0];
            GeometrikSekil sekil2 = sekiller[1];

            bool carpismaVar = CarpismaTespit(sekil1, sekil2);

            if (carpismaVar)
            {
                panel1.BackColor = Color.Green;
                MessageBox.Show("Çarpışma tespit edildi!");
            }
            else
            {
                panel1.BackColor = Color.Red;
                MessageBox.Show("Çarpışma tespit edilmedi.");
            }

            // Panel rengini beyaza döndür
            panel1.BackColor = Color.White;

            // Yeni iki nesne için paneli temizle
            sekiller.Clear();
            panel1.Invalidate();
        }

            private bool CarpismaTespit(GeometrikSekil sekil1, GeometrikSekil sekil2)
        {
            if (sekil1 is Nokta && sekil2 is Dikdortgen)
            {
                Nokta nokta = (Nokta)sekil1;
                Dikdortgen dikdortgen = (Dikdortgen)sekil2;
                return CarpismaDenetimi.NoktaDortgen(nokta, dikdortgen);
            }
            else if (sekil1 is Nokta && sekil2 is Cember)
            {
                Nokta nokta = (Nokta)sekil1;
                Cember cember = (Cember)sekil2;
                return CarpismaDenetimi.NoktaCember(nokta, cember);
            }
            else if (sekil1 is Dikdortgen && sekil2 is Dikdortgen)
            {
                Dikdortgen dikdortgen1 = (Dikdortgen)sekil1;
                Dikdortgen dikdortgen2 = (Dikdortgen)sekil2;
                return CarpismaDenetimi.DikdortgenDikdortgen(dikdortgen1, dikdortgen2);
            }
            else if (sekil1 is Dikdortgen && sekil2 is Cember)
            {
                Dikdortgen dikdortgen = (Dikdortgen)sekil1;
                Cember cember = (Cember)sekil2;
                return CarpismaDenetimi.DikdortgenCember(dikdortgen, cember);
            }
            else if (sekil1 is Cember && sekil2 is Cember)
            {
                Cember cember1 = (Cember)sekil1;
                Cember cember2 = (Cember)sekil2;
                return CarpismaDenetimi.CemberCember(cember1, cember2);
            }
            else if (sekil1 is Nokta && sekil2 is Kure)
            {
                Nokta nokta = (Nokta)sekil1;
                Kure kure = (Kure)sekil2;
                return CarpismaDenetimi.NoktaKure(nokta, kure);
            }
            else if (sekil1 is Nokta && sekil2 is Silindir)
            {
                Nokta nokta = (Nokta)sekil1;
                Silindir silindir = (Silindir)sekil2;
                return CarpismaDenetimi.NoktaSilindir(nokta, silindir);
            }
            else if (sekil1 is Silindir && sekil2 is Silindir)
            {
                Silindir silindir1 = (Silindir)sekil1;
                Silindir silindir2 = (Silindir)sekil2;
                return CarpismaDenetimi.SilindirSilindir(silindir1, silindir2);
            }
            else if (sekil1 is Kure && sekil2 is Kure)
            {
                Kure kure1 = (Kure)sekil1;
                Kure kure2 = (Kure)sekil2;
                return CarpismaDenetimi.KureKure(kure1, kure2);
            }
            else if (sekil1 is Kure && sekil2 is Silindir)
            {
                Kure kure = (Kure)sekil1;
                Silindir silindir = (Silindir)sekil2;
                return CarpismaDenetimi.KureSilindir(kure, silindir);
            }
            else if (sekil1 is Yuzey && sekil2 is Kure)
            {
                Yuzey yuzey = (Yuzey)sekil1;
                Kure kure = (Kure)sekil2;
                return CarpismaDenetimi.YuzeyKure(yuzey, kure);
            }
            else if (sekil1 is Yuzey && sekil2 is Silindir)
            {
                Yuzey yuzey = (Yuzey)sekil1;
                Silindir silindir = (Silindir)sekil2;
                return CarpismaDenetimi.YuzeySilindir(yuzey, silindir);
            }
            else if (sekil1 is Yuzey && sekil2 is DikdortgenPrizma)
            {
                Yuzey yuzey = (Yuzey)sekil1;
                DikdortgenPrizma prizma = (DikdortgenPrizma)sekil2;
                return CarpismaDenetimi.YuzeyDikdortgenPrizma(yuzey, prizma);
            }
            else if (sekil1 is Kure && sekil2 is DikdortgenPrizma)
            {
                Kure kure = (Kure)sekil1;
                DikdortgenPrizma prizma = (DikdortgenPrizma)sekil2;
                return CarpismaDenetimi.KureDikdortgenPrizma(kure, prizma);
            }
            else if (sekil1 is DikdortgenPrizma && sekil2 is DikdortgenPrizma)
            {
                DikdortgenPrizma prizma1 = (DikdortgenPrizma)sekil1;
                DikdortgenPrizma prizma2 = (DikdortgenPrizma)sekil2;
                return CarpismaDenetimi.DikdortgenPrizmaDikdortgenPrizma(prizma1, prizma2);
            }

            return false; // Herhangi bir eşleşme olmadığında false döndür
        }

        private bool cizimYapilabilir = true;

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!cizimYapilabilir || sekiller.Count >= 2)
            {
                MessageBox.Show("Sadece 2 nesne ekleyebilirsiniz.");
                sekiller.Clear();
                panel1.Invalidate();
                return;
            }

            GeometrikSekil secilenSekil = null;

            foreach (var sekil in sekiller)
            {
                if (sekil.Kapsar(e.Location))
                {
                    secilenSekil = sekil;
                    break;
                }
            }

            if (secilenSekil == null)
            {
                MessageBox.Show("Lütfen geçerli bir nesne seçin.");
                return;
            }

            sekiller.Add(secilenSekil);

            if (sekiller.Count == 2)
            {
                // İki nesne seçildi
                cizimYapilabilir = false;

                // Otomatik olarak çarpışma denetimine geç
                GeometrikSekil sekil1 = sekiller[0];
                GeometrikSekil sekil2 = sekiller[1];

                bool carpismaVar = CarpismaTespit(sekil1, sekil2);

                if (carpismaVar)
                {
                    panel1.BackColor = Color.Green;
                    MessageBox.Show("Çarpışma tespit edildi!");
                }
                else
                {
                    panel1.BackColor = Color.Red;
                    MessageBox.Show("Çarpışma tespit edilmedi.");
                }

                // Yeni iki nesne için paneli temizle
                sekiller.Clear();
                cizimYapilabilir = true;
                panel1.BackColor = Color.White;
                panel1.Invalidate();
            }
        }
    }
}
        

