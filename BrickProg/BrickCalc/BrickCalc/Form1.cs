using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrickCalc
{
    public partial class Form1 : Form
    {
        //A brick has the dimensions of 215mm long, 102.5 high
        //A block has the dimensions of 440mm long, 215 high
        //A mortar mix is 1 part cement to 5 parts sand
        //A mortar gap is 10mm
        //50kg of cement with 250kg of sand = 300kg dry which will lay 300 bricks or 53 blocks
        //If wall is double thickness then you also need wall ties – the use is 2.5 wall ties per 1 m 2

        //setting all variables
        //bblength / height is set before you choose between brick or block allowing for conversion in calculate();
        double bblength = 0;
        double bbheight = 0;
        double length = 0;
        double height = 0;
        double area = 0;
        double walltiefreq = 0;
        bool walltiesneeded = false;
        //i could of used a bool but in this case its to optimise a small display change 
        string brickorblock = "";
        bool laborbool = false;
        double time = 0;
        double cement = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void CHKbrick_CheckedChanged(object sender, EventArgs e)
        {
            //basically setting if its checked to the opposite of the other checkbox 
            //more optimised than an if or switch statement 
            CHKblock.Checked = !CHKbrick.Checked;
            bblength = 215;
            bbheight = 102.5;
            brickorblock = "bricks!";
            //just now realising i spelt 'calculate' wrong 
            caculate();
        }

        private void CHKblock_CheckedChanged(object sender, EventArgs e)
        {
            CHKbrick.Checked = !CHKblock.Checked;
            bblength = 440;
            bbheight = 215;
            brickorblock = "blocks!";
            caculate();
        }

        private void TXTlength_TextChanged(object sender, EventArgs e)
        {
            //i swear im not dislexic 
            caculate();
        }

        private void TXTheight_TextChanged(object sender, EventArgs e)
        {
            
            caculate();
        }

        private void CHKsingle_CheckedChanged(object sender, EventArgs e)
        {
            CHKdouble.Checked = !CHKsingle.Checked;
            walltiesneeded = false;
            caculate();
        }

        private void CHKdouble_CheckedChanged(object sender, EventArgs e)
        {
            CHKsingle.Checked = !CHKdouble.Checked;
            walltiesneeded = true;
            caculate();
        }

        void caculate()
        {
            //try ans catch for a better optimised error stopping 'thing?'
            try
            {
                //conversion of some variables 
                //a better way of soing with this otherwise the variables get messed with and i cba with that
                height = Convert.ToDouble(TXTheight.Text);
                length = Convert.ToDouble(TXTlength.Text);

                //making it mm as the bricks are measured in mm
                height = height * 1000;
                length = length * 1000;

                //calculates how many bricks are needed 
                //local variable 
                double bblayer = (Math.Ceiling(length / bblength));
                double bblayerheight = (Math.Ceiling(height / bbheight));

                //easy way of calculating the bricks needed 
                double materialsneeded = bblayer * bblayerheight;

                if (walltiesneeded)
                {
                    materialsneeded = materialsneeded * 2;
                }
                TXTmaterialfreq.Text = Convert.ToString("You need.... " + materialsneeded + " " + (brickorblock));
                

                if (brickorblock == "blocks!")
                {
                    cement = materialsneeded / 53;
                }
                else
                {
                    cement = materialsneeded / 300;
                }

                cement = cement / 25;

                TXTcementfreq.Text = Convert.ToString("You need.... " + Math.Round(cement) + " Bags of cement");

                double sand = Math.Round(cement * 5);

                TXTsandfreq.Text = Convert.ToString("You need...." + sand + " Bags of sand");
                if (walltiesneeded)
                {
                    area = length * height;
                    area = area / 1000000;
                    walltiefreq = area / 2.5;                    
                }
                TXTwalltiefreq.Text = Convert.ToString("You need.... " + walltiefreq + " Wall ties");

                //500 with a laborer 300 without 
                if (laborbool)
                {
                    time = (materialsneeded / 500) * 8;
                    TXTtime.Text = Convert.ToString("It will take.... " + Math.Round(time) + " Hours");
                }
                else
                {
                    time = (materialsneeded / 300) * 8;
                    TXTtime.Text = Convert.ToString("It will take.... " + Math.Round(time) + " Hours");
                }

                double matcost = (Math.Round(materialsneeded / 390)) * 256;
                double cementcost = ((materialsneeded * 5.66) / 25) * 2.89;
                double walltiecost = (walltiefreq / 20) * 11.02;
                double buildercost = 0;
                if (laborbool)
                {
                    buildercost = 50 * time;
                }
                else
                {
                    buildercost = 35 * time;
                }

                double totalcost = matcost + cementcost + walltiecost + buildercost;

                TXTmoney.Text = Convert.ToString("It will cost.... " + Math.Round(totalcost) + " Pounds");




            }

            catch
            {
                TXTsandfreq.Text = ("");
                TXTmaterialfreq.Text = ("");
                TXTtime.Text = ("");
                TXTwalltiefreq.Text = ("");
                return;
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CHKlabourer_CheckedChanged(object sender, EventArgs e)
        {
            if (CHKlabourer.Checked == true)
            {
                laborbool = true;
                caculate();
            }
            else
            {
                laborbool = false;
                caculate();
            }
        }
    }
}
