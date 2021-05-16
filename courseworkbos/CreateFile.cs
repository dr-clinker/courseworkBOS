using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;using System.IO;

namespace courseworkbos
{
    public partial class CreateFile : Form
    {
        string disk;
        string filename;
        string file;
        bool overwrite = false;
        List<int> startindexes = new List<int>();
        public CreateFile(string disk, List<int> startindexes)
        {
            InitializeComponent();
            this.disk = disk;
            overwrite = false;
            this.startindexes = startindexes;
        }
        public CreateFile(string disk, List<int> startindexes, string filename, string file)
        {
            InitializeComponent();
            this.disk = disk;
            this.filename = filename;
            this.file = file;
            this.startindexes = startindexes;
            textBox1.Text = filename;
            richTextBox1.Text = file;
            overwrite = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int alllenght = textBox1.Text.Length + richTextBox1.Text.Length;

            if (overwrite == false) //если файл новый
            {
                
                if (disk.Contains(textBox1.Text + "^"))
                {
                    MessageBox.Show("Ошибка, заданное имя файла уже сущестует");
                }
                else
                {
                    string rm = "*";
                    for (int i = 0; i < alllenght; i++)
                    {
                        rm += "*";
                    }
                    rewrite(rm, textBox1.Text, richTextBox1.Text);

                }               
            }
            else //если файл перезаписывается
            {
                rewrite(filename + "^" + file, textBox1.Text, richTextBox1.Text);
            }

        }
        public void rewrite(string rewrstring, string filename, string file)
        {
            int startindex = disk.IndexOf(rewrstring);
            disk = disk.Remove(startindex, rewrstring.Length);
            disk = disk.Insert(startindex, filename + "^" + file);
            File.WriteAllText("harddrive.txt", disk);
            StreamWriter writer = File.AppendText("catalog.txt");
            writer.WriteLine(startindex);
            writer.Close();
            StreamReader reader = new StreamReader("catalog.txt");
            startindexes.Clear();
            while (!reader.EndOfStream)
            {
                startindexes.Add(Int32.Parse(reader.ReadLine()));
            }
            startindexes.Sort();
            foreach(int index in startindexes)
            {
                richTextBox1.Text.Insert(richTextBox1.Text.Length, index + "   ");
            }
            MessageBox.Show("Файл успешно сохранён");
            
        }
    }
}
