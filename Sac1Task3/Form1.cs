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

namespace Sac1Task3
{
    public partial class Form1 : Form
    {
        //path for the CSV nomader where you put the reposoitory
        string path = Path.Combine(Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 9) + "Task3_Shop_Data.csv");
        OpenFileDialog openFileDialog = new OpenFileDialog();

        List<string> lines = new List<string>();

        //actual storage of all the values
        List<Textbooks> bookList = new List<Textbooks>();
        //whats displayed to the dgv
        List<Textbooks> displayList = new List<Textbooks>();

        BindingSource bs = new BindingSource();

        //values for selection sorting algo
        int min;
        int temp;
        bool swap = false;

        public Form1()
        {
            bs.DataSource = displayList;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Fileialog Params
            openFileDialog.Filter = " CSV | *.csv| Text File | *.txt";
            openFileDialog.InitialDirectory = Path.GetDirectoryName(path);
            openFileDialog.FileName = "Task3_Shop_Data.csv";

            //resets the table and total profit value
            dgvTable.Rows.Clear();

            //opens file dialog and checks if OK is pressed
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog.FileName;

                //reads all lines from selected file
                lines = File.ReadAllLines(path).ToList();

                //removes the headers
                if (lines[0].Contains("Purchase price")) { lines.RemoveAt(0); }

                //loops through adding them to items
                foreach (string line in lines)
                {
                    List<string> items = line.Split(',').ToList();

                    //adds the corresponding values in the CSV to the Texbooks class
                    Textbooks t = new Textbooks(items[0], items[1], items[4], items[5], items[6]);

                    //adds to bookList and displayList
                    bookList.Add(t);
                    displayList.Add(t);
                }
                dgvTable.DataSource = bs;
                bs.ResetBindings(false);
            }
        }

        private void txtbxFilter_TextChanged(object sender, EventArgs e)
        {
            //switch depending on the selected item in the combobox
            switch (cmbxFilter.SelectedItem)
            {
                case "Textbook":
                    filter_By_Textbook();
                    break;
                case "Subject":
                    filter_By_subject();
                    break;
                case "Rating":
                    filter_By_Rating();
                    break;
            }
        }
        private void filter_By_Textbook()
        {
            displayList.Clear();
            foreach(Textbooks entry in bookList)
            {
                //if Textbook contains the search text
                if (entry.Textbook.ToLower().Contains(txtbxFilter.Text.ToLower()))
                {
                    displayList.Add(entry);
                }
            }
            dgvTable.DataSource = bs;
            bs.ResetBindings(false);
        }
        private void filter_By_subject()
        {
            displayList.Clear();
            foreach (Textbooks entry in bookList)
            {
                //if Subject contains the search text
                if (entry.Subject.ToLower().Contains(txtbxFilter.Text.ToLower()))
                {
                    displayList.Add(entry);
                }
            }
            dgvTable.DataSource = bs;
            bs.ResetBindings(false);
        }

        private void filter_By_Rating()
        {
            if (txtbxFilter.Text == "")
            {
                //fills the displayList back up before calling sort
                foreach (Textbooks entry in bookList)
                {
                    displayList.Add(entry);
                }
                sort();
            }
            else
            {
                displayList.Clear();
                foreach (Textbooks entry in bookList)
                {
                    //if rating contains the search text
                    if (entry.Rating.ToLower().Contains(txtbxFilter.Text.ToLower()))
                    {
                        displayList.Add(entry);
                    }
                }
            }
            dgvTable.DataSource = bs;
            bs.ResetBindings(false);
        }

        private void sort()
        {
            //converts all "none" values in the ratings column to 0 so it can be parsed for selection sorting calculations
            in_Out("none", "0");

            for (int a = 0; a < displayList.Count; a++)
            {
                //sets the min to the first value
                min = Int32.Parse(displayList[a].Rating);
                swap = false;
                for (int i = a; i < displayList.Count; i++)
                {
                    //checks if min is greater or lesser than each number and sets it accordingly
                    if (Int32.Parse(displayList[i].Rating) > min)
                    {
                        min = Int32.Parse(displayList[i].Rating);
                        temp = i;
                        swap = true;
                    }
                }
                //swaps the positions of each value if swap is true
                if (swap is true)
                {
                    (displayList[a], displayList[temp]) = (displayList[temp], displayList[a]);
                }
            }
            //converts all "0"s in the ratings columns back to "none" for accuracy sake
            in_Out("0", "none");
        }

        private void cmbxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //clears filter 
            txtbxFilter.Text = "";

            //calls sort if "Ratings" is selected in the combo box
            if (cmbxFilter.SelectedIndex == 2) sort();
            dgvTable.DataSource = bs;
            bs.ResetBindings(false);
        }

        private void in_Out(string inVal, string outVal)
        {
            foreach (Textbooks entry in displayList)
            {
                Textbooks t = entry;
                if (t.Rating == inVal) t.Rating = outVal;
            }
        }
        //check
    }
}
