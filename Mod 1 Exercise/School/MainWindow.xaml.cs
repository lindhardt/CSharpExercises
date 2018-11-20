using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using School.Data;


namespace School
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Connection to the School database
        private SchoolDBEntities schoolContext = null;

        // Field for tracking the currently selected teacher
        private Teacher teacher = null;

        // List for tracking the students assigned to the teacher's class
        private IList studentsInfo = null;

        #region Predefined code

        public MainWindow()
        {
            InitializeComponent();
            //studentsList.Focus();
        }

        // Connect to the database and display the list of teachers when the window appears
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.schoolContext = new SchoolDBEntities();
            teachersList.DataContext = this.schoolContext.Teachers;
        }

        // When the user selects a different teacher, fetch and display the students for that teacher
        private void teachersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Find the teacher that has been selected
            this.teacher = teachersList.SelectedItem as Teacher;
            this.schoolContext.LoadProperty<Teacher>(this.teacher, s => s.Students);

            // Find the students for this teacher
            this.studentsInfo = ((IListSource)teacher.Students).GetList();

            // Use databinding to display these students
            studentsList.DataContext = this.studentsInfo;
        }

        #endregion

        // When the user presses a key, determine whether to add a new student to a class, remove a student from a class, or modify the details of a student
        private void studentsList_KeyDown(object sender, KeyEventArgs e)
        {
            // TODO: Exercise 1: Task 1a: If the user pressed Enter, edit the details for the currently selected student
            // TODO: Exercise 1: Task 2a: Use the StudentsForm to display and edit the details of the student
            // TODO: Exercise 1: Task 2b: Set the title of the form and populate the fields on the form with the details of the student
            // TODO: Exercise 1: Task 3a: Display the form
            // TODO: Exercise 1: Task 3b: When the user closes the form, copy the details back to the student
            // TODO: Exercise 1: Task 3c: Enable saving (changes are not made permanent until they are written back to the database)

            switch (e.Key)
            {
                case Key.Enter:
                    Student student = studentsList.SelectedItem as Student;
                    StudentForm sf = new StudentForm();

                    if (student != null)
                    {
                        sf.Title = "Edit Student Details";

                        sf.firstName.Text = student.FirstName;
                        sf.lastName.Text = student.LastName;

                        sf.dateOfBirth.Text = student.DateOfBirth.ToString("d");

                        if (sf.ShowDialog().Value)
                        {
                            student.FirstName = sf.firstName.Text;
                            student.LastName = sf.lastName.Text;

                            student.DateOfBirth = DateTime.Parse( sf.dateOfBirth.Text);

                            saveChanges.IsEnabled = true;
                        }
                    }
                    break;
                // TODO: Exercise 2: Task 1a: If the user pressed Insert, add a new student
                // TODO: Exercise 2: Task 2a: Use the StudentsForm to get the details of the student from the user
                // TODO: Exercise 2: Task 2b: Set the title of the form to indicate which class the student will be added to (the class for the currently selected teacher)
                // TODO: Exercise 2: Task 3a: Display the form and get the details of the new student
                // TODO: Exercise 2: Task 3b: When the user closes the form, retrieve the details of the student from the form and use them to create a new Student object
                // TODO: Exercise 2: Task 4a: Assign the new student to the current teacher
                // TODO: Exercise 2: Task 4b: Add the student to the list displayed on the form
                // TODO: Exercise 2: Task 4c: Enable saving (changes are not made permanent until they are written back to the database)

                case Key.Insert:
                    sf = new StudentForm();

                    if( sf.ShowDialog().Value )
                    {
                        Student newStudent = new Student();

                        newStudent.FirstName = sf.firstName.Text;
                        newStudent.LastName = sf.lastName.Text;

                        newStudent.DateOfBirth = DateTime.Parse(sf.dateOfBirth.Text);

                        teacher.Students.Add(newStudent);

                        saveChanges.IsEnabled = true;
                    }
                    break;
                // TODO: Exercise 3: Task 1a: If the user pressed Delete, remove the currently selected student
                // TODO: Exercise 3: Task 2a: Prompt the user to confirm that the student should be removed
                // TODO: Exercise 3: Task 3a: If the user clicked Yes, remove the student from the database
                // TODO: Exercise 3: Task 3b: Enable saving (changes are not made permanent until they are written back to the database)

                case Key.Delete:
                    student = studentsList.SelectedItem as Student;

                    if(student != null)
                    {
                        var result=MessageBox.Show(String.Format("Remove {0} {1}", student.FirstName, student.LastName),
                            "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if( result == MessageBoxResult.Yes )
                        {
                            schoolContext.Students.DeleteObject(student);

                            saveChanges.IsEnabled = true;
                        }
                    }
                    break;
            }
        }

        #region Predefined code

        private void studentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        // Save changes back to the database and make them permanent
        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }

    [ValueConversion(typeof(string), typeof(Decimal))]
    class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            // Convert the date of birth provided in the value parameter and convert to the age of the student in years
            // TODO: Exercise 4: Task 2a: Check that the value provided is not null. If it is, return an empty string
            // TODO: Exercise 4: Task 2b: Convert the value provided into a DateTime value
            // TODO: Exercise 4: Task 2c: Work out the difference between the current date and the value provided
            // TODO: Exercise 4: Task 2d: Convert this result into a number of years
            // TODO: Exercise 4: Task 2e: Convert the number of years into a string and return it
            if( value != null )
            {
                DateTime dob = (DateTime)value;

                int age = (int)(DateTime.Now.Subtract(dob).Days / 365.25);

                return "" + age; 
            }
            return "";
        }

        #region Predefined code

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
