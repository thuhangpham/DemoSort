using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        #region Biến toàn cục.
        int[] a;
        int spt = 0;
        Label[] chi_so;
        Label[] node;
        int kich_thuoc, khoang_cach, cochunode,
            cochuchi_so, khoangcach_y, canh_le;
        bool tao_mang = false;
        int sleep_millis = 0, move = 1;
        #endregion

        #region Form main.
        public Form1()
        {
            InitializeComponent();
            rad_Ascending.Checked = true;
            rad_Decending.Checked = false;
            btn_restart.Enabled = false;
            lbl_hoanthanh.Visible = false;
            rad_sellec.Checked = true;
            btn_move.Enabled = false;
        }
        #endregion

        #region Open file notepad.
        private void btn_openfile_Click(object sender, EventArgs e)
        {
            Process notepad = new Process();
            notepad.StartInfo.FileName = "notepad.exe";
            notepad.StartInfo.Arguments = Application.StartupPath + @"/Input file.txt";
            notepad.Start();
        }
        #endregion

        #region Read file.
        private void btn_readfile_Click(object sender, EventArgs e)
        {
            if (tao_mang == true)
            {
                XoaMang(node, chi_so);
                lbl_hoanthanh.Visible = false;
            }
            string file_text = null;
            try
            {
                file_text = File.ReadAllText(Application.StartupPath + @"/Input file.txt");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Không tìm thấy file!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                spt = Convert.ToInt32((file_text.Trim().Split(' ', '\n', '\t'))[0]);
            }
            catch
            {
                MessageBox.Show("Số lượng phần tử không hợp lệ!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (spt < 2)
            {
                MessageBox.Show("Số lượng phần tử ít nhất phải là 2!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            int i = 0;
            a = new int[spt];
            try
            {
                while (i < spt)
                {
                    a[i] = Convert.ToInt32((file_text.Trim().Split(' ', '\n', '\t'))[i + 1]);
                    i++;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Phần tử không hợp lệ!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (IndexOutOfRangeException)
            {
                for (int k = i; k < spt - 1; k++)
                    a[k] = 0;
            }

            TaoMang(Properties.Resources.chuaxep);
            lbl_hoanthanh.Visible = false;
            btn_move.Enabled = false;
            grb_thuattoan.Enabled = true;
            grb_Sort.Enabled = true;
            rad_sellec.Enabled = true;
            rad_bubble.Enabled = true;
            btn_move.Enabled = true;

        }
        #endregion

        #region Tạo Mảng.
        void TaoMang(System.Drawing.Image image)
        {
            for (int i = 0; i < spt; i++)
            {
                if (spt < 2 || spt > 15)
                {
                    MessageBox.Show("1 < số phần tử < 16!\nXin đọc lại file.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (a[i] < 0 || spt > 100)
                {
                    MessageBox.Show("0 < phần tử < 100!\nXin đọc lại file.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            tao_mang = true;
            kich_thuoc = 100; khoang_cach = 10; cochunode = 35;
            cochuchi_so = 20; khoangcach_y = 310;
            if (spt > 10)
            {
                kich_thuoc = 80;
                khoang_cach = 5; cochunode = 28;
            }
            canh_le = (Width - (khoang_cach + kich_thuoc) * spt) / 2;
            node = new Label[spt];
            chi_so = new Label[spt];
            for (int i = 0; i < spt; i++)
            {
                node[i] = new Label();
                node[i].Location = new Point(canh_le + (khoang_cach + kich_thuoc) * i, khoangcach_y);
                node[i].Width = kich_thuoc;
                node[i].Height = kich_thuoc;
                node[i].Text = a[i].ToString();
                node[i].TextAlign = ContentAlignment.MiddleCenter;
                node[i].ForeColor = Color.White;
               // node[i].Font = new Font(this.Font, FontStyle.Bold);
                node[i].Font = new System.Drawing.Font("Arial", cochunode, FontStyle.Bold);
                node[i].BackgroundImage = image;
                node[i].BackgroundImageLayout = ImageLayout.Stretch;
                node[i].BorderStyle = 0;
                node[i].BackColor = Color.Transparent;
                this.Controls.Add(node[i]);
                chi_so[i] = new Label();
                chi_so[i].Location = new Point(canh_le + (khoang_cach + kich_thuoc) * i, khoangcach_y + kich_thuoc * 3 - 20);
                chi_so[i].Width = kich_thuoc;
                chi_so[i].Height = kich_thuoc;
                chi_so[i].Text = i.ToString();
                chi_so[i].TextAlign = ContentAlignment.MiddleCenter;
                chi_so[i].ForeColor = Color.Orange;
                chi_so[i].Font = new Font(this.Font, FontStyle.Bold);
                chi_so[i].Font = new System.Drawing.Font("Arial", cochuchi_so, FontStyle.Bold);
                chi_so[i].BorderStyle = 0;
                chi_so[i].BackColor = Color.Transparent;
                this.Controls.Add(chi_so[i]);
            }
        }
        #endregion

        #region Xóa Mảng.
        void XoaMang(Label[] node, Label[] chi_so)
        {
            tao_mang = false;
            grb_thuattoan.Enabled = false;
            rad_bubble.Enabled = false;
            rad_sellec.Enabled = false;
            foreach (var n in node)
                Controls.Remove(n);
            foreach (var i in chi_so)
                Controls.Remove(i);
        }
        #endregion

        #region Các hàm Di chuyển Node trái, phải, lên, xuống.
        void NodeSangTrai(Label node, int kc)
        {
            int loc_x = node.Location.X;
            while (node.Location.X >= loc_x - kc)
            {
                Application.DoEvents();
                node.Location = new Point(node.Location.X - move, node.Location.Y);
                node.BackgroundImage = Properties.Resources.manglon;
                Thread.Sleep(sleep_millis);
            }
        }
        void NodeSangPhai(Label node, int kc)
        {
            int loc_x = node.Location.X;
            while (node.Location.X <= loc_x + kc)
            {
                Application.DoEvents();
                node.Location = new Point(node.Location.X + move, node.Location.Y);
                node.BackgroundImage = Properties.Resources.manglon;
                Thread.Sleep(sleep_millis);
            }
        }
        void NodeDiXuong(Label node)
        {
            int loc_y = node.Location.Y, loc_x = node.Location.X;
            while (node.Location.Y <= (loc_y + kich_thuoc * 1.3))
            {
                Application.DoEvents();
                node.BackgroundImage = Properties.Resources.manglon;
                node.Location = new Point(node.Location.X, node.Location.Y + move);
                Thread.Sleep(sleep_millis);
            }
        }
        void NodeDiLen(object opjnode)
        {
            Label node = (Label)opjnode;
            int loc_y = node.Location.Y, loc_x = node.Location.X;
            while (node.Location.Y >= (loc_y - kich_thuoc * 1.3))
            {
                Application.DoEvents();
                node.Location = new Point(node.Location.X, node.Location.Y - move);
                node.BackgroundImage = Properties.Resources.manglon;
                Thread.Sleep(sleep_millis);
            }
            // Tra lai color
        }
        #endregion

        #region Bubble sort
        void BubbleSort()
        {
            int j = 0;
            for (int i = 0; i < spt; i++)
            {
                for (j = spt - 1; j > i; j--)
                {
                    if (rad_Ascending.Checked)
                    {
                        try
                        {
                            if (Convert.ToInt32(node[j].Text) < Convert.ToInt32(node[j - 1].Text))
                            {
                                HoanViNode(node[j - 1], node[j]);
                                HoanViNode_Ngam(j - 1, j);
                                HoanViChiSo_Ngam(j - 1, j);
                                // tra lai image
                                node[j - 1].BackgroundImage = Properties.Resources.chuaxep;
                                node[j].BackgroundImage = Properties.Resources.chuaxep;
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            if (Convert.ToInt32(node[j].Text) > Convert.ToInt32(node[j - 1].Text))
                            {
                                HoanViNode(node[j - 1], node[j]);
                                HoanViNode_Ngam(j - 1, j);
                                HoanViChiSo_Ngam(j - 1, j);
                                // tra lai image
                                node[j - 1].BackgroundImage = Properties.Resources.chuaxep;
                                node[j].BackgroundImage = Properties.Resources.chuaxep;
                            }
                        }
                        catch { }
                    }

                }
                node[j].BackgroundImage = Properties.Resources.daxep;
            }
            for (int i = 0; i < spt; i++)
                node[i].BackgroundImage = Properties.Resources.index;
            btn_restart.Enabled = true;
        }
        #endregion

        #region Sellection sort
        void SellectionSort()
        {
            int vitrimin, i, j;

            for (i = 0; i < spt; i++)
            {
                vitrimin = i;
                for (j = i + 1; j < spt; j++)
                {
                    if (rad_Ascending.Checked)
                    {
                        try
                        {
                            if (Convert.ToInt32(node[j].Text) < Convert.ToInt32(node[vitrimin].Text))
                                vitrimin = j;
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            if (Convert.ToInt32(node[j].Text) > Convert.ToInt32(node[vitrimin].Text))
                                vitrimin = j;
                        }
                        catch { }
                    }
                }
                if (vitrimin != i)
                {
                    HoanViNode(node[i], node[vitrimin]);
                    HoanViNode_Ngam(i, vitrimin);
                    HoanViChiSo_Ngam(i, vitrimin);
                    // tra lai image
                    node[i].BackgroundImage = Properties.Resources.chuaxep;
                    node[vitrimin].BackgroundImage = Properties.Resources.chuaxep;
                }
                node[i].BackgroundImage = Properties.Resources.index2;
            }
            //
            for (i = 0; i < spt; i++)
            {
                node[i].BackgroundImage = Properties.Resources.daxep;
                node[i].Refresh();
            }
            btn_restart.Enabled = true;
        }
        #endregion

        #region Các hàm hoán vị.
        public void HoanViNode_Ngam(int t1, int t2)
        {
            Label Temp = node[t1];
            node[t1] = node[t2];
            node[t2] = Temp;
        }

        public void HoanViChiSo_Ngam(int t1, int t2)
        {
            Label Temp = chi_so[t1];
            chi_so[t1] = chi_so[t2];
            chi_so[t2] = Temp;
        }
        void HoanViNode(Label lb_1, Label lb_2)
        {
            int locx_1 = lb_1.Location.X, locx_2 = lb_2.Location.X;
            if (locx_2 - locx_1 < kich_thuoc * 2)
            {
                NodeDiLen(lb_2);
                NodeSangPhai(lb_1, (locx_2 - locx_1));
                NodeSangTrai(lb_2, (locx_2 - locx_1));
                NodeDiXuong(lb_2);
            }
            else
            {
                NodeDiLen(lb_2);
                NodeDiXuong(lb_1);
                NodeSangTrai(lb_2, (locx_2 - locx_1));
                NodeSangPhai(lb_1, (locx_2 - locx_1));
                NodeDiXuong(lb_2);
                NodeDiLen(lb_1);
            }
        }
        #endregion

        #region Sự kiện Sort, stop, rad_sellec, rad_bubbel.
        // button Sort.
        private void btn_move_Click(object sender, EventArgs e)
        {
            trb_tocdo.Enabled = true;
            btn_move.Enabled = false;
            grb_thuattoan.Enabled = false;
            grb_Sort.Enabled = false;
            rad_bubble.Enabled = false;
            rad_sellec.Enabled = false;
            btn_restart.Enabled = true;
            if (rad_bubble.Checked == true)
                BubbleSort();
            if (rad_sellec.Checked == true)
                SellectionSort();
            lbl_hoanthanh.Visible = true;
            trb_tocdo.Enabled = false;
        }
        private void rad_sellec_CheckedChanged(object sender, EventArgs e)
        {
            btn_move.Enabled = true;
        }
        private void rad_bubble_CheckedChanged(object sender, EventArgs e)
        {
            btn_move.Enabled = true;
        }
        private void btn_restart_Click(object sender, EventArgs e)
        {
            XoaMang(node, chi_so);
            btn_restart.Enabled = false;
            trb_tocdo.Enabled = false;
            rad_bubble.Checked = false;
            rad_sellec.Checked = false;
            btn_move.Enabled = false;
            rad_Ascending.Checked = true;
            rad_Decending.Checked = false;
            lbl_hoanthanh.Visible = false;
            rad_sellec.Checked = true;
            btn_move.Enabled = false;
            Application.Restart();
        }
        private void trb_tocdo_Scroll(object sender, EventArgs e)
        {
            sleep_millis = 11 - trb_tocdo.Value;
            if (trb_tocdo.Value < 1)
                move = 0;
            if (1 <= trb_tocdo.Value && trb_tocdo.Value <= 3)
                move = 1;
            if (3 <= trb_tocdo.Value && trb_tocdo.Value <= 6)
                move = 2;
            if (6 <= trb_tocdo.Value && trb_tocdo.Value <= 10)
                move = 3;
        }
        #endregion

        #region tool bar
        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Step 1: Click Open File.\nStep 2: Click Read File.\nStep 3: Seclec Sort Algorithms.\nStep 4: Selec Ascending or Decending.\nStep 5: Click Sort.",
                "View Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void aboutDesingersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("STT\t        Name\t                Student's ID\n\n  1\t Ngô Thanh Đông\t\t14110039\n  2\t Phạm Thị Thu Hằng\t\t14110051\n  3\t Nguyễn Cao Trí\t\t14110210",
                "About Designers", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_readfile_Click(sender, e);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_openfile_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
