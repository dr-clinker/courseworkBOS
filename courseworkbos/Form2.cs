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
        List<int> indexzvezda = new List<int>();

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
            listBox3.Items.Clear();
            StreamReader streamReader = new StreamReader("catalog.txt");
            while (!streamReader.EndOfStream)
            {
                startindexes.Add(int.Parse(streamReader.ReadLine()));
            }
            streamReader.Close();
            StreamReader streamReader1 = new StreamReader("harddrive.txt");
            disk = streamReader1.ReadToEnd();
            streamReader1.Close();
            string tmp =disk.Replace("*", "");
            listBox3.Items.Add($"общее свободное место на диске - {disk.Length - tmp.Length}");
            listBox3.Items.Add("Свободные блоки на диске: ");
            tmp = disk;
            int start;
            for(int i =0; i< tmp.Length; i++)
            {
                if(tmp[i] == '*')
                {
                    indexzvezda.Add(i);
                }
            }
            for(int i=1; i<indexzvezda.Count; i++)
            {
                if (indexzvezda.ElementAt(i) == indexzvezda.ElementAt(i-1) +1)
                {
                    start = i;
                        try
                        {
                            while (indexzvezda.ElementAt(i) == indexzvezda.ElementAt(i - 1) + 1)
                                {
                                    i += 1;
                                }
                            listBox3.Items.Add($" {indexzvezda.ElementAt(start)} - {indexzvezda.ElementAt(i -1)}");
                            
                        }
                        catch(Exception ex)
                        {
                        listBox3.Items.Add($" {indexzvezda.ElementAt(start)} - {indexzvezda.ElementAt(i - 1)}");
                        }
                    
                }
            }
            
            checkfiles();
        }

        public void checkfiles()
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            foreach (int index in startindexes)
            {
                listBox1.Items.Add(disk.Substring(index, disk.Substring(index).IndexOf('^')));
            }
            for (int i = 0; i < startindexes.Count; i++)
            {
                string temp = disk.Substring(startindexes.ElementAt(i));
                temp = temp.Substring(0, temp.IndexOf("*"));
                try
                {
                    int next = startindexes.ElementAt(i + 1);
                    if(temp.Length > next - startindexes.ElementAt(i))
                    {
                        temp = temp.Substring(0, next - startindexes.ElementAt(i));
                    }
                }
                catch (Exception ex) { }
                listBox2.Items.Add($"Объем - {temp.Length}, первый блок - {startindexes.ElementAt(i)} ");
                
            }
        }

        private void button2_Click(object sender, EventArgs e) //закрыть программу
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e) //удалить файл
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите файл");
            }
            else
            {
                string filename = listBox1.SelectedItem.ToString();
                int i;
                for (i = 0; i < startindexes.Count; i++)
                {
                    if (disk.Substring(startindexes.ElementAt(i)).StartsWith(filename + "^"))
                    {
                        if (disk.Substring(startindexes.ElementAt(i) + filename.Length + 2).Contains("^"))
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
            }
            
            


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

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Выберите файл");
            }
            else
            {
                string selectedfilename = listBox1.SelectedItem.ToString();
                int index = disk.IndexOf(selectedfilename);
                int lastindex = -1;
                for (int i = 0; i < startindexes.Count; i++)
                {
                    if (startindexes.ElementAt(i) > index)
                    {
                        lastindex = startindexes.ElementAt(i);
                        break;
                    }
                }
                if (lastindex == -1)
                {
                    lastindex = startindexes.Last<int>() + disk.Substring(startindexes.Last<int>()).IndexOf("*");
                }

                string file = disk.Substring(index, lastindex - index);
                file = file.Substring(file.IndexOf('^') + 1).Replace("*", "");
                string rewrstring = "*";
                for (int i = 0; i < selectedfilename.Length + file.Length + 1; i++)
                {
                    rewrstring += "*";
                }
                int startindex = disk.IndexOf(rewrstring);
                disk = disk.Remove(startindex, rewrstring.Length);
                disk = disk.Insert(startindex, selectedfilename + "1" + "^" + file);

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
                indexcomparer indexcomparer = new indexcomparer();
                startindexes.Sort(indexcomparer);
                File.Delete("catalog.txt");
                StreamWriter streamWriter = File.AppendText("catalog.txt");
                foreach (int ind in startindexes)
                {
                    streamWriter.WriteLine(ind);
                }
                streamWriter.Close();
                MessageBox.Show("Файл успешно сохранён");

                checkfiles();
            }
            
        }
    }
}