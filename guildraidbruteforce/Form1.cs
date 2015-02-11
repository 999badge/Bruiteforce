using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace guildraidbruteforce
{
    

    public partial class Form1 : Form
    {

        private Bitmap myCanvas;
        private Random rand = new Random();
        List<City> citylist = new List<City>();
        List<string> combolist = new List<string>();
        guildraidbruteforce.Facet.Combinatorics.Permutations<int> permutations;
        private double lowestdist = 99999.9;
        private double temp = 0;
        private string bestcombo;
        private int numCities;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SolidBrush myBrush = new SolidBrush(Color.Black);
            myCanvas = new Bitmap(400, 400,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(myCanvas);
            g.Clear(Color.White);
            //random points
            
        }

        private double distancebetween(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(myCanvas, 0, 0, myCanvas.Width, myCanvas.Height);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(myCanvas);
            for (int i = 0; i < int.Parse(numcityTextbox.Text); i++)
            {
                citylist.Add(new City(new Point(rand.Next(0, 401), rand.Next(0, 401)), i.ToString()));
                citylist[i].Draw(g);
            }
            this.Refresh();
            numCities = int.Parse(numcityTextbox.Text);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int[] inputSet = new int[numCities];

            for (int i = 0; i < numCities; i++)
            {
                inputSet[i] = i;
            }

            permutations = new guildraidbruteforce.Facet.Combinatorics.Permutations<int>(inputSet);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            outputLabel.Text = "done";
            
            foreach (IList<int> p in permutations)
            {
                //richTextBox1.Text += String.Format("{{{0} {1} {2} {3} {4} {5} {6} {0}}}", p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[0]) + "\n";
      

                temp = distancebetween(citylist[p[0]].getLocation(), citylist[p[1]].getLocation())
                    + distancebetween(citylist[p[1]].getLocation(), citylist[p[2]].getLocation())
                    + distancebetween(citylist[p[2]].getLocation(), citylist[p[3]].getLocation())
                    + distancebetween(citylist[p[3]].getLocation(), citylist[p[4]].getLocation())
                    + distancebetween(citylist[p[4]].getLocation(), citylist[p[5]].getLocation())
                    + distancebetween(citylist[p[5]].getLocation(), citylist[p[6]].getLocation())
                    + distancebetween(citylist[p[6]].getLocation(), citylist[p[0]].getLocation());

                if (temp < lowestdist)
                {
                    lowestdist = temp;
                    bestcombo = String.Format("{{{0} {1} {2} {3} {4} {5} {6} {0}}}", p[0], p[1], p[2], p[3], p[4], p[5], p[6], p[0]);
                }
            }
            outputLabel.Text = lowestdist.ToString();
            comboLabel.Text = bestcombo;
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(myCanvas);
            g.Clear(Color.White);
            this.Refresh();
            citylist.Clear();
            outputLabel.Text = "";
            comboLabel.Text = "";
            numcityTextbox.Text = "";

        }


    }
    public class City
    {
        Point location;
        string index;

        public City()
        {
            location = new Point(0, 0);
        }

        public City(Point p, string n)
        {
            location = p;
            index = n;
        }
        public void set_location(int x, int y)
        {
            location.X = x;
            location.Y = y;
        }

        public void Draw(Graphics g)
        {
            //Font myFont = new Font("Courier", FontStyle.Regular);
            Font myFont = new Font("Courier", 12);
            SolidBrush myBrush = new SolidBrush(Color.Red);
            g.FillEllipse(new SolidBrush(Color.Blue), location.X - 20 / 2, location.Y - 20 / 2, 20, 20);
            g.DrawString(index, myFont, myBrush, new PointF(location.X, location.Y));
        }

        public Point getLocation()
        {
            return location;
        }
    }


}