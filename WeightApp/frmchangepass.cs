using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Lephone.Data;

namespace WeightApp
{
    public partial class frmchangepass : Form
    {
        public frmchangepass()
        {
            InitializeComponent();
        }

        private void btn_cpok_Click(object sender, EventArgs e)
        {
            string npd = txtnewpd.Text;
            string npd1 = txtnewpd1.Text;
            if (npd == "")
            {
                MessageBox.Show("���벻��Ϊ�գ�");
                return;
            }
            if (npd != npd1)
            {
                MessageBox.Show("����ȷ�ϴ���");
                return;
            }
            if (MessageBox.Show("�Ƿ��޸����룿", "������ʾ", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    //Cursor.Current = Cursors.WaitCursor;
                    string sqlstr = "update WMS_Bms_Rec_WGD_Manage set userpassword='" + txtnewpd.Text + "' where username='" + Public.usermangno + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    MessageBox.Show("�޸ĳɹ���");
                    Close();

                }
                catch (Exception err)
                {
                    MessageBox.Show("���ݷ���ʧ�ܣ�" + err.Message);
                }
                finally
                {
                    //Cursor.Current = Cursors.Default;
                }

            }
        }

        private void btn_cpCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}