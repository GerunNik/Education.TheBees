using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeApp
{
    public partial class BeehiveSim : Form
    {
        int passedFrames = 0;
        public BeehiveSim()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void RefreshStatistics()
        {
            txt_Bee.Text = Form1.World.Bees.Count.ToString();
            txt_Flower.Text = Form1.World.Flowers.Count.ToString();
            txt_Honey.Text = Form1.World.Nest.Honey.ToString();

            double allNectar = 0;
            foreach (var item in Form1.World.Flowers)
            {
                allNectar += item.Nectar;
            }

            txt_Nectar.Text = allNectar.ToString();
            txt_Frames.Text = passedFrames.ToString();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            switch (toolStripLabel1.Text)
            {
                case "Stop Simulation":
                    toolStripLabel1.Text = "Start Simulation";
                    toolStripStatusLabel1.Text = "Simulation has stopped";
                    break;
                case "Start Simulation":
                    toolStripLabel1.Text = "Stop Simulation";
                    toolStripStatusLabel1.Text = "Simulation has started";
                    break;
            }
        }

        private void FillBeeText()
        {
                int useless = 0;
                int flyingToFlower = 0;
                int collectingNectar = 0;
                int flyingToNest = 0;
                int producingHoney = 0;
                int inRetirement = 0;

            txt_BeeBox.Text = string.Empty;

            foreach (var item in Form1.World.Bees)
            {
                switch (item.State)
                {
                    case BeeStatus.Useless:
                        useless++;
                        break;
                    case BeeStatus.FlyingToFlower:
                        flyingToFlower++;
                        break;
                    case BeeStatus.CollectingNectar:
                        collectingNectar++;
                        break;
                    case BeeStatus.FlyingToNest:
                        flyingToNest++;
                        break;
                    case BeeStatus.ProducingHoney:
                        producingHoney++;
                        break;
                    case BeeStatus.InRetirement:
                        inRetirement++;
                        break;
                }
            }
            if (useless > 0)
            {
                txt_BeeBox.Text += "Useless: " + useless + " Bees\n";
            }
            if (flyingToFlower > 0)
            {
                txt_BeeBox.Text += "FlyingToFlower: " + flyingToFlower + " Bees\n";
            }
            if (collectingNectar > 0)
            {
                txt_BeeBox.Text += "CollectingNectar: " + collectingNectar + " Bees\n";
            }
            if (flyingToNest > 0)
            {
                txt_BeeBox.Text += "FlyingToNest: " + flyingToNest + " Bees\n";
            }
            if (producingHoney > 0)
            {
                txt_BeeBox.Text += "ProducingHoney: " + producingHoney + " Bees\n";
            }
            if (inRetirement > 0)
            {
                txt_BeeBox.Text += "InRetirement: " + inRetirement + " Bees\n";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (toolStripLabel1.Text == "Stop Simulation")
            {
                Form1.World.Walk(new Random());
                RefreshStatistics();
                FillBeeText();
                passedFrames++;
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            World newWorld = new World();
            Form1.World = newWorld;
            passedFrames = 0;
            RefreshStatistics();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (toolStripLabel1.Text == "Stop Simulation")
            {
                timer1.Stop();
            }

            IFormatter formatter = new BinaryFormatter();
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);

                formatter.Serialize(stream, Form1.World);
                stream.Close();
            }

            if (toolStripLabel1.Text == "Stop Simulation")
            {
                timer1.Start();
            }
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            if (toolStripLabel1.Text == "Stop Simulation")
            {
                timer1.Stop();
            }
            IFormatter formatter = new BinaryFormatter();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                Form1.World = (World)formatter.Deserialize(stream);
                stream.Close();
                RefreshStatistics();
                FillBeeText();
            }

            if (toolStripLabel1.Text == "Stop Simulation")
            {
                timer1.Start();
            }
        }
    }
}
