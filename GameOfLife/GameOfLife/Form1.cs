using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        int visina = 10, sirina = 10;
        int[,] stanje = new int[65, 35], stanje2 = new int[65, 35];
        bool ShowGrid = true;
        int ExtraPixel = 1;

        Color CPozadina = Color.FromArgb(112, 151, 117);
        Color CSpace = Color.FromArgb(242, 248, 242);
        Color CBlock = Color.FromArgb(17, 29, 19);

        public Form1()
        {
            InitializeComponent();
        }

        void Nacrtaj()
        {
            //crtanje mreze
            Graphics g = pictureBox1.CreateGraphics();
            Pen Olovka = new Pen(Color.Black);
            SolidBrush Cetka = new SolidBrush(CSpace);
            g.FillRectangle(Cetka, 0, 0, pictureBox1.Width, pictureBox1.Height);
            if (ShowGrid == true)
            {
                g.FillRectangle(Cetka, 0, 0, pictureBox1.Width, pictureBox1.Height);
                for (int i = 0; i <= pictureBox1.Height + 1; i += visina)
                    g.DrawLine(Olovka, 0, i, pictureBox1.Width, i);
                for (int i = 0; i <= pictureBox1.Width + 1; i += sirina)
                    g.DrawLine(Olovka, i, 0, i, pictureBox1.Height);
            }
            //ipunjavanje
            Cetka.Color=CBlock;
            for (int i = 0; i < 65; i++)
                for (int j = 0; j < 35; j++) if (stanje[i, j] == 1) g.FillRectangle(Cetka, i * sirina + ExtraPixel, j * visina + ExtraPixel, sirina - ExtraPixel, visina - ExtraPixel);
        }
        int PrebrojSusjede(int x,int y)
        {
            int zbir = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i < 0 || x + i > 64 || y + j < 0 || y + j > 34) continue;
                    zbir += stanje[x + i,y + j];
                }
            }
            return zbir - stanje[x,y];
        }
        void AzurirajStanje()
        {
            Graphics g = pictureBox1.CreateGraphics();
            SolidBrush Cetka = new SolidBrush(CBlock);

            for(int i=0;i<65;i++)
            {
                for(int j=0;j<35;j++)
                {
                    int BrojSusjeda = PrebrojSusjede(i,j);
                    if(BrojSusjeda==3 || (stanje[i,j]==1 && BrojSusjeda==2))
                    {
                        stanje2[i, j] = 1; 
                        Cetka.Color=CBlock;
                            g.FillRectangle(Cetka, i * sirina + ExtraPixel, j * visina + ExtraPixel, sirina - ExtraPixel, visina - ExtraPixel);
                    }
                    if (stanje[i, j] == 1 && (BrojSusjeda <2 || BrojSusjeda>3))
                    {
                        stanje2[i, j] = 0;
                        Cetka.Color = CSpace;
                            g.FillRectangle(Cetka, i * sirina + ExtraPixel, j * visina + ExtraPixel, sirina - ExtraPixel, visina - ExtraPixel);
                    }
                }
            }
            //
            for (int i = 0; i < 65; i++)
                for (int j = 0; j < 35; j++) stanje[i, j] = stanje2[i, j];
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int X = e.X - e.X % visina;
            int Y = e.Y - e.Y % sirina;
            if (X >= pictureBox1.Width - 1 || Y >= pictureBox1.Height - 1) return;

            Graphics g = pictureBox1.CreateGraphics();
            SolidBrush Cetka = new SolidBrush(CSpace);

            if (stanje[(X / visina), (Y / sirina)] == 1 || stanje2[(X / visina), (Y / sirina)] == 1)
            {
                Cetka.Color = CSpace;
                stanje2[(X / visina), (Y / sirina)] = 0;
                stanje[(X / visina), (Y / sirina)] = 0;
            }
            else
            {
                Cetka.Color = CBlock;
                stanje2[(X / visina), (Y / sirina)] = 1;
                stanje[(X / visina), (Y / sirina)] = 1;
            }
            g.FillRectangle(Cetka, X + ExtraPixel, Y + ExtraPixel, visina - ExtraPixel, visina - ExtraPixel);
        }

        private void Start_button_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            Randomize_button.Enabled = !Randomize_button.Enabled;
            Reset_button.Enabled = !Reset_button.Enabled;
            comboBox1.Enabled = !comboBox1.Enabled;
        }

        private void Reset_button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 65; i++)
                for (int j = 0; j < 35; j++)
                {
                    stanje[i, j] = 0;
                    stanje2[i, j] = 0;
                }
            Nacrtaj();
        }
        private void Randomize_button_Click(object sender, EventArgs e)
        {
            Random rnd=new Random();
            for(int i=0;i<65;i++)
            {
                for(int j=0;j<35;j++)
                {
                  if(rnd.Next(100)<20)  stanje[i, j] = 1;
                }
            }
            Nacrtaj();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            pictureBox1.Width = 651;
            pictureBox1.Height = 351;
            this.BackColor = CPozadina;

            for (int i = 0; i < 65; i++)
                for (int j = 0; j < 35; j++)
                    stanje[i, j] = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AzurirajStanje();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ShowGrid = !ShowGrid;
            if (ShowGrid == true) ExtraPixel = 1;
            else ExtraPixel = 0;
            Nacrtaj();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reset_button_Click(this, null);
            if(comboBox1.SelectedIndex==0)//gospers
            {
                stanje[1, 6] = 1;
                stanje[1, 7] = 1;
                stanje[2, 6] = 1;
                stanje[2, 7] = 1;
                //
                stanje[13, 4] = 1;
                stanje[14, 4] = 1;
                stanje[12, 5] = 1;
                stanje[11, 6] = 1;
                stanje[11, 7] = 1;
                stanje[11, 8] = 1;
                stanje[12, 9] = 1;
                stanje[13, 10] = 1;
                stanje[14, 10] = 1;
                //
                stanje[15, 7] = 1;
                stanje[17, 6] = 1;
                stanje[17, 7] = 1;
                stanje[17, 8] = 1;
                stanje[18, 7] = 1;
                //
                stanje[16, 5] = 1;
                stanje[16, 9] = 1;
                for (int i = 21; i <= 22; i++)//21,4
                    for (int j = 4; j <= 6; j++)
                        stanje[i, j] = 1;
                //
                stanje[23, 3] = 1;
                stanje[23, 7] = 1;
                //
                stanje[25, 3] = 1;
                stanje[25, 2] = 1;
                stanje[25, 7] = 1;
                stanje[25, 8] = 1;
                //
                for (int i = 35; i <= 36; i++)//35,4
                    for (int j = 4; j <= 5; j++)
                        stanje[i, j] = 1;
            }
            if(comboBox1.SelectedIndex==1)
            {
                for (int i = 6; i <= 7; i++)//1,1
                    for (int j = 6; j <= 7; j++)
                        stanje[i, j] = 1;
                for (int i = 13; i <= 14; i++)//8,1
                    for (int j = 6; j <= 7; j++)
                        stanje[i, j] = 1;
                for (int i = 10; i <= 11; i++)//5,4
                    for (int j = 9; j <= 10; j++)
                        stanje[i, j] = 1;
                //22,10
                stanje[28, 15] = 1;
                stanje[29, 15] = 1;

                stanje[27, 16] = 1;
                stanje[27, 17] = 1;
                stanje[27, 18] = 1;

                stanje[28, 18] = 1;
                stanje[29, 18] = 1;

                stanje[31, 15] = 1;
                stanje[32, 15] = 1;
                stanje[33, 16] = 1;
                stanje[34, 17] = 1;
                stanje[33, 18] = 1;
                stanje[32, 19] = 1;
                for (int i = 37; i <= 38; i++)//32,12
                    for (int j = 17; j <= 18; j++)
                        stanje[i, j] = 1;
            }
            Nacrtaj();
        }
    }
}
