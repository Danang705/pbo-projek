using RentalMobil.Controller;
using RentalMobil.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentalMobil.Views.pelanggan_
{
    public partial class DashboardPelanggan : Form
    {
        private readonly KendaraanController kendaraanController;
        public Pelanggan Pelanggan { get; private set; }
        public DashboardPelanggan(Pelanggan pelanggan)
        {
            InitializeComponent();
            kendaraanController = new KendaraanController();
            Pelanggan = pelanggan;

            // Pasangkan event click
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;

            LoadData();

        }

        private void LoadData()
        {
            var kendaraanList = kendaraanController.GetAllKendaraan();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "id_kendaraan",
                Name = "id_kendaraan"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Merk",
                DataPropertyName = "merk",
                Name = "merk"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Model",
                DataPropertyName = "model",
                Name = "model"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Tahun",
                DataPropertyName = "tahun",
                Name = "tahun"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Nomor Plat",
                DataPropertyName = "nomor_plat",
                Name = "nomor_plat"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Harga Sewa/Hari",
                DataPropertyName = "harga_sewa_perhari",
                Name = "harga_sewa_perhari"
            });

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                DataPropertyName = "status",
                Name = "status"
            });

            var btnSewa = new DataGridViewButtonColumn
            {
                HeaderText = "Aksi",
                Text = "Sewa",
                Name = "btnSewa",
                UseColumnTextForButtonValue = true
            };
            dataGridView1.Columns.Add(btnSewa);

            dataGridView1.DataSource = kendaraanList;

            // Styling DataGridView
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.BackgroundColor = Color.FromArgb(80, 80, 80); // #505050
            dataGridView1.GridColor = Color.FromArgb(120, 120, 120);
            dataGridView1.BorderStyle = BorderStyle.None;

            // Header
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(80, 80, 80);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Trik tambahan agar teks terlihat benar-benar di tengah (secara vertikal)
            dataGridView1.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 15, 0, 15); // top padding bisa kamu sesuaikan

            dataGridView1.ColumnHeadersHeight = 75;// Atur sesuai kebutuhan


            // Baris biasa
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dataGridView1.DefaultCellStyle.ForeColor = Color.White;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromArgb(40, 40, 40);
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Alternating row color
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(60, 60, 60);

            // Lain-lain
            dataGridView1.RowTemplate.Height = 70;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToOrderColumns = false;

            dataGridView1.DataSource = kendaraanList;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnSewa" && e.RowIndex >= 0)
            {
                // Get the selected vehicle data
                var selectedKendaraan = (Kendaraan)dataGridView1.Rows[e.RowIndex].DataBoundItem;

                // Open the rental form
                SewaKendaraan sewaForm = new SewaKendaraan(selectedKendaraan, Pelanggan);
                this.Hide();
                sewaForm.Show();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            AuthForm authForm = new AuthForm();
            this.Hide();
            authForm.Show();
        }

        private void btnDataDiri_Click(object sender, EventArgs e)
        {
            this.Hide();
            EditDataDiri dataDiriPelanggan = new EditDataDiri(Pelanggan);
            dataDiriPelanggan.Show();
        }
    }
}
