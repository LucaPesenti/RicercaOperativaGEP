using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RicercaOperativa
{
    public partial class Form1 : Form
    {
        int app = 0;
        public Form1()
        {
            InitializeComponent();
            Bitmap bmp = new Bitmap(crea.Width, crea.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Rectangle r = new Rectangle(0, 0, bmp.Width, bmp.Height);
                using (LinearGradientBrush br = new LinearGradientBrush(
                                                    r,
                                                    Color.Azure, //DarkRed
                                                    Color.DeepSkyBlue, //MediumVioletRed
                                                    LinearGradientMode.ForwardDiagonal))
                {
                    g.FillRectangle(br, r);
                }
            }
            crea.BackgroundImage = bmp;
            riempi.BackgroundImage = bmp;
            risolvi.BackgroundImage = bmp;

            Bitmap bm = new Bitmap(1650, 750);
            using (Graphics gg = Graphics.FromImage(bm))
            {
                Rectangle rr = new Rectangle(0, 0, bm.Width, bm.Height);
                using (LinearGradientBrush brr = new LinearGradientBrush(
                                                    rr,
                                                    Color.LightSalmon,
                                                    Color.OrangeRed,
                                                    LinearGradientMode.ForwardDiagonal))
                {
                    gg.FillRectangle(brr, rr);
                }
            }
            this.BackgroundImage = bm;
        }

        private void crea_Click(object sender, EventArgs e)
        {
            int colonne;
            int righe;
            try
            {
                colonne = Int32.Parse(tcolonne.Text);
                righe = Int32.Parse(trighe.Text);

                if (colonne < 2 || righe < 2)
                {
                    MessageBox.Show("Il numero di produttori/consumatori deve essere maggiore di 1.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult result = System.Windows.Forms.DialogResult.Yes;
                    if (app != 0)
                        result = MessageBox.Show("Sicuro di voler sovrascrivere la tabella?\nCosì facendo i dati ora visualizzati verranno persi.", "Attenzione", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        app++;
                        tabControl.SelectedTab = PagT;
                        tabella.Rows.Clear();
                        tabella.ColumnCount = colonne + 1;
                        tabella.RowCount = righe + 1;
                        tabella.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);
                        tabella.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);
                        tabella.AllowUserToAddRows = false;
                        tmin.Value = 0;
                        tmax.Value = 0;
                        tPCmin.Value = 0;
                        tPCmax.Value = 0;
                        Smetodo.SelectedIndex = -1;
                        Smetodo.Text = "";
                        lbRisNO.Text = "";
                        gbRisultati.Enabled = false;
                        gbRisultati.Visible = false;

                        for (int i = 0; i < righe; i++)
                        {
                            tabella.Rows[i].HeaderCell.Value = "Produttore " + (i + 1);
                            tabella.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        }
                        tabella.RowHeadersWidth = 125;
                        tabella.Rows[righe].HeaderCell.Value = "Fabbisogno";
                        tabella.Rows[righe].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        for (int i = 0; i < colonne; i++)
                        {
                            tabella.Columns[i].HeaderText = "Consumatore " + (i + 1);
                            tabella.Columns[i].Width = 130;
                            tabella.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            tabella.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }
                        tabella.Columns[colonne].HeaderText = "Produzione";
                        tabella.Columns[colonne].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        tabella.Columns[colonne].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }
                }
                groupBoxRangeCosti.Visible = true;
                groupBoxRangeCosti.Enabled = true;
                groupBoxRangePC.Visible = true;
                groupBoxRangePC.Enabled = true;
                GBMetodo.Visible = true;
                GBMetodo.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Inserire correttamente i dati, utilizzando solo ed esclusivamente numeri.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void riempi_Click(object sender, EventArgs e)
        {
            tabControl.SelectedTab = PagT;
            try
            {
                Smetodo.SelectedIndex = -1;
                Smetodo.Text = "";
                lbRisNO.Text = "";
                gbRisultati.Enabled = false;
                gbRisultati.Visible = false;
                int Cmin = Int32.Parse(tmin.Text);
                int Cmax = Int32.Parse(tmax.Text);
                int PCmin = Int32.Parse(tPCmin.Text);
                int PCmax = Int32.Parse(tPCmax.Text);
                int sommaMin;
                int sommaMax;
                tabella.RowsDefaultCellStyle.Font = new Font("Calibri", 11);

                if (Cmin == 0 && Cmax == 0 && PCmin == 0 && PCmax == 0)
                {
                    tmin.Value = 10;
                    Cmin = 10;
                    tmax.Value = 100;
                    Cmax = 100;
                    tPCmin.Value = 10;
                    PCmin = 10;
                    tPCmax.Value = 100;
                    PCmax = 100;
                }

                if (tabella.Rows.Count < tabella.Columns.Count)
                    sommaMin = PCmin * (tabella.Columns.Count - 1);
                else
                    sommaMin = PCmin * (tabella.Rows.Count - 1);

                if (tabella.Rows.Count > tabella.Columns.Count)
                    sommaMax = PCmax * (tabella.Columns.Count - 1);
                else
                    sommaMax = PCmax * (tabella.Rows.Count - 1);

                if (sommaMin > sommaMax)
                    MessageBox.Show("Range non accettabili.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    Random r = new Random();

                    for (int i = 0; i < tabella.Rows.Count; i++)
                    {
                        for (int j = 0; j < tabella.Columns.Count; j++)
                        {
                            if (i != tabella.Rows.Count - 1 && j != tabella.Columns.Count - 1)
                            {
                                tabella.Rows[i].Cells[j].Value = r.Next(Cmin, Cmax + 1);
                                tabella.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                            }
                        }
                    }

                    int s;
                    int s2;
                    int n;

                    do
                    {
                        s = 0;
                        for (int i = 0; i < tabella.Columns.Count - 1; i++)
                        {
                            n = r.Next(PCmin, PCmax + 1);
                            tabella.Rows[tabella.Rows.Count - 1].Cells[i].Value = n;
                            s += n;
                            tabella.Rows[tabella.Rows.Count - 1].Cells[i].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                            tabella.Rows[tabella.Rows.Count - 1].Cells[i].Style.ForeColor = Color.Blue;
                        }
                        tabella.Rows[tabella.Rows.Count - 1].Cells[tabella.Columns.Count - 1].Value = s;
                        tabella.Rows[tabella.Rows.Count - 1].Cells[tabella.Columns.Count - 1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        tabella.Rows[tabella.Rows.Count - 1].Cells[tabella.Columns.Count - 1].Style.ForeColor = Color.Red;
                    } while (s > sommaMax || s < sommaMin);

                    do
                    {
                        s2 = 0;
                        for (int i = 0; i < tabella.Rows.Count - 2; i++)
                        {
                            n = r.Next(PCmin, PCmax + 1);
                            tabella.Rows[i].Cells[tabella.Columns.Count - 1].Value = n;
                            s2 += n;
                            tabella.Rows[i].Cells[tabella.Columns.Count - 1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                            tabella.Rows[i].Cells[tabella.Columns.Count - 1].Style.ForeColor = Color.Blue;
                        }
                    } while (!((s - s2) > PCmin && (s - s2) < PCmax));
                    tabella.Rows[tabella.Rows.Count - 2].Cells[tabella.Columns.Count - 1].Value = s - s2;
                    tabella.Rows[tabella.Rows.Count - 2].Cells[tabella.Columns.Count - 1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    tabella.Rows[tabella.Rows.Count - 2].Cells[tabella.Columns.Count - 1].Style.ForeColor = Color.Blue;
                }
            }
            catch
            {
                MessageBox.Show("Inserire correttamente i dati.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
        }

        private void risolvi_Click(object sender, EventArgs e)
        {
            if (tabella.Rows.Count > 0)
            {
                if (ControlloCV())
                    MessageBox.Show("Inserire correttamente i dati.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (!ControlloTotali())
                    MessageBox.Show("I totali non corrispondono. Fare attenzione.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    var scelta = Smetodo.Text;
                    if (scelta == "Nord-Ovest")
                    {
                        Nordovest();
                    }
                    else if (scelta == "Minimi costi")
                    {
                        Minimicosti();
                    }
                    else if (scelta == "Vogel")
                    {
                        Vogel();
                    }
                    else if (scelta == "Russel")
                    {
                        Russel();
                    }
                    else if (scelta == "Tutti")
                    {
                        MessageBox.Show("Supportato solo il metodo Nord-Ovest.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Nordovest();
                        //Minimicosti();
                        //Vogel();
                        //Russel();
                    }
                    else
                    {
                        MessageBox.Show("Selezionare un metodo.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Creare una tabella prima.", "Attenzione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool ControlloCV()
        {
            string numString;
            int numero = 0;
            bool canConvert;

            for (int i = 0; i < tabella.Columns.Count; i++)
            {
                for (int j = 0; j < tabella.Rows.Count; j++)
                {
                    if (i != tabella.Rows.Count - 1 || j != tabella.Columns.Count - 1)
                    {
                        if (tabella.Rows[j].Cells[i].Value == null)
                            return true;

                        numString = tabella.Rows[j].Cells[i].Value.ToString();
                        canConvert = Int32.TryParse(numString, out numero);
                        if (!canConvert)
                            return true;
                    }
                }
            }
            return false;
        }

        public bool ControlloTotali()
        {
            int sommar = 0;
            for (int i = 0; i < tabella.Columns.Count - 1; i++)
            {
                sommar += Int32.Parse(tabella.Rows[tabella.Rows.Count - 1].Cells[i].Value.ToString());
            }
            int sommac = 0;
            for (int i = 0; i < tabella.Rows.Count - 1; i++)
            {
                sommac += Int32.Parse(tabella.Rows[i].Cells[tabella.Columns.Count - 1].Value.ToString());
            }
            if (sommar == sommac)
                return true;
            else
                return false;
        }

        private void Nordovest()
        {
            tabControl.SelectedTab = PagNO;
            tabellaNO.Rows.Clear();
            tabellaNO.ColumnCount = tabella.ColumnCount;
            tabellaNO.RowCount = tabella.RowCount;
            tabellaNO.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);
            tabellaNO.RowHeadersDefaultCellStyle.Font = new Font("Calibri", 10, FontStyle.Bold);
            tabellaNO.RowsDefaultCellStyle.Font = new Font("Calibri", 11);

            for (int i = 0; i < tabellaNO.RowCount; i++)
            {
                for (int j = 0; j < tabellaNO.ColumnCount; j++)
                {
                    tabellaNO.Rows[i].Cells[j].Value = tabella.Rows[i].Cells[j].Value;
                    tabellaNO.Rows[i].Cells[j].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            for (int i = 0; i < tabellaNO.RowCount; i++)
            {
                tabellaNO.Rows[i].HeaderCell.Value = tabella.Rows[i].HeaderCell.Value;
                tabellaNO.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            tabellaNO.RowHeadersWidth = 125;
            tabellaNO.Rows[tabellaNO.RowCount - 1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < tabellaNO.ColumnCount; i++)
            {
                tabellaNO.Columns[i].HeaderCell.Value = tabella.Columns[i].HeaderCell.Value;
                tabellaNO.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                tabellaNO.Columns[i].Width = 130;
                tabellaNO.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            int tot = 0;
            int s = 0;
            int n = 0;
            int colonne = tabellaNO.Columns.Count - 1;
            int righe = tabellaNO.Rows.Count - 1;
            bool appC = true;
            bool appR = true;

            while (tabellaNO.ColumnCount > 1 || tabellaNO.RowCount > 1)
            {
                int cunita = Int32.Parse(tabellaNO.Rows[0].Cells[0].Value.ToString());
                int prod = Int32.Parse(tabellaNO.Rows[0].Cells[tabellaNO.ColumnCount - 1].Value.ToString());
                int fab = Int32.Parse(tabellaNO.Rows[tabellaNO.RowCount - 1].Cells[0].Value.ToString());
                if (prod > fab)
                {
                    if (appC)
                    {
                        colonne = Int32.Parse(tabellaNO.Columns.Count.ToString()) - 1;
                        appC = false;
                    }
                    tot += fab * cunita;
                    prod -= fab;
                    tabellaNO.Rows[0].Cells[tabellaNO.Columns.Count - 1].Value = prod;
                    tabellaNO.Rows[0].Cells[tabellaNO.Columns.Count - 1].Style.ForeColor = Color.Red;
                    for (int i = 0; i < tabellaNO.Columns.Count; i++)
                    {
                        tabellaNO.Rows[0].Cells[i].Style.BackColor = Color.Yellow;
                    }
                    Thread.Sleep(900);
                    tabellaNO.Columns.RemoveAt(0);
                    colonne--;

                    for (int i = 0; i < colonne; i++)
                    {
                        n = Int32.Parse(tabellaNO.Rows[righe].Cells[i].Value.ToString());
                        s += n;
                    }
                    if (righe != 0 && colonne != 0)
                    {
                        tabellaNO.Rows[righe].Cells[colonne].Value = s;
                    }

                    n = 0;
                    s = 0;
                    tabellaNO.Refresh();
                }
                else
                {
                    if (appR)
                    {
                        righe = Int32.Parse(tabellaNO.Rows.Count.ToString()) - 1;
                        appR = false;
                    }
                    tot += prod * cunita;
                    fab -= prod;
                    tabellaNO.Rows[tabellaNO.Rows.Count - 1].Cells[0].Value = fab;
                    tabellaNO.Rows[tabellaNO.Rows.Count - 1].Cells[0].Style.ForeColor = Color.Red;
                    for (int i = 0; i < tabellaNO.Rows.Count; i++)
                    {
                        tabellaNO.Rows[i].Cells[0].Style.BackColor = Color.Yellow;
                    }
                    Thread.Sleep(900);
                    tabellaNO.Rows.RemoveAt(0);
                    righe--;

                    for (int i = 0; i < righe; i++)
                    {
                        n = Int32.Parse(tabellaNO.Rows[i].Cells[colonne].Value.ToString());
                        s += n;
                    }
                    if (righe != 0 && colonne != 0)
                    {
                        tabellaNO.Rows[righe].Cells[colonne].Value = s;
                    }

                    n = 0;
                    s = 0;
                    tabellaNO.Refresh();
                }
            }
            MessageBox.Show("Il costo totale del trasporto è di: " + tot.ToString(), "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
            gbRisultati.Enabled = true;
            gbRisultati.Visible = true;
            lbRisNO.Text = tot.ToString();
        }

        private void Minimicosti()
        {
            MessageBox.Show("Metodo dei Minimi costi non ancora sviluppato.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Vogel()
        {
            MessageBox.Show("Metodo Vogel non ancora sviluppato.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Russel()
        {
            MessageBox.Show("Metodo Russel non ancora sviluppato.", "Informazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}