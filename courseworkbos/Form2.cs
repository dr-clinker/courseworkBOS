using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace courseworkbos
{
    public partial class Form2 : Form
    {
        string disk;
        List<int> startindexes = new List<int>();

        public Form2()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateFile createFile = new CreateFile(disk, startindexes);
            ActiveForm.Hide();
            createFile.Show();
        }

        private void checkfiles_button_Click(object sender, EventArgs e)
        {
            checkfiles();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            string selectedfilename = listBox1.SelectedItem.ToString();
            int index = disk.IndexOf(selectedfilename);
            int lastindex = 0;
            for(int i = 0; i<startindexes.Count; i++)
            {
                if(startindexes.ElementAt(i) > index)
                {
                    lastindex = startindexes.ElementAt(i);
                    break;
                }
            }
           
            string file = disk.Substring(index, lastindex - index);
            file = file.Substring(file.IndexOf('^') + 1).Replace("*", "");
            CreateFile createFile = new CreateFile(disk,startindexes, selectedfilename, file);
            ActiveForm.Hide();
            createFile.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            StreamReader streamReader = new StreamReader("catalog.txt");
            while (!streamReader.EndOfStream)
            {
                startindexes.Add(int.Parse(streamReader.ReadLine()));
            }
            streamReader.Close();
            StreamReader streamReader1 = new StreamReader("harddrive.txt");
            disk = streamReader1.ReadToEnd();
            streamReader1.Close();
            checkfiles();
        }

        public void checkfiles()
        {
            listBox1.Items.Clear();
            foreach (int index in startindexes)
            {
                listBox1.Items.Add(disk.Substring(index, disk.Substring(index).IndexOf('^')));
            }
        }
    }
}