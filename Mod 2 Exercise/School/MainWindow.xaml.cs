﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Data;
using System.Data.Objects;

using School.Data;

// TODO: Exercise 3: Task 2a: Bring the System.Data and System.Data.Objects namespaces into scope

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

            switch (e.Key)
            {
                case Key.Enter:
                    Student student = studentsList.SelectedItem as Student;

                    // TODO: Exercise 1: Task 3a: Refactor as the editStudent method
                    editStudent(student);
                    break;

                // If the user pressed Insert, add a new student
                case Key.Insert:
                    // TODO: Exercise 1: Task 4a: Refactor as the addNewStudent method
                    addStudent();
                    break;

                // If the user pressed Delete, remove the currently selected student
                case Key.Delete:
                    student = studentsList.SelectedItem as Student;

                    removeStudent(student);
                    break;
            }
        }

        // TODO: Exercise 1: Task 1a: If the user double-clicks a student, edit the details for that student
        private void studentsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Student student = studentsList.SelectedItem as Student;

            editStudent(student);
        }

        private void editStudent(Student student)
        {
            StudentForm sf = new StudentForm();

            if (student != null)
            {
                // Set the title of the form and populate the fields on the form with the details of the student           
                sf.Title = "Edit Student Details";

                sf.firstName.Text = student.FirstName;
                sf.lastName.Text = student.LastName;

                sf.dateOfBirth.Text = student.DateOfBirth.ToString("d");

                // Display the form
                if (sf.ShowDialog().Value)
                {
                    // When the user closes the form, copy the details back to the student
                    student.FirstName = sf.firstName.Text;
                    student.LastName = sf.lastName.Text;

                    student.DateOfBirth = DateTime.Parse(sf.dateOfBirth.Text);

                    // Enable saving (changes are not made permanent until they are written back to the database)
                    saveChanges.IsEnabled = true;
                }
            }

        }

        private void addStudent()
        {
            // Use the StudentsForm to get the details of the student from the user
            StudentForm sf = new StudentForm();

            // Set the title of the form to indicate which class the student will be added to (the class for the currently selected teacher)
            sf.Title = "New Student for Class " + teacher.Class;

            if (sf.ShowDialog().Value)
            {
                // When the user closes the form, retrieve the details of the student from the form
                // and use them to create a new Student object
                Student newStudent = new Student();

                newStudent.FirstName = sf.firstName.Text;
                newStudent.LastName = sf.lastName.Text;

                newStudent.DateOfBirth = DateTime.Parse(sf.dateOfBirth.Text);

                // Assign the new student to the current teacher
                this.teacher.Students.Add(newStudent);

                // Add the student to the list displayed on the form
                this.studentsInfo.Add(newStudent);

                // Enable saving (changes are not made permanent until they are written back to the database)
                saveChanges.IsEnabled = true;
            }

        }

        private void removeStudent(Student student)
        {
            if (student != null)
            {
                // TODO: Exercise 1: Task 4b: Refactor as the removeStudent method

                // Prompt the user to confirm that the student should be removed
                var result = MessageBox.Show(String.Format("Remove {0} {1}", student.FirstName, student.LastName),
                    "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                // If the user clicked Yes, remove the student from the database
                if (result == MessageBoxResult.Yes)
                {
                    schoolContext.Students.DeleteObject(student);

                    // Enable saving (changes are not made permanent until they are written back to the database)
                    saveChanges.IsEnabled = true;
                }
            }

        }
        // Save changes back to the database and make them permanent
        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Exercise 3: Task 2b: Save the changes by calling the SaveChanges method of the schoolContext object
            // TODO: Exercise 3: Task 3a: If an OptimisticConcurrencyException occurs then another user has changed the same students earlier, then overwrite their changes with the new data (see the lab instructions for details)
            // TODO: Exercise 3: Task 3b: If an UpdateException occurs then report the error to the user and rollback (see the lab instructions for details)
            // TODO: Exercise 3: Task 3c: If some other sort of error has occurs, report the error to the user and retain the data so the user can try again - the error may be transitory (see the lab instructions for details)

            try
            {
                schoolContext.SaveChanges();
                saveChanges.IsEnabled = false;
            } catch( OptimisticConcurrencyException )
            {
                schoolContext.Refresh(RefreshMode.StoreWins, schoolContext.Students);
                schoolContext.SaveChanges();
            } catch( UpdateException uEx )
            {
                // If some sort of database exception has occured,
                // then display the reason for the exception and rollback
                MessageBox.Show(uEx.InnerException.Message, "Error saving changes",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                schoolContext.Refresh(RefreshMode.StoreWins, schoolContext.Students);
            } catch( Exception ex )
            {
                MessageBox.Show(ex.Message, "Error saving changes",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                schoolContext.Refresh(RefreshMode.StoreWins, schoolContext.Students);
            }
        }
    }

    [ValueConversion(typeof(string), typeof(Decimal))]
    class AgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                              System.Globalization.CultureInfo culture)
        {
            // Convert the date of birth provided in the value parameter and convert to the age of the student in years
            if (value != null)
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