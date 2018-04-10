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
    public partial class frmwgdmanage : Form
    {
        public frmwgdmanage()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void searchwgd()
        {
            string sqlstr = "";
            DataSet ds = null;
            string pch = txtpch.Text;
            sqlstr = "select top 3000 *,case wcbz "+
                     " when 0 then '����'  "+
                     " when 1 then '������'  "+
                     " when 2 then '�������'  "+
                     " when 3 then '������'  "+
                     " end wcbzname  from WMS_Bms_Rec_WGD "+
                     " where (pch like '%"+pch+"%') ";
            ds = DbEntry.Context.ExecuteDataset(sqlstr); 
            dgvwgd.DataSource=ds.Tables[0];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvwgd.SelectedRows.Count == 0)
            {
                MessageBox.Show("ѡ���깤����","ϵͳ��ʾ");
                return;
            }
            if (!Public.getWeightManageAuthority(Public.usermangno, "SearchWGD"))
            {
                MessageBox.Show("û��Ȩ�ޣ�","ϵͳ��ʾ");
                return;
            }
            string wgdh = dgvwgd.SelectedRows[0].Cells[0].Value.ToString();
            string pch = dgvwgd.SelectedRows[0].Cells[1].Value.ToString();
            string sqlstr = "select top 3000 * from WMS_Com_Log where ComResult=1 and DOCID='"+wgdh+"'";
            DataSet ds = null;
            ds = DbEntry.Context.ExecuteDataset(sqlstr);

            if (ds != null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
            {
                if (MessageBox.Show("�깤���Ѿ��ش�NC���Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            sqlstr = "select top 3000 * from WMS_Com_Log where ComResult=1 and DOCID='" + pch + "'";

            ds = DbEntry.Context.ExecuteDataset(sqlstr);

            if (ds != null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count > 0)
            {
                if (MessageBox.Show("���ϳ��ⵥ�Ѿ��ش�NC���Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            if (int.Parse(dgvwgd.SelectedRows[0].Cells["colwcbz"].Value.ToString()) < 2)
            {
                MessageBox.Show("�������ã�","ϵͳ��ʾ");
                return;
            }
            
            if (MessageBox.Show("�Ƿ������깤�������κš�" + pch + "��", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            try
            {
                DbEntry.UsingTransaction(delegate()
                        {
                            sqlstr = "update WMS_Bms_Rec_WGD set wcbz=1 , sendend=0 where pch='" + pch + "'";
                            DbEntry.Context.ExecuteNonQuery(sqlstr);

                            sqlstr = "insert into WMS_Bms_Rec_WGD_ManageLog(barcode,zl,sx,gh,yzl,ysx,ygh,opetype,oper)" +
                                 " values('" + wgdh + "','0',' ',0,0,' ',0,'�����깤��','" + Public.usermangno + "')";
                            DbEntry.Context.ExecuteNonQuery(sqlstr);
                            MessageBox.Show("���óɹ���");
                            searchwgd();
                        });
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ��:" + ex.Message, "ϵͳ��ʾ");
            }
        }

        private void frmwgdmanage_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            dgvwgd.AutoGenerateColumns = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvwgd.SelectedRows.Count == 0)
            {
                MessageBox.Show("ѡ���깤����", "ϵͳ��ʾ");
                return;
            }
            if (!Public.getWeightManageAuthority(Public.usermangno, "ReCheck"))
            {
                MessageBox.Show("û��Ȩ�ޣ�", "ϵͳ��ʾ");
                return;
            }
            string wgdh = dgvwgd.SelectedRows[0].Cells[0].Value.ToString();
            string pch = dgvwgd.SelectedRows[0].Cells[1].Value.ToString();
            string sqlstr = "select top 3000 * from WMS_Com_Log where ComResult=1 and DOCID='" + wgdh + "'";
            DataSet ds = null;
            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (MessageBox.Show("�깤���Ѿ��ش�NC���Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            sqlstr = "select top 3000 * from WMS_Com_Log where ComResult=1 and DOCID='" + pch + "'";

            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (MessageBox.Show("���ϳ��ⵥ�Ѿ��ش�NC���Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            if (dgvwgd.SelectedRows[0].Cells["colzjbz"].Value.ToString() == "0")
            {
                MessageBox.Show("δ�ʼ죬���������ʼ죡", "ϵͳ��ʾ");
                return;
            }
            sqlstr = "select count(1) as fcount from WMS_Bms_Inv_Info where pch='"+pch+"'";
            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            if (int.Parse(ds.Tables[0].Rows[0]["fcount"].ToString()) > 0)
            {
                if (MessageBox.Show("�Ѿ��������Ƿ������������ɾ�����������߲�", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            sqlstr = "select count(1) as fcount from WMS_Bms_Inv_OutInfo where pch='"+pch+"'";
            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            if (int.Parse(ds.Tables[0].Rows[0]["fcount"].ToString()) > 0)
            {
                MessageBox.Show("�Ѿ����⣬���������ʼ�", "ϵͳ��ʾ"); return;
            }
            if (MessageBox.Show("�Ƿ������ʼ죬ɾ�����������ָܻ���", "ϵͳ����", MessageBoxButtons.YesNo) == DialogResult.No) return;
            try
            {
                DbEntry.UsingTransaction(delegate()
                            {
                                sqlstr = "update WMS_Bms_Rec_WGD set PGBZ=0,wcbz=0,zjbz=0,sendend=0 where pch='" + pch + "'";

                                DbEntry.Context.ExecuteNonQuery(sqlstr);
                                sqlstr = "delete from WMS_Bms_Inv_Info where pch='" + pch + "'";
                                DbEntry.Context.ExecuteNonQuery(sqlstr);
                                
                                sqlstr = "delete from Wms_Bms_Inv_Bzzl where pch='" + pch + "'";
                                DbEntry.Context.ExecuteNonQuery(sqlstr);
                                sqlstr = "delete from  WMS_Bms_Rec_WGD_Liquid where pch='" + pch + "'";
                                DbEntry.Context.ExecuteNonQuery(sqlstr);
                                sqlstr = "insert into WMS_Bms_Rec_WGD_ManageLog(barcode,zl,sx,gh,yzl,ysx,ygh,opetype,oper)" +
                                     " values('" + wgdh + "','0',' ',0,0,' ',0,'�����ʼ��깤��','" + Public.usermangno + "')";
                                DbEntry.Context.ExecuteNonQuery(sqlstr);
                                MessageBox.Show("�����ɹ���");
                                searchwgd();
                            }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ��:" + ex.Message, "ϵͳ��ʾ");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dgvwgd.SelectedRows.Count == 0)
            {
                MessageBox.Show("ѡ���깤����", "ϵͳ��ʾ");
                return;
            }
            if (!Public.getWeightManageAuthority(Public.usermangno, "DelWgd"))
            {
                MessageBox.Show("û��Ȩ�ޣ�", "ϵͳ��ʾ");
                return;
            }
            string wgdh = dgvwgd.SelectedRows[0].Cells[0].Value.ToString();
            string pch = dgvwgd.SelectedRows[0].Cells[1].Value.ToString();
            string sqlstr = "select top 3000 * from WMS_Com_Log where ComResult=1 and DOCID='" + wgdh + "'";
            DataSet ds = null;

            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (MessageBox.Show("�깤���Ѿ��ش�NC���Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            sqlstr = "select top 3000 * from WMS_Com_Log where ComResult=1 and DOCID='" + pch + "'";

            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (MessageBox.Show("���ϳ��ⵥ�Ѿ��ش�NC���Ƿ������", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            }
            string wcbz=dgvwgd.SelectedRows[0].Cells["colwcbz"].ToString();
            string msg="";
            switch(wcbz)
            {
                case "0":
                    msg="�Ƿ�ɾ����";
                    break;
                case "1":
                    msg="�Ѿ��������Ƿ�ɾ����";
                    break;
                case "2":
                    msg="�Ѿ������������Ƿ�ɾ��?";
                    break;
                case "3":
                    msg="�Ѿ���⣬�Ƿ�ɾ����";
                    break;
                default:
                    msg="�Ƿ�ɾ����";
                    break;
            }
            sqlstr = "select count(1) as fcount from WMS_Bms_Inv_OutInfo where pch='" + pch + "'";
            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            if (int.Parse(ds.Tables[0].Rows[0]["fcount"].ToString()) > 0)
            {
                MessageBox.Show("�Ѿ����⣬����ɾ��", "ϵͳ��ʾ"); return;
            }
            if (MessageBox.Show(msg+"ɾ�����������ָܻ���", "ϵͳ����", MessageBoxButtons.YesNo) == DialogResult.No) return;
            try
            {
                DbEntry.UsingTransaction(delegate()
                {
                    sqlstr = "delete WMS_Bms_Rec_WGD where pch='" + pch + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    sqlstr = "delete WMS_Bms_Rec_WGD_item where pch='" + pch + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    sqlstr = "delete from WMS_Bms_Inv_Info where pch='" + pch + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    sqlstr = "delete from WMS_Bms_Rec_WGD_Free where pch='" + pch + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    sqlstr = "delete from WMS_Bms_Rec_WGD_Item_Free where pch='" + pch + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    
                    sqlstr = "delete from Wms_Bms_Inv_Bzzl where pch='" + pch + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    sqlstr = "delete from  WMS_Bms_Rec_WGD_Liquid where pch='" + pch + "'";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    sqlstr = "insert into WMS_Bms_Rec_WGD_ManageLog(barcode,zl,sx,gh,yzl,ysx,ygh,opetype,oper)" +
                         " values('" + wgdh + "','0',' ',0,0,' ',0,'ɾ���깤��','" + Public.usermangno + "')";
                    DbEntry.Context.ExecuteNonQuery(sqlstr);
                    
                    MessageBox.Show("�����ɹ���");
                    searchwgd();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ʧ��:" + ex.Message, "ϵͳ��ʾ");
            }
            
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            searchwgd();
        }

        private void dgvwgd_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvwgd.SelectedRows.Count == 0) return;
            string pch = dgvwgd.SelectedRows[0].Cells[1].Value.ToString();
            string sqlstr = "SELECT RANK() OVER (ORDER BY barcode DESC) AS ���,* from WMS_Bms_Inv_Info  where 1=1 and pch='" + pch + "'  order by barcode";
            DataSet ds = null;
            
            ds = DbEntry.Context.ExecuteDataset(sqlstr);
            dgv.DataSource = ds.Tables[0];
        }
    }
}