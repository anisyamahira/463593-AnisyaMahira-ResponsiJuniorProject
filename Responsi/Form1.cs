
using Npgsql;
using Npgsql.Logging;
using System.Data;
using System.Windows.Forms;

namespace Responsi
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = "Host=localhost;Port=2022;Username=postgres;Password=informatika;Database=db_departemen";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }

        private void btnLoaddata_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = "select*from st_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();

                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "FAIL!!", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select*from st_insert()";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_name",txtNamakaryawan.Text);
                cmd.Parameters.AddWithValue("_id_dep", cbDepkaryawan.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("data berhasil diinput", "well done!",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    conn.Close();
                    btnLoaddata.PerformClick();
                    txtNamakaryawan = null;
                    cbDepkaryawan = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error:" + ex.Message, "insert fail!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                txtNamakaryawan.Text = r.Cells["_name"].Value.ToString();
                cbDepkaryawan.Text = r.Cells["_id_dep"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Pilih baris data","Good!" , MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                conn.Open();
                sql = @"select*from st_update(:_id, :_name: _id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_name",txtNamakaryawan);
                cmd.Parameters.AddWithValue("_nama_dep", cbDepkaryawan);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data berhasil diupdate", "Well Done!", MessageBoxButtons.OK,MessageBoxIcon.Information);
                    conn.Close();
                    btnLoaddata.PerformClick();
                    txtNamakaryawan = null;
                    cbDepkaryawan = null;
                    r = null;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Update FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Pilih baris data", "Good!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else{
                //MessageBox.Show("Apakah benar hapus data" + r.Cells["_name"].Value.ToString() + "?", "Hapus data terkonfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButon.Button1 == DialogResult.Yes);
                try
                {
                    conn.Open();
                    sql = @"select*from st_delete(:_id_karyawan)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                    if((int)cmd.ExecuteScalar() == 1)
                    {
                        MessageBox.Show("Data berhasil dihapus", "Well Done!", MessageBoxButtons.OK,MessageBoxIcon.Information);
                        conn.Close();
                        btnLoaddata.PerformClick();
                        txtNamakaryawan = null;
                        cbDepkaryawan = null;
                        r = null;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Delete FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            
        }
    }
}