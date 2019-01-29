using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace filtry_graficzne_csharp
{
    public delegate void fnPrzekazRozmiarMaski(string sRozmiar, string sType);
    public partial class filtry : Form
    {
        /// <summary>
        /// przetwarzana bitmapa
        /// </summary>
        Bitmap oBitmapa;
        /// <summary>
        /// oryginalna bitmapa
        /// </summary>
        Bitmap oBitmapaOrginal;

        /// <summary>
        /// 
        /// </summary>
        public filtry()
        {
            InitializeComponent();
            label4.Text = "";
            setOryginal.Enabled = false;
            cbJeszczeRaz.Enabled = false;
        }

        /// <summary>
        /// otworzenie okna z wyborem pliku graficzego
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();

            /// ustawienie dla okna wyboru pliku
            dlgOpen.Title = "Wybierz plik graficzny";
            dlgOpen.ShowReadOnly = true;
            dlgOpen.Multiselect = false;
            dlgOpen.Filter = "Plik graficzny (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            /// jesli nastapilo wybranie obrazka
            /// nalezy przypisac go do lewego boksa
            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                string sFileName = dlgOpen.FileName;
                oBitmapa = new Bitmap(sFileName);

                /// przypisanie do obiektu zeby miec calyczas oryginal
                oBitmapaOrginal = new Bitmap(sFileName);

                pictureBox1.Image = oBitmapa;
            }
        }

        /// <summary>
        /// zastosowanie wybranego filtra na obrazku, jesli takiego nie ma wybranego
        /// ładowany jest domyslny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Nie została wybrana żadna grafika, zostanie załadowana domyślna!");
                pictureBox1.Image = oBitmapa = (Bitmap)Properties.Resources.lena;
            }

            if (pictureBox1.Image != null)
            {
                wielkosc_maski oWielkoscMaski = new wielkosc_maski();

                label4.Text = "Użyty filtr: " + comboBox1.Text;

                oBitmapa = (setOryginal.Checked) ? oBitmapaOrginal : oBitmapa;
                oBitmapa = (!cbJeszczeRaz.Checked) ? (Bitmap)pictureBox1.Image : (Bitmap)pictureBox2.Image;

                switch (comboBox1.SelectedIndex)
                {
                    case 0: // czarno-bialy
                        rtOpis.Text = "Przekonwertowanie kolorowej grafiki na czarno-białą";
                        pictureBox2.Image = czarnoBialy();
                        break;

                    case 1: // negatyw
                        rtOpis.Text = "Stworzenie negatywu grafiki";
                        pictureBox2.Image = negatyw();
                        break;

                    case 2: // usun srednia
                        rtOpis.Text = "Jest to podstawowa wersja filtru górnoprzepustowego, jego użycie powoduje znaczne wyostrzenie obrazu, ale także wzmocnienie wszelkich szumów i zakłóceń.";
                        pictureBox2.Image = meanRemoval();
                        break;

                    case 3: // poziomy
                        rtOpis.Text = "Wykonuje przesunięcie obrazu o jeden punkt w kierunku poziomym a następnie odjęcie wartości punktu od jego kopii, w ten sposób wykrywa krawędzie pionowe w obrazie.";
                        pictureBox2.Image = filtrPoziomy();
                        break;

                    case 4: // pionowy
                        rtOpis.Text = "Wykonuje przesunięcie obrazu o jeden punkt w kierunku pionowym a następnie odjęcie wartości punktu od jego kopii, w ten sposób wykrywa krawędzie poziome w obrazie.";
                        pictureBox2.Image = filtrPionowy();
                        break;

                    case 5: // minimalny
                        rtOpis.Text = "Zwany jest także filtrem kompresujacym albo erozyjnym. Jego działanie polega na wybraniu z pod maski punktu o wartości najmniejszej. Jego działanie powoduje zmniejszenie jasnosci obrazu dajace efekt erozji obiektów.";

                        oWielkoscMaski.rozmiarMaski = new fnPrzekazRozmiarMaski(this.stworzObrazek);
                        oWielkoscMaski.sType = "max";
                        oWielkoscMaski.ShowDialog();

                        break;

                    case 6: // maksymalny
                        rtOpis.Text = "Zwany jest także filtrem dekompresujacym albo ekspansywnym (dylatacyjnym). Jego działanie polega na wybraniu z pod maski punktu o wartości największej. Jego działanie powoduje zwiększenie jasnosci obrazu dajace efekt powiększania się obiektów.";

                        oWielkoscMaski.rozmiarMaski = new fnPrzekazRozmiarMaski(this.stworzObrazek);
                        oWielkoscMaski.sType = "min";
                        oWielkoscMaski.ShowDialog();

                        break;

                    case 7: // Filtr Laplace'a
                        rtOpis.Text = "Stosowany do wykrywania krawędzi. W porównaniu do innych filtrów cechuje go wielokierunkowość - wykrywa krawędzie we wszystkich kierunkach. Ponadto daje w efekcie ostrzejsze krawędzie.";
                        pictureBox2.Image = filtrLaplacea();
                        break;

                    case 8: // wyrownanie histogramu
                        pictureBox2.Image = wyrownanieHistogramu();
                        break;

                    case 9: // coherence matrix
                        pictureBox2.Image = coherenceMatrix();
                        break;
                }

                if (pictureBox2.Image != null)
                {
                    setOryginal.Enabled = true;
                    cbJeszczeRaz.Enabled = true;
                }
            }
        }

        private void stworzObrazek(string sRozmiar, string sType)
        {
            int iRozmiar = Convert.ToInt32(sRozmiar);
            // obliczenie prawdziwego rozmiary po podaniu wpolczynnika q
            iRozmiar = (2 * iRozmiar) + 1;
            MessageBox.Show("Wielkość maski to: " + Convert.ToString(iRozmiar) + "x" + Convert.ToString(iRozmiar));
            pictureBox2.Image = filtrMaxMin(iRozmiar, sType);
        }

        private Bitmap coherenceMatrix()
        {
            /// nowy obrazek
            Bitmap newBitmap = new Bitmap(oBitmapa.Width, oBitmapa.Height);
            /// stworzenie tablicy pikseli
            int[, ,] aColory = new int[oBitmapa.Width, oBitmapa.Height, 3];
            aColory = stworzMacierzPikseli();

            int iRozmiar = 2;
            int iMargines = ((iRozmiar - 1) / 2);

            for (int i = iMargines; i < oBitmapa.Width - iMargines; i++)
            {
                for (int j = iMargines; j < oBitmapa.Height - iMargines; j++)
                {

                }
            }

            return newBitmap;
        }

        private double[] LUT;
        private Bitmap wyrownanieHistogramu()
        {
            /// nowy obrazek
            Bitmap newBitmap = new Bitmap(oBitmapa.Width, oBitmapa.Height);
            /// stworzenie tablicy pikseli
            int[, ,] aColory = new int[oBitmapa.Width, oBitmapa.Height, 3];
            aColory = stworzMacierzPikseli();

            int numberOfPixels = (oBitmapa.Width) * (oBitmapa.Height);

            double sumR = 0;
            double sumG = 0;
            double sumB = 0;

            double[] r = new double[256];
            double[] g = new double[256];
            double[] b = new double[256];

            //oblicz dystrybuante
            for (int i = 0; i < oBitmapa.Width; i++)
            {
                for (int j = 0; j < oBitmapa.Height; j++)
                {
                    r[aColory[i, j, 0]]++;
                    g[aColory[i, j, 1]]++;
                    b[aColory[i, j, 2]]++;
                }
            }
            double[] Dr = new double[256];
            double[] Dg = new double[256];
            double[] Db = new double[256];
            LUT = new double[256];

            for (int i = 0; i < 256; i++)
            {
                sumR += (r[i] / numberOfPixels);
                sumG += (g[i] / numberOfPixels);
                sumB += (b[i] / numberOfPixels);

                Dr[i] += sumR;
                Dg[i] += sumG;
                Db[i] += sumB;
            }

            UpdateLUT(Dr);
            UpdateLUT(Dg);
            UpdateLUT(Db);

            for (int i = 0; i < oBitmapa.Width; i++)
            {
                for (int j = 0; j < oBitmapa.Height; j++)
                {
                    int rValue = (aColory[i, j, 0]);
                    int gValue = (aColory[i, j, 1]);
                    int bValue = (aColory[i, j, 2]);

                    /// stworzenie nowych kolorow w RGB po zastosowaniu filtra
                    Color newColor = Color.FromArgb(
                            poprawka((int)(LUT[rValue])),
                            poprawka((int)(LUT[gValue])),
                            poprawka((int)(LUT[bValue]))
                    );

                    /// dodanie kolejnych pikseli do nowego obrazka
                    newBitmap.SetPixel(i, j, newColor);
                }
            }
            return newBitmap;
        }

        public double[] UpdateLUT(double[] D)
        {
            double D0min;

            /// znajdz pierwszą niezerową wartosc dystrybuanty
            int i = 0;
            while (D[i] == 0) i++;
            D0min = D[i];

            for (i = 0; i < 256; i++)
            {
                LUT[i] = (((D[i] - D0min) / (1 - D0min)) * (256 - 1));
            }

            return LUT;
        }

        /// <summary>
        /// Filtr Laplace'a. Stosowany do wykrywania krawędzi.
        /// </summary>
        /// <param name="original">orginalna bitmapa</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap filtrLaplacea()
        {
            int[] aFiltr = new int[] {-1, -1, -1,
                                      -1,  8, -1,
                                      -1, -1, -1};

            return zastosujFiltr(aFiltr, 3);
        }

        /// <summary>
        /// Filtr pionowy. Wykonuje przesunięcie obrazu o jeden punkt w kierunku pionowym,
        /// a następnie odjęcie wartości punktu od jego kopii, 
        /// w ten sposób wykrywa krawędzie poziome w obrazie.
        /// </summary>
        /// <param name="original">orginalna bitmapa</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap filtrPionowy()
        {
            int[] aFiltr = new int[] { 0, -1,  0,
                                       0,  1,  0,
                                       0,  0,  0};

            return zastosujFiltr(aFiltr, 3);
        }

        /// <summary>
        /// Filtr poziomy. Wykonuje przesunięcie obrazu o jeden punkt w kierunku poziomym,
        /// a następnie odjęcie wartości punktu od jego kopii, 
        /// w ten sposób wykrywa krawędzie pionowe w obrazie.
        /// </summary>
        /// <param name="original">orginalna bitmapa</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap filtrPoziomy()
        {
            int[] aFiltr = new int[] { 0,  0,  0,
                                      -1,  1,  0,
                                       0,  0,  0};

            return zastosujFiltr(aFiltr, 3);
        }

        /// <summary>
        /// Filtr usuwającu średnią. Jest to podstawowa wersja filtru górnoprzepustowego, 
        /// jego użycie powoduje znaczne wyostrzenie obrazu, 
        /// ale także wzmocnienie wszelkich szumów i zakłóceń.
        /// </summary>
        /// <param name="original">orginalna bitmapa</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap meanRemoval()
        {
            int[] aFiltr = new int[] {-1, -1, -1,
                                      -1,  9, -1,
                                      -1, -1, -1};

            return zastosujFiltr(aFiltr, 3);
        }

        /// <summary>
        /// zastosowanie filtra
        /// </summary>
        /// <param name="aFiltr">tablica filtra</param>
        /// <param name="iRozmiar">ilosc kolumn</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap zastosujFiltr(int[] aFiltr, int iRozmiar)
        {
            int rSuma, gSuma, bSuma;

            /// nowy obrazek
            Bitmap newBitmap = new Bitmap(oBitmapa.Width, oBitmapa.Height);
            /// stworzenie tablicy pikseli
            int[, ,] aColory = new int[oBitmapa.Width, oBitmapa.Height, 3];
            aColory = stworzMacierzPikseli();

            /// wyliczenie marginesu obrazka zeby nie wyjsc poza obrazek
            int iMargines = ((iRozmiar - 1) / 2);

            /// zsumowanie wszystkich elementow fitra, tzw. normalizacja jadra
            int iNormalizacja = 0;
            for (int i = 0; i < iRozmiar; i++)
            {
                for (int j = 0; j < iRozmiar; j++)
                {
                    iNormalizacja += aFiltr[i + iRozmiar * j];
                }
            }

            /// poprawka normalizacji zeby nie bylo 0, bo przez 0nie da sie dzielic
            if (iNormalizacja == 0) { iNormalizacja = 1; }

            /// rastrowe zastosowanie filtra do danego obrazka z uwzglednieniem marginesu
            for (int i = iMargines; i < (oBitmapa.Width - iMargines); i++)
            {
                for (int j = iMargines; j < (oBitmapa.Height - iMargines); j++)
                {
                    rSuma = 0;
                    gSuma = 0;
                    bSuma = 0;

                    for (int k = 0; k < iRozmiar; k++)
                    {
                        for (int l = 0; l < iRozmiar; l++)
                        {
                            int iX = (i + k - iMargines);
                            int iY = (j + l - iMargines);
                            int iPozycjaZFiltra = (k * iRozmiar + l);

                            rSuma += aFiltr[iPozycjaZFiltra] * aColory[iX, iY, 0];
                            gSuma += aFiltr[iPozycjaZFiltra] * aColory[iX, iY, 1];
                            bSuma += aFiltr[iPozycjaZFiltra] * aColory[iX, iY, 2];
                        }
                    }

                    /// normalizacja
                    rSuma /= iNormalizacja;
                    gSuma /= iNormalizacja;
                    bSuma /= iNormalizacja;

                    /// poprawka zeby nie wyjsc poza zakres
                    rSuma = poprawka(rSuma);
                    gSuma = poprawka(gSuma);
                    bSuma = poprawka(bSuma);

                    /// stworzenie nowych kolorow w RGB po zastosowaniu filtra
                    Color newColor = Color.FromArgb(rSuma, gSuma, bSuma);

                    /// dodanie kolejnych pikseli do nowego obrazka
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }

        /// <summary>
        /// poprawka dla wartosci R G B zeby nie wyskoczyc poza zakres 0-255
        /// w ktorym to znajduje sie paleta kolorow
        /// </summary>
        /// <param name="iWartosc"></param>
        /// <returns></returns>
        private int poprawka(int iWartosc)
        {
            if (iWartosc > 255) { iWartosc = 255; }
            else if (iWartosc < 0) { iWartosc = 0; }

            return iWartosc;
        }

        /// <summary>
        /// stworzenie negatywu obrazka. Aby zrobic negatyw nalezy odwrocic wszsytkie kolory,
        /// czyli od kazdej skladowej RGB nalezy odjac 255 jako wartosc dla barwy bialej
        /// </summary>
        /// <param name="original">orginalna bitmapa</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap negatyw()
        {
            Bitmap newBitmap = new Bitmap(oBitmapa.Width, oBitmapa.Height);
            int[, ,] aColory = new int[oBitmapa.Width, oBitmapa.Height, 3];
            aColory = stworzMacierzPikseli();

            for (int i = 0; i < oBitmapa.Width; i++)
            {
                for (int j = 0; j < oBitmapa.Height; j++)
                {
                    Color newColor = Color.FromArgb((255 - aColory[i, j, 0]), (255 - aColory[i, j, 1]), (255 - aColory[i, j, 2]));
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }

        /// <summary>
        /// stworzenie czarnobialego obrazka. Aby zrobic obraz czarno-bialy musimy zsumowac 
        /// wartosci trzech skladowych RGB i podzielic przez ich ilosc
        /// </summary>
        /// <param name="original">orginalna bitmapa</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap czarnoBialy()
        {
            Bitmap newBitmap = new Bitmap(oBitmapa.Width, oBitmapa.Height);

            int[, ,] aColory = new int[oBitmapa.Width, oBitmapa.Height, 3];
            aColory = stworzMacierzPikseli();

            for (int i = 0; i < oBitmapa.Width; i++)
            {
                for (int j = 0; j < oBitmapa.Height; j++)
                {
                    int colorRGB = (int)(aColory[i, j, 0] + aColory[i, j, 1] + aColory[i, j, 2]) / 3;
                    Color newColor = Color.FromArgb(colorRGB, colorRGB, colorRGB);
                    newBitmap.SetPixel(i, j, newColor);
                }
            }
            return newBitmap;
        }

        /// <summary>
        /// filtr kompresujacy albo erozyjny oraz filtr dekompresujacy albo ekspansywny (dylatacja)
        /// </summary>
        /// <param name="iRozmiar">rozmiar maski</param>
        /// <param name="sTyp">typ</param>
        /// <returns>nowa bitmapa</returns>
        private Bitmap filtrMaxMin(int iRozmiar, string sTyp)
        {
            Bitmap newBitmap = new Bitmap(oBitmapa.Width, oBitmapa.Height);
            int[, ,] aColory = new int[oBitmapa.Width, oBitmapa.Height, 3];
            aColory = stworzMacierzPikseli();

            //int iRozmiar = 3;
            int rMin, gMin, bMin;

            /// wyliczenie marginesu obrazka zeby nie wyjsc poza obrazek
            int iMargines = ((iRozmiar - 1) / 2);

            for (int i = iMargines; i < oBitmapa.Width - iMargines; i++)
            {
                for (int j = iMargines; j < oBitmapa.Height - iMargines; j++)
                {
                    if (sTyp == "min")
                    {
                        rMin = 255;
                        gMin = 255;
                        bMin = 255;
                    }
                    else
                    {
                        rMin = 0;
                        gMin = 0;
                        bMin = 0;
                    }
                    for (int k = 0; k < iRozmiar; k++)
                    {
                        for (int l = 0; l < iRozmiar; l++)
                        {
                            int iX = (i + k - iMargines);
                            int iY = (j + l - iMargines);
                            if (sTyp == "min")
                            {
                                if (rMin > aColory[iX, iY, 0]) { rMin = aColory[iX, iY, 0]; }
                                if (gMin > aColory[iX, iY, 1]) { gMin = aColory[iX, iY, 1]; }
                                if (bMin > aColory[iX, iY, 2]) { bMin = aColory[iX, iY, 2]; }
                            }
                            else if (sTyp == "max")
                            {
                                if (rMin < aColory[iX, iY, 0]) { rMin = aColory[iX, iY, 0]; }
                                if (gMin < aColory[iX, iY, 1]) { gMin = aColory[iX, iY, 1]; }
                                if (bMin < aColory[iX, iY, 2]) { bMin = aColory[iX, iY, 2]; }
                            }
                        }
                    }

                    Color newColor = Color.FromArgb(rMin, gMin, bMin);
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;

        }

        /// <summary>
        /// stworzenie macierzy pikseli z podanego obrazka
        /// </summary>
        /// <param name="original">orginalna bitmapa</param>
        /// <returns>tablica pikselami</returns>
        private int[, ,] stworzMacierzPikseli()
        {
            int[, ,] aColory = new int[oBitmapa.Width, oBitmapa.Height, 3];

            for (int i = 0; i < oBitmapa.Width; i++)
            {
                for (int j = 0; j < oBitmapa.Height; j++)
                {
                    Color originalColor = oBitmapa.GetPixel(i, j); /// pobranie piksela
                    aColory[i, j, 0] = (int)originalColor.R;       /// pobranie skladowej R
                    aColory[i, j, 1] = (int)originalColor.G;       /// pobranie skladowej G
                    aColory[i, j, 2] = (int)originalColor.B;       /// pobranie skladowej B
                }
            }

            return aColory;
        }

        /// <summary>
        /// akcja gdy zostanie zaznaczony radio button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setOryginal_CheckedChanged(object sender, EventArgs e)
        {
            cbJeszczeRaz.Enabled = (setOryginal.Checked) ? false : true;
        }

        private void bgWorkerTworzenieObrazka_DoWork(object sender, DoWorkEventArgs e)
        {

        }
    }
}
