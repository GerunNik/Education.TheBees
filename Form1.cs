using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeApp
{
    public partial class Form1 : Form
    {
        public static World World;
        public Form1()
        {
            InitializeComponent();
            World = new World();
            BeehiveSim Simulator = new BeehiveSim();
            Simulator.Show();
            
        }
    }
}
