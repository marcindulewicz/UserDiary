using DiaryUser.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace DiaryUser
{
    public partial class Main : Form
    {
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        private string NoFilterDataWhen = Program.NoFilterDataWhen;

        public bool IsMaximized
        {
            get { return Settings.Default.IsMaximised; }
            set { Settings.Default.IsMaximised = value; }
        }
        public Main()
        {
            InitializeComponent();
            MaximizeWindowOnStart();
            RefreshDiary(Program.NoFilterDataWhen);
            SetColumnHeaders();
            SetClassesItems(Program.AllClasses);
            cbClasses.SelectedItem = cbClasses.Items[0];
        }
        private void MaximizeWindowOnStart()
        {
            if (IsMaximized)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }
        private void SetClassesItems(List<string> items)
        {

            cbClasses.Items.Add(Program.NoFilterDataWhen);
            foreach (var item in items)
            {
                cbClasses.Items.Add(item.ToString());
            }

        }
        private void RefreshDiary(string classes)
        {
            var students = _fileHelper.DeserializeFromFile();
            var students2 = students.OrderBy(x => x.Id).ToList();
            if (classes!=NoFilterDataWhen)
                students2 = students2.FindAll(x => x.Class == classes);
            dgvStudents.DataSource = students2;

        }
        private void SetColumnHeaders()
        {
            dgvStudents.Columns[0].HeaderText = "Numer ID";
            dgvStudents.Columns[1].HeaderText = "Imie";
            dgvStudents.Columns[2].HeaderText = "Nazwisko";
            dgvStudents.Columns[3].HeaderText = "Matematyka";
            dgvStudents.Columns[4].HeaderText = "Technologia";
            dgvStudents.Columns[5].HeaderText = "Fizyka";
            dgvStudents.Columns[6].HeaderText = "Język Polski";
            dgvStudents.Columns[7].HeaderText = "Język obcy";
            dgvStudents.Columns[8].HeaderText = "Uwagi";
            dgvStudents.Columns[9].HeaderText = "Klasa";
            dgvStudents.Columns[10].HeaderText = "Dodatkowe zajęcia";


        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var userAddEditWindow = new UserAddEdit();
            userAddEditWindow.FormClosing += UserAddEditWindow_FormClosing;
            userAddEditWindow.ShowDialog();
        }

        private void UserAddEditWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            RefreshDiary(cbClasses.SelectedItem.ToString());
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count < 1)
            {
                MessageBox.Show("Proszę wskaż ucznia w tabeli");
                return;
            }
            var userAddEditWindow = new UserAddEdit(Convert.ToInt32(dgvStudents.SelectedRows[0].Cells[0].Value));
            userAddEditWindow.FormClosing += UserAddEditWindow_FormClosing;
            userAddEditWindow.ShowDialog();
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count < 1)
            {
                MessageBox.Show("Proszę wskaż ucznia w tabeli");
                return;
            }
            var selectedStudent = dgvStudents.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy na pewno chcesz usunąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim() } ", "Usuwanie ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                Deleteuser(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary(cbClasses.SelectedItem.ToString());
            }
        }
        private void Deleteuser(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (cbClasses.SelectedItem.ToString() == NoFilterDataWhen)
                RefreshDiary(Program.NoFilterDataWhen);
            else
                RefreshDiary(cbClasses.SelectedItem.ToString());

        }
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {

            IsMaximized = WindowState == FormWindowState.Maximized ? true : false;
            Settings.Default.Save();

        }
    }
}
