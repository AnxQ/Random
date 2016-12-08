using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Random
{
    public partial class Form1 : Form
    {
        NameListReader Class;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Class = new NameListReader("..\\..\\students.xml");
            try
            {
                Class.DoReadXML();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            RefreshList();
        }
        private void RefreshList()
        {
            listBox1.Items.Clear();
            Class.ListSort(SortOrder.ID_ASC);
            foreach (var ClassMember in Class.StudentsList)
            {
                listBox1.Items.Add(ClassMember.Key + ":" + ClassMember.Value);
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void loadXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStrip1.Show(Cursor.Position);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.CheckFileExists)
            {
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> LuckyList = new List<int>(Decimal.ToInt32(numericUpDown1.Value));
            var List_pure = new List<int>(Class.StudentsList.Keys);
            System.Random rand;
            foreach (var CheatPerson in Class.CheatData)
            {
                if (CheatPerson.Value == CheatMod.THROW_OUT)
                {
                    List_pure.Remove(CheatPerson.Key);
                }
                else
                {
                    LuckyList.Add(CheatPerson.Key);
                    List_pure.Remove(CheatPerson.Key);
                }
            }
            int m = LuckyList.Count;
            for (int i = 0; i < LuckyList.Capacity-m; ++i)
            {
                rand = new System.Random((DateTime.Now.Millisecond + i).GetHashCode());
                int LuckyID = List_pure.ElementAt(rand.Next(0, List_pure.Count));
                LuckyList.Add(LuckyID);
                List_pure.Remove(LuckyID);
            }
            string FinalText = "";
            foreach (int ID_picked in LuckyList)
            {
                FinalText += ID_picked + ":" + Class.StudentsList[ID_picked] + " !\n";
            }
            MessageBox.Show(FinalText);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > Class.StudentsList.Count)
                numericUpDown1.Value = Class.StudentsList.Count;
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                if (listBox1.SelectedIndex >= 0)
                {
                    int rm_item = Class.StudentsList.ElementAt(listBox1.SelectedIndex).Key;
                    Class.StudentsList.Remove(rm_item);
                    try {
                        Class.CheatData.Remove(rm_item);
                    }
                    catch (Exception err) { }
                }
            RefreshList();
        }
    }
}
