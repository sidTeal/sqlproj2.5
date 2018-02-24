using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tisdale_Project_2
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();

            updateComboBoxes();

        }

        private void updateComboBoxes()
        {
            string[] employeeArrayView = DatabaseManager.getEmployeeNames();
            string[] employeeArrayModify = new string[employeeArrayView.Length];
            string[] employeeArrayDelete = new string[employeeArrayView.Length];

            employeeArrayView.CopyTo(employeeArrayDelete, 0);
            employeeArrayView.CopyTo(employeeArrayModify, 0);

            cbViewEmployee.DataSource = employeeArrayView;
            cbViewEmployee.SelectedIndex = 0;

            cbDeleteEmployee.DataSource = employeeArrayDelete;
            cbDeleteEmployee.SelectedIndex = 0;

            cbModifyEmployee.DataSource = employeeArrayModify;
            cbModifyEmployee.SelectedIndex = 0;

            string[] customerArrayView = DatabaseManager.getCustomerNames();
            string[] customerArrayDelete = new string[customerArrayView.Length];
            string[] customerArrayModify = new string[customerArrayView.Length];

            customerArrayView.CopyTo(customerArrayDelete, 0);
            customerArrayView.CopyTo(customerArrayModify, 0);

            cbViewCustomer.DataSource = customerArrayView;
            cbViewCustomer.SelectedIndex = 0;

            cbDeleteCustomer.DataSource = customerArrayDelete;
            cbDeleteCustomer.SelectedIndex = 0;

            cbModifyCustomer.DataSource = customerArrayModify;
            cbModifyCustomer.SelectedIndex = 0;
        }


        private void bnAddCustomer_Click(object sender, EventArgs e)
        {
            Customer temp = null;
            bool legalCustomerValues = false;
            string dateString = "";

            try
            {
                string firstName = tbCustomerFirstName.Text.Replace(" ", "");
                string lastName = tbCustomerLastName.Text.Replace(" ", "");


                temp = new Customer(firstName, lastName, tbCustomerAddress.Text, dtpCustomerDateOfBirth.Value, tbCustomerFavoriteDepartment.Text);
                dateString = dtpCustomerDateOfBirth.Value.ToString("yyyy-MM-dd");
                legalCustomerValues = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (legalCustomerValues)
            {
                Console.WriteLine(temp.FirstName);
                DatabaseManager.addCustomer(temp.FirstName, temp.LastName, temp.Address, dateString, temp.FavoriteDepartment);
                MessageBox.Show("Customer Added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                updateComboBoxes();

                tbCustomerFirstName.Text = "";
                tbCustomerLastName.Text = "";
                tbCustomerAddress.Text = "";
                dtpCustomerDateOfBirth.Value = DateTime.Now;
                tbCustomerFavoriteDepartment.Text = "";
            }

            updateComboBoxes();

        }


        private void cbViewEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            string viewSelection = cbViewEmployee.Text;
            if (viewSelection != "")
            {
                string[] name = new string[1];

                viewSelection = viewSelection.Replace(",", "");
                name = viewSelection.Split(' ');
                //name[0] = last name
                //name[1] = first name

                dgvViewEmployee.DataSource = DatabaseManager.viewEmployee(name[0], name[1]);
                updateComboBoxes();
            }

        }

        private void cbDeleteCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string deleteSelection = cbDeleteCustomer.Text;

            if (deleteSelection != "")
            {
                string[] name = new string[1];

                deleteSelection = deleteSelection.Replace(",", "");
                name = deleteSelection.Split(' ');
                //name[0] = last name
                //name[1] = first name

                int personID = DatabaseManager.getPersonIdNumber(name[1], name[0]);
                DatabaseManager.deleteCustomer(personID);
                updateComboBoxes();
                MessageBox.Show("Customer deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void cbViewCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

            string viewSelection = cbViewCustomer.Text;
            if (viewSelection != "")
            {
                string[] name = new string[1];

                viewSelection = viewSelection.Replace(",", "");
                name = viewSelection.Split(' ');
                //name[0] = last name
                //name[1] = first name

                dgvViewCustomer.DataSource = DatabaseManager.viewCustomer(name[0], name[1]);
                updateComboBoxes();
            }



        }
        int personID;

        private void cbModifyCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string modifySelection = cbModifyCustomer.Text;

            if (modifySelection != "")
            {
                string[] name = new string[1];

                modifySelection = modifySelection.Replace(",", "");
                name = modifySelection.Split(' ');
                //name[0] = last name
                //name[1] = first name

                personID = DatabaseManager.getPersonIdNumber(name[1], name[0]);

                pnlModifyCustomer.Visible = true;

                string cusInfo = DatabaseManager.getCustomerToModify(personID);

                string[] cleanCusInfo = new string[3];
                cleanCusInfo = cusInfo.Split('|');

                tbModifyCustomerFirstName.Text = cleanCusInfo[0];
                tbModifyCustomerLastName.Text = cleanCusInfo[1];
                tbModifyCustomerAddress.Text = cleanCusInfo[2];
                dtpModifyCustomerDateOfBirth.Value = Convert.ToDateTime(cleanCusInfo[3]);
                tbModifyCustomerFavoriteDepartment.Text = DatabaseManager.getCustomerFavoriteDepartment(personID);


                updateComboBoxes();

                //MessageBox.Show("Customer modified.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void bnCancelModify_Click(object sender, EventArgs e)
        {
            pnlModifyCustomer.Visible = false;
        }
        
        private void bnModifyCustomer_Click(object sender, EventArgs e)
        {
            
            string dateString = dtpModifyCustomerDateOfBirth.Value.ToString("yyyy-MM-dd");
            Console.WriteLine(personID);
            DatabaseManager.modifyCustomer(personID, tbModifyCustomerFirstName.Text, tbModifyCustomerLastName.Text,
                tbModifyCustomerAddress.Text, dateString, tbModifyCustomerFavoriteDepartment.Text);

            pnlModifyCustomer.Visible = false;
            MessageBox.Show("Customer modified.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            updateComboBoxes();
        }

        private void bnAddEmployee_Click(object sender, EventArgs e)
        {
            Employee temp = null;
            bool legalEmployeeValues = false;
            string dateString = "";

            try
            {
                string firstName = tbEmployeeFirstName.Text.Replace(" ", "");
                string lastName = tbEmployeeLastName.Text.Replace(" ", "");


                temp = new Employee(firstName, lastName, tbEmployeeAddress.Text, dtpEmployeeDateOfBirth.Value, tbEmployeeDepartment.Text);
                dateString = dtpEmployeeDateOfBirth.Value.ToString("yyyy-MM-dd");
                legalEmployeeValues = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Entry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (legalEmployeeValues)
            {
                Console.WriteLine(temp.FirstName);
                DatabaseManager.addEmployee(temp.FirstName, temp.LastName, temp.Address, dateString, temp.Department);
                MessageBox.Show("Employee Added", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                updateComboBoxes();

                tbEmployeeFirstName.Text = "";
                tbEmployeeLastName.Text = "";
                tbEmployeeAddress.Text = "";
                dtpEmployeeDateOfBirth.Value = DateTime.Now;
                tbEmployeeDepartment.Text = "";
            }

            updateComboBoxes();

        }

        private void cbDeleteEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            string deleteSelection = cbDeleteEmployee.Text;

            if (deleteSelection != "")
            {
                string[] name = new string[1];

                deleteSelection = deleteSelection.Replace(",", "");
                name = deleteSelection.Split(' ');
                //name[0] = last name
                //name[1] = first name

                int personID = DatabaseManager.getPersonIdNumber(name[1], name[0]);
                DatabaseManager.deleteEmployee(personID);
                updateComboBoxes();
                MessageBox.Show("Employee deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void cbModifyEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            string modifySelection = cbModifyEmployee.Text;

            if (modifySelection != "")
            {
                string[] name = new string[1];

                modifySelection = modifySelection.Replace(",", "");
                name = modifySelection.Split(' ');
                //name[0] = last name
                //name[1] = first name

                personID = DatabaseManager.getPersonIdNumber(name[1], name[0]);

                pnlModifyEmployee.Visible = true;

                string empInfo = DatabaseManager.getCustomerToModify(personID);

                string[] cleanEmpInfo = new string[3];
                cleanEmpInfo = empInfo.Split('|');

                tbModifyEmployeeFirstName.Text = cleanEmpInfo[0];
                tbModifyEmployeeLastName.Text = cleanEmpInfo[1];
                tbModifyEmployeeAddress.Text = cleanEmpInfo[2];
                dtpModifyEmployeeDateOfBirth.Value = Convert.ToDateTime(cleanEmpInfo[3]);
                tbModifyEmployeeDepartment.Text = DatabaseManager.getEmployeeDepartment(personID);


                updateComboBoxes();

                //MessageBox.Show("Customer modified.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        private void bnCancelModifyEmployee_Click(object sender, EventArgs e)
        {
            pnlModifyEmployee.Visible = false;
        }

        private void bnModifyEmployee_Click(object sender, EventArgs e)
        {
            string dateString = dtpModifyEmployeeDateOfBirth.Value.ToString("yyyy-MM-dd");
            Console.WriteLine(personID);
            DatabaseManager.modifyEmployee(personID, tbModifyEmployeeFirstName.Text, tbModifyEmployeeLastName.Text,
                tbModifyEmployeeAddress.Text, dateString, tbModifyEmployeeDepartment.Text);

            pnlModifyEmployee.Visible = false;
            MessageBox.Show("Employee modified.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            updateComboBoxes();
        }
    }
}
