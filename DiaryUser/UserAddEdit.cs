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
using System.Xml.Serialization;

namespace DiaryUser
{
    public partial class UserAddEdit : Form
    {
       
        private int _studentId;
        private Student _student;
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        

        public UserAddEdit(int id = 0)
        {
            InitializeComponent();
            SetClassesItems(Program.AllClasses);
            _studentId = id;
            GetStudentData();
            

        }
        private void SetClassesItems(List<string> items)

          
        {
            foreach (var item in items)
            {

                cbClassesAddEdit.Items.Add(item.ToString());
            }
            
        }
        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);
                if (_student == null)
                    throw new Exception("Brak użytkownika o podanym Id");
                FillTextBoxes();
            }
        }
        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbTechnology.Text = _student.Technology;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            richTextBoxComments.Text = _student.Comments;
            cbClassesAddEdit.SelectedItem = _student.Class;
            checkBoxAdditionalClass.Checked = _student.AdditionalClass;
        }
        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
                AssignToNewStudent(students);
            AddNewUserToList(students);
            _fileHelper.SerializeToFile(students);
            Close();




        }
        private void  AddNewUserToList(List<Student> students)
        {
            var student = new Student()
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Math = tbMath.Text,
                Physics = tbPhysics.Text,
                Technology = tbTechnology.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                Comments = richTextBoxComments.Text,
                Class = cbClassesAddEdit.SelectedItem.ToString(),
                AdditionalClass = checkBoxAdditionalClass.Checked
            };

            students.Add(student);
        }
        private void AssignToNewStudent(List<Student> students)
        {
            var studentHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentHighestId == null ? 1 : studentHighestId.Id + 1;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
