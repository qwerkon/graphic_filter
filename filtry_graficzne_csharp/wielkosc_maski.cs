using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace filtry_graficzne_csharp
{
    public partial class wielkosc_maski : Form
    {
        public fnPrzekazRozmiarMaski rozmiarMaski;
        public string sType;
        public wielkosc_maski()
        {
            InitializeComponent();
        }

        private void btWykonaj_Click(object sender, EventArgs e)
        {
            rozmiarMaski(tbWspolczynnikQ.Text, sType);
            this.Close();
        }
    }
}
