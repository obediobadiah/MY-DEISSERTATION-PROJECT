﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace BANK_CUSTOMERS_MANAGEMENT
{
    public partial class BankAccountDetails : UserControl
    {
        public BankAccountDetails()
        {
            InitializeComponent();
            label5.Visible = false;
            Date_BankAccountLimitDate.Visible = false;
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-454MBGL;Initial Catalog=BANK_CUSTOMERS_Disseration_Project_DB;Integrated Security=True");

        CommunicationsSender obj = new CommunicationsSender();
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_BankAccountType.SelectedItem == "Savings Account")
            {
                label5.Visible = true;
                Date_BankAccountLimitDate.Visible = true;
            }
            else
            {
                label5.Visible = false;
                Date_BankAccountLimitDate.Visible = false;
            }
        }

        private void button_save_acc_cr_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM BANK_ACCOUNT_DETAILS WHERE Identifier = '" + txt_BankAcccountIdentifier.Text + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    MessageBox.Show("The Account Name  " + txt_BankAcccountIdentifier.Text + " has already account details in the system");
                    clear();
                }
                else
                {
                    SqlCommand cmd1 = new SqlCommand("SELECT First_Name FROM PERSONAL_DETAILS WHERE First_Name = '" + txt_BankAcccountIdentifier.Text + "'", conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    DataTable dt1 = new DataTable();
                    da1.Fill(dt1);

                    if (dt1.Rows.Count >= 1)
                    {
                        Save();
                        CommunicationAccountNumber();
                        CommunicationMobileNumber();
                        message();
                        obj.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("The Name  " + txt_BankAcccountIdentifier.Text + " Doesn't exit in the personal details");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            
        }
        public void clear()
        {
            txt_BankAcccountIdentifier.Text = "";
        }

        public void Save()
        {
            try
            {
                if (txt_BankAcccountIdentifier.Text == "" || cb_BankAccountType.Text == "")
                {
                    MessageBox.Show("You cannot end the procees without filling all required fields");
                }
                else if (cb_BankAccountType.SelectedItem == "Checking Account")
                {
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand("INSERT into BANK_ACCOUNT_DETAILS values (@Date_of_creation,@Identifier,@Bank_Account_Type,@Limit_Date)", conn);
                    cmd2.Parameters.AddWithValue("@Date_of_creation", label9.Text);
                    cmd2.Parameters.AddWithValue("@Identifier", txt_BankAcccountIdentifier.Text);
                    cmd2.Parameters.AddWithValue("@Bank_Account_Type", cb_BankAccountType.SelectedItem);
                    cmd2.Parameters.AddWithValue("@Limit_Date", "----");

                    int i;
                    i = cmd2.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Process finished successfully, Account created", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
                }
                else if (cb_BankAccountType.SelectedItem == "Savings Account")
                {
                    conn.Open();
                    SqlCommand cmd2 = new SqlCommand("INSERT into BANK_ACCOUNT_DETAILS values (@Date_of_creation,@Identifier,@Bank_Account_Type,@Limit_Date)", conn);

                    cmd2.Parameters.AddWithValue("@Date_of_creation", label9.Text);
                    cmd2.Parameters.AddWithValue("@Identifier", txt_BankAcccountIdentifier.Text);
                    cmd2.Parameters.AddWithValue("@Bank_Account_Type", cb_BankAccountType.SelectedItem);
                    cmd2.Parameters.AddWithValue("@Limit_Date", Date_BankAccountLimitDate.Value.Date.ToShortDateString());

                    int i;
                    i = cmd2.ExecuteNonQuery();
                    if (i > 0)
                    {
                        MessageBox.Show("Process finished successfully, Account created", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        public void CommunicationAccountNumber()
        {
            try
            {
                conn.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT ID_Number FROM BANK_ACCOUNT_DETAILS where Identifier = '" + txt_BankAcccountIdentifier.Text + "'", conn);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                if (dt1.Rows.Count == 1)
                {
                    obj.label_AccountNumber.Text = dt1.Rows[0]["ID_Number"].ToString();
                }
                else
                {
                    MessageBox.Show("This Account number does not exist", "Information", MessageBoxButtons.OK);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void message()
        {
            obj.txt_Message.Text = "You have created on IMARA Cooperative of Savings and Credtis an " + cb_BankAccountType.SelectedItem.ToString() + " Number " + obj.label_AccountNumber.Text + " named " + txt_BankAcccountIdentifier.Text + " on " + label9.Text;
        }

        public void CommunicationMobileNumber()
        {
            try
            {
                string Code;
                string Number;

                conn.Open();
                SqlCommand cmd2 = new SqlCommand("SELECT Mobile_Number_Code,Mobile_Number FROM PERSONAL_DETAILS where First_Name = '" + txt_BankAcccountIdentifier.Text + "'", conn);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    Code = dt1.Rows[0]["Mobile_Number_Code"].ToString();
                    Number = dt1.Rows[0]["Mobile_Number"].ToString();

                    obj.txt_PhoneNumber.Text = Code + Number;
                }
                else
                {
                    MessageBox.Show("The mobile number of this Account name does not exist", "Information", MessageBoxButtons.OK);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txt_BankAcccountIdentifier_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
                e.Handled = true;
        }

        private void BankAccountDetails_Load(object sender, EventArgs e)
        {
            label9.Text = DateTime.Today.ToShortDateString();
        }
    }
}
