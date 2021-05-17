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

namespace courseworkbos
{
    public partial class CreateFile : Form
    {
        string disk;
        string filename;
        string file;
        bool overwrite = false;
        bool seek = false;
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
        public CreateFile(string disk, List<int> startindexes, bool seek)
        {
            InitializeComponent();
            this.disk = disk;
            overwrite = false;
            this.startindexes = startindexes;
            this.seek = seek;
            textBox2.Visible = true;
            label1.Visible = true;
            MessageBox.Show("В появившемся окне введите номер позиции, с которой нужно записать файл");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (seek)
            {
                int newindex;
                if(Int32.TryParse(textBox2.Text, out newindex))
                {
                    newindex = Int32.Parse(textBox2.Text);
                }
                else
                {
                    MessageBox.Show("Введите корректное число");
                }
                
                indexcomparer indexcomparer = new indexcomparer();
                string filename = textBox1.Text;
                string file = richTextBox1.Text;
                int alllenght = textBox1.Text.Length + richTextBox1.Text.Length;
                string rewrstring = "*";
                for (int i = 0; i < alllenght; i++)
                {
                    rewrstring += "*";
                }
                try
                {
                    if (disk.Substring(newindex).StartsWith(rewrstring))
                    {
                        disk = disk.Remove(newindex, rewrstring.Length);
                        disk = disk.Insert(newindex, filename + "^" + file);
                        File.WriteAllText("harddrive.txt", disk);
                        StreamWriter writer = File.AppendText("catalog.txt");
                        if (!startindexes.Contains(newindex))
                        {
                            writer.WriteLine(newindex);
                        }
                        writer.Close();
                        startindexes.Clear();
                        StreamReader reader = new StreamReader("catalog.txt");
                        while (!reader.EndOfStream)
                        {
                            startindexes.Add(Int32.Parse(reader.ReadLine()));
                        }
                        reader.Close();
                        startindexes.Sort(indexcomparer);
                        File.Delete("catalog.txt");
                        StreamWriter streamWriter = File.AppendText("catalog.txt");
                        foreach (int index in startindexes)
                        {
                            streamWriter.WriteLine(index);
                        }
                        streamWriter.Close();
                        MessageBox.Show("Файл успешно сохранён");
                        //закрытие формы после сохранения
                        this.Close();
                        Form2 form2 = new Form2();
                        form2.Show();
                    }
                    else
                    {
                        MessageBox.Show("Невозможно записать файл в указанное место. Недостаточно места на диске или место занято другим файлом");
                    }
                    
                }
                catch(Exception exc)
                {
                    MessageBox.Show("Ошибка адреса ячейки. Выберите другой адрес или уменьшите файл");
                }

            }
            else
            {
                if (overwrite == false) //если файл новый
                {
                    rewrite();
                }
                else //если файл перезаписывается
                {
                    string rm = "*";
                    for (int i = 0; i < filename.Length + file.Length; i++)
                    {
                        rm += "*";
                    }
                    startindexes.Remove(disk.IndexOf(filename + "^" + file));
                    disk = disk.Replace(filename + "^" + file, rm);
                    File.Delete("catalog.txt");
                    StreamWriter streamWriter = File.AppendText("catalog.txt");
                    foreach (int index in startindexes)
                    {
                        streamWriter.WriteLine(index);
                    }
                    streamWriter.Close();
                    rewrite();
                }
           
            }

        }
        public void rewrite()
        {
            if (disk.Contains(textBox1.Text + "^"))
            {
                MessageBox.Show("Ошибка, заданное имя файла уже сущестует");
            }
            else
            {
                indexcomparer indexcomparer = new indexcomparer();
                string filename = textBox1.Text;
                string file = richTextBox1.Text;
                int alllenght = textBox1.Text.Length + richTextBox1.Text.Length;
                string rewrstring = "*";
                for (int i = 0; i < alllenght; i++)
                {
                    rewrstring += "*";
                }
                int startindex = disk.IndexOf(rewrstring);
                disk = disk.Remove(startindex, rewrstring.Length);
                disk = disk.Insert(startindex, filename + "^" + file);
                File.WriteAllText("harddrive.txt", disk);
                StreamWriter writer = File.AppendText("catalog.txt");
                if (!startindexes.Contains(startindex))
                {
                    writer.WriteLine(startindex);
                }
                writer.Close();
                startindexes.Clear();
                StreamReader reader = new StreamReader("catalog.txt");

                while (!reader.EndOfStream)
                {
                    startindexes.Add(Int32.Parse(reader.ReadLine()));
                }
                reader.Close();

                startindexes.Sort(indexcomparer);
                File.Delete("catalog.txt");
                StreamWriter streamWriter = File.AppendText("catalog.txt");
                foreach (int index in startindexes)
                {
                    streamWriter.WriteLine(index);
                }
                streamWriter.Close();
                MessageBox.Show("Файл успешно сохранён");

                //закрытие формы после сохранения
                this.Close();
                Form2 form2 = new Form2();
                form2.Show();
            }
        }
    }
}
