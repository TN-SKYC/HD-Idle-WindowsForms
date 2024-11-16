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

            // Collect all associated drive letters (partitions)
            string driveLetters = "";

            ManagementObjectSearcher partitionSearcher = new ManagementObjectSearcher(
                $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID='{deviceID}'}} WHERE AssocClass=Win32_DiskDriveToDiskPartition");

            foreach (ManagementObject partition in partitionSearcher.Get())
            {
                string partitionDeviceID = partition["DeviceID"]?.ToString() ?? "Unknown Partition Device ID";

                // Use the partition device ID to find the associated logical drives (letters)
                ManagementObjectSearcher logicalSearcher = new ManagementObjectSearcher(
                    $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partitionDeviceID}'}} WHERE AssocClass=Win32_LogicalDiskToPartition");

                foreach (ManagementObject logical in logicalSearcher.Get())
                {
                    string driveLetter = logical["DeviceID"]?.ToString();  // This is the drive letter (e.g., C:, D:)
                    if (!string.IsNullOrEmpty(driveLetter))
                    {
                        if (!string.IsNullOrEmpty(driveLetters))
                            driveLetters += ", ";
                        driveLetters += driveLetter;
                    }
                }
            }

            // Display disk model along with its drive letters in the ComboBox
            if (!string.IsNullOrEmpty(driveLetters))
                cmbDrives.Items.Add($"{model} - ({driveLetters})");
            else
                cmbDrives.Items.Add(model);

            // Log the disk details (optional)
            Console.WriteLine($"Disk: {model}, Device ID: {deviceID}, Drive Letters: {driveLetters}");
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