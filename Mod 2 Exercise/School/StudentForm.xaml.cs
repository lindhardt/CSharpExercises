using System;
using System.Windows;

namespace School
{
    /// <summary>
    /// Interaction logic for StudentForm.xaml
    /// </summary>
    public partial class StudentForm : Window
    {
        #region Predefined code

        public StudentForm()
        {
            InitializeComponent();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Exercise 2: Task 2a: Check that the user has provided a first name
            // TODO: Exercise 2: Task 2b: Check that the user has provided a last name
            // TODO: Exercise 2: Task 3a: Check that the user has entered a valid date for the date of birth
            // TODO: Exercise 2: Task 3b: Verify that the student is at least 5 years old

            if (string.IsNullOrEmpty(firstName.Text))
            {
                MessageBox.Show("Student must have a have a first name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(lastName.Text))
            {
                MessageBox.Show("Student must have a have a last name", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(dateOfBirth.Text))
            {
                MessageBox.Show("Student must have a have a date of birth", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            DateTime result;

            if( !DateTime.TryParse(dateOfBirth.Text, out result) )
            {
                MessageBox.Show("The date of birth must be valid date", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            int age = (int)(DateTime.Now.Subtract(result).Days / 365.25);

            if( age < 5 )
            {
                MessageBox.Show("The student must be at least 5 years old", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            this.DialogResult = true;
        }

        #endregion
    }
}
