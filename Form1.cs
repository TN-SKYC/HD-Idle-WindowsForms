using System;
using System.Windows.Forms;

namespace HdIdleApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Populate cmbDrives with available drives
            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {
                cmbDrives.Items.Add(drive.Name);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtOutput.AppendText("Starting HD idle...\n");
            // Add code to start the idle process
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            txtOutput.AppendText("Stopping HD idle...\n");
            // Add code to stop the idle process
        }
    }
}
