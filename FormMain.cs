using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace SerialConnect
{
    public partial class FormMain : Form
    {
        static string StatusText;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();

            foreach(string port in ports)
            {
                comboBoxSerialPort.Items.Add(port);
            }

            comboBoxSerialSpeed.SelectedIndex = 11;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if(comboBoxSerialPort.Text.Trim().Length == 0)
            {
                MessageBox.Show("Select a port and try again", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if(comboBoxSerialSpeed.Text.Trim().Length == 0)
            {
                MessageBox.Show("Select port connect speed and try again", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            serialPort1.PortName = comboBoxSerialPort.Text.Trim();
            serialPort1.BaudRate = Convert.ToInt32(comboBoxSerialSpeed.Text);
            serialPort1.Open();

            if(serialPort1.IsOpen)
            {
                buttonConnect.Enabled = false;
                buttonDisconnect.Enabled = true;
            }
        }

        private void WriteStatusLine(string text)
        {
            textBoxStatus.AppendText(String.Format("{0}\r\n", text));
        }

        private void DisplayThreadText(object sender, EventArgs e)
        {
            WriteStatusLine(StatusText);
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            StatusText = serialPort1.ReadExisting();
            Invoke(new EventHandler(DisplayThreadText));
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonDisconnect.Enabled = false;
                buttonConnect.Enabled = true;
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen && textBoxDataSend.Text.Length > 0)
            {
                serialPort1.WriteLine(textBoxDataSend.Text);
            }
        }
    }
}
