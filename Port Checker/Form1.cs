using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Port_Checker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Dictionary<int, string> portNames = GetPortNames();

            GetServiceName(21);
        }

        static string[] GetServiceName(int port)
        {
            string[] port_info = { port.ToString(),"",""};
            try
            {
                string port_ = port.ToString() + "/";
                string[] ports_list = File.ReadAllLines("C:\\windows\\system32\\drivers\\etc\\services");
                foreach(var _port in ports_list)
                {
                    if (_port.Contains(port_))
                    {
                        port_info[1] = _port.Split(' ')[0];
                        port_info[2] = _port.Split('#')[1];
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
            return port_info;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            add_range_ports();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            add_range_ports();
            if(!richTextBox1.Text.Contains(","))
            {
                MessageBox.Show("No Ports To Check", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                add_range_ports();
                //MessageBox.Show(isalive("fngmf4.ddns.net", 5552).ToString());
                int checks = richTextBox1.Text.Split(',').Length;
                foreach(var port in richTextBox1.Text.Split(','))
                {
                    new Thread(() =>
                    {
                        try
                        {
                            using (TcpClient client = new TcpClient())
                            {
                                client.Connect(hostname_input.Text, Int32.Parse(port));
                            }
                            listView1.Invoke(new MethodInvoker(delegate
                            {
                                string[] port_info = GetServiceName(Int32.Parse(port));
                                

                                if (!exist_list(listView1,port_info[0],0))
                                {
                                    ListViewItem list = new ListViewItem(port_info[0]);
                                    list.SubItems.Add(port_info[1]);
                                    list.SubItems.Add(port_info[2]);
                                    listView1.Items.Add(list);
                                }
                               
                            }));
                            checks--;
                        }
                        catch(Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            checks--;
                        }
                    })
                    { IsBackground = true }.Start();
                    
                }
                new Thread(() =>
                {
                    while (true)
                    {
                        if (checks == 0)
                        {
                            MessageBox.Show("Checks Finished !", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        }
                    }
                })
                { IsBackground = true }.Start();
                
                

            }
        }
        public void add_range_ports()
        {
            if(end_port.Value == 0 && start_port.Value == 0)
            {
                //MessageBox.Show("Invalid 'From' And 'To' Ports Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }else if(start_port.Value >= end_port.Value)
            {
                MessageBox.Show("Invalid 'From' And 'To' Ports Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                for(int i = Int32.Parse(start_port.Value.ToString()); i < end_port.Value+1; i++)
                {
                    richTextBox1.Text += i.ToString()+",";
                }
                richTextBox1.Text = richTextBox1.Text.Substring(0, richTextBox1.Text.Length - 1);
            }
        }
        public bool isalive(string ip,int port)
        {
            bool result = false;
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ip, port);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
                
            return result;
            
        }
        public bool exist_list(ListView list,string text,int position)
        {
            bool got_it = false;
            for(var i = 0; i < list.Items.Count; i++)
            {
                if(list.Items[i].SubItems[position].Text == text)
                {
                    got_it = true;
                }
            }
            return got_it;
        }

    }
}
