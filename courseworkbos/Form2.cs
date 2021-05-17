using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
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
            int lastindex = -1;
            for(int i = 0; i<startindexes.Count; i++)
            {
                if(startindexes.ElementAt(i) > index)
                {
                    lastindex = startindexes.ElementAt(i);
                    break;
                }
            }
            if (lastindex ==-1)
            {
                lastindex = startindexes.Last<int>() + disk.Substring(startindexes.Last<int>()).IndexOf("*");
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

        private void button2_Click(object sender, EventArgs e) //закрыть программу
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e) //удалить файл
        {
            string filename = listBox1.SelectedItem.ToString();
            int i;
            for (i = 0; i < startindexes.Count; i++)
            {
                if (disk.Substring(startindexes.ElementAt(i)).StartsWith(filename + "^"))
                {
                    if(disk.Substring(startindexes.ElementAt(i) + filename.Length + 2).Contains("^"))
                    {
                        disk = disk.Remove(startindexes.ElementAt(i), startindexes.ElementAt(i + 1) - startindexes.ElementAt(i));                                
                        string rm = "*";
                        for (int z = 1; z < startindexes.ElementAt(i + 1) - startindexes.ElementAt(i); z++)
                        {
                            rm += "*";
                        }
                        disk = disk.Insert(startindexes.ElementAt(i), rm);
                        startindexes.RemoveAt(i);
                    }
                    else
                    {
                        string file = disk.Substring(startindexes.ElementAt(i));
                        int k = file.IndexOf("*");
                        disk = disk.Remove(startindexes.ElementAt(i), k);
                        string rm = "*";
                        for (int z = 0; z < k; z++)
                        {
                            rm += "*";
                        }
                        disk = disk.Insert(startindexes.ElementAt(i), rm);
                        startindexes.RemoveAt(i);
                    }
                }
            }

            //startindexes.Remove(disk.IndexOf(filename + "^" + file));
            //disk = disk.Replace(filename + "^" + file, rm);
            //File.Delete("catalog.txt");
            //StreamWriter streamWriter = File.AppendText("catalog.txt");
            //foreach (int index in startindexes)
            //{
            //    streamWriter.WriteLine(index);
            //}
            //streamWriter.Close();

            


            File.WriteAllText("harddrive.txt", disk);
            File.Delete("catalog.txt");
            StreamWriter streamWriter = File.AppendText("catalog.txt");
            foreach (int index in startindexes)
            {
                streamWriter.WriteLine(index);
            }
            streamWriter.Close();
            this.Hide();
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}