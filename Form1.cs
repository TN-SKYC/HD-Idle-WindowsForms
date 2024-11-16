using System;
using System.Management;
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
    // Clear previous items in the ComboBox
    cmbDrives.Items.Clear();

    try
    {
        // Create a searcher for physical disks
        ManagementObjectSearcher diskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

        foreach (ManagementObject disk in diskSearcher.Get())
        {
            // Retrieve disk model and device ID
            string model = disk["Model"]?.ToString() ?? "Unknown Model";
            string deviceID = disk["DeviceID"]?.ToString() ?? "Unknown Device ID";

            // Add the disk model to the ComboBox
            cmbDrives.Items.Add(model);

            // Log the disk details (optional)
            Console.WriteLine($"Disk: {model}, Device ID: {deviceID}");

            // Search for associated partitions
            ManagementObjectSearcher partitionSearcher = new ManagementObjectSearcher(
                $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{deviceID}'}} WHERE AssocClass=Win32_DiskDriveToDiskPartition");

            foreach (ManagementObject partition in partitionSearcher.Get())
            {
                string partitionDeviceID = partition["DeviceID"]?.ToString() ?? "Unknown Partition Device ID";
                string partitionType = partition["Type"]?.ToString() ?? "Unknown Partition Type";

                // Optionally, you can log the partition info or use it in the UI
                Console.WriteLine($"Partition Device ID: {partitionDeviceID}, Type: {partitionType}");

                // Add partition details as a sub-item under the corresponding disk (if you want to show partitions as well)
                cmbDrives.Items.Add($"   Partition: {partitionDeviceID}, Type: {partitionType}");
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error detecting drives: " + ex.Message);
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
