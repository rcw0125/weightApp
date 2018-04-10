using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FastReport;

namespace WeightApp
{
    public partial class frmprintmodule : Form
    {
        public frmprintmodule()
        {
            InitializeComponent();
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmprintmodule_Load(object sender, EventArgs e)
        {
            listmodule();
        }
        private string[] ListDirs(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            DirectoryInfo[] items = d.GetDirectories();
            string fl="";
            foreach (DirectoryInfo item in items)
            {
                fl += item.Name+",";
            }
            string[] tl;
            if (fl.Length > 0) fl = fl.Substring(0,fl.Length-1);
            tl = fl.Split(',');
            return tl;
        }
        private void listmodule()
        {
            dgvm.Rows.Clear();
            dgv.Rows.Clear();
            string PrintModule = Public.GetXmlValue("PrintModuleName");
            if (PrintModule == "") PrintModule = "��׼ģ��";
            string[] tl = null;
            tl=ListDirs(Application.StartupPath + "\\report");
            foreach (string s in tl)
            {
                string isck = "false";
                if (PrintModule == s) isck = "true";
                string[] r ={ s, isck };
                dgvm.Rows.Add(r);
            }
            dgvm.Rows[0].Selected=true;
            
        }
        private void findfile(string path)
        {
            dgv.Rows.Clear();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo f in dir.GetFiles("*.fr3 "))
            {
                string name = f.Name;
                string namef = "";
                if (name=="ckcz.fr3") namef="���ڲı�ǩ";
                if (name == "ptcz.fr3") namef = "��׼��ͨ�ı�ǩ";
                if (name == "ptcz1.fr3") namef = "��ͨ�ı�ǩ�������Ժ�������Ϣ";
                if (name == "ptcz2.fr3") namef = "��ͨ�ı�ǩ������";
                if (name == "ptcz3.fr3") namef = "��ͨ�ı�ǩ��������Ϣ";
                if (name == "ptcz4.fr3") namef = "��ͨ�ı�ǩ�����Ժ�������Ϣ";
                if (name == "ptcz5.fr3") namef = "R1/2���Ա�ǩ";
                string[] r = { name, namef };
                dgv.Rows.Add(r);
            }

        }
        private void dgvm_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvm.SelectedRows.Count == 0) return;
            string modulename = dgvm.SelectedRows[0].Cells[0].Value.ToString();
            string f = Application.StartupPath + "\\report\\" + modulename;
            if (!Directory.Exists(f)) return;
            findfile(f);
        }

        private void btn_lbdesign_Click(object sender, EventArgs e)
        {
            if (dgvm.SelectedRows.Count == 0) return;
            if (dgv.SelectedRows.Count == 0) return;
            string modename = dgvm.SelectedRows[0].Cells[0].Value.ToString();
            string lbname = dgv.SelectedRows[0].Cells[0].Value.ToString();
            if (modename == "��׼ģ��")
            {
                MessageBox.Show("��׼ģ�岻������ƣ�", "ϵͳ��ʾ");
                return;
            }
            string filefullpath = Application.StartupPath + "\\report\\" + modename + "\\" + lbname;
            TfrxReportClass Report = Report = new TfrxReportClass();
            if (!File.Exists(filefullpath))
            {
                MessageBox.Show("��ǩ�ļ������ڣ�","ϵͳ��ʾ");
                return;
            }
            try
            {
                Report.LoadReportFromFile(filefullpath);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("���ش�ӡģ���������ϵϵͳ����Ա��" + ex.Message, "ϵͳ����");
                return;
            }
            Report.DesignReport();

        }

        private void btn_setuse_Click(object sender, EventArgs e)
        {
            if (dgvm.SelectedRows.Count == 0) return;
            string modename=dgvm.SelectedRows[0].Cells[0].Value.ToString();
            if (MessageBox.Show("�����õ�ǰģ����Ϊ����״̬?","ϵͳ��ʾ",MessageBoxButtons.YesNo) == DialogResult.No) return;
            Public.SetXmlValue("PrintModuleName", modename);
            listmodule();
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (dgvm.SelectedRows.Count == 0) return;
            string modename = dgvm.SelectedRows[0].Cells[0].Value.ToString();
            if (modename == "��׼ģ��")
            {
                MessageBox.Show("��׼ģ�岻����ɾ����", "ϵͳ��ʾ");
                return;
            }
            if (MessageBox.Show("������ɾ��ѡ�е�ģ����?", "ϵͳ��ʾ", MessageBoxButtons.YesNo) == DialogResult.No) return;
            string dirfullpath = Application.StartupPath + "\\report\\" + modename;
            Directory.Delete(dirfullpath, true);
            listmodule();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            frminputdirname frm = new frminputdirname();
            frm.ShowDialog();
            string vv = frminputdirname.inputmodename;
            
            if (vv != null&&vv!="")
            {
                string mpath = Application.StartupPath + "\\report\\��׼ģ��";
                if (!Directory.Exists(mpath))
                {
                    MessageBox.Show("��׼ģ�岻���ڣ���ϵϵͳ����Ա��","ϵͳ��ʾ");
                    return;
                }
                string fpath = Application.StartupPath + "\\report\\";
                try
                {

                    //�����ļ���
                    Directory.CreateDirectory(fpath + vv);
                    //�����ļ�

                    DirectoryInfo dir = new DirectoryInfo(mpath);
                    foreach (FileInfo f in dir.GetFiles("*.fr3 "))
                    {
                        string name = f.Name;
                        File.Copy(f.FullName,fpath+vv+"\\"+f.Name);
                    }
                    listmodule();
                    MessageBox.Show("�����ɹ���","ϵͳ��ʾ");
                }
                catch (Exception err)
                {
                    MessageBox.Show("����ʧ�ܣ�"+err.Message,"ϵͳ��ʾ");
                }
            }
        }
    }
}