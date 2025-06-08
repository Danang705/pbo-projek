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
    public partial class SewaKendaraan : Form
    {
        private Kendaraan _kendaraan;
        private Pelanggan _pelanggan;
        // Fix the invalid declaration of the TransaksiController field
        private readonly TransaksiController _transaksiController;

      

        // Add missing controls as fields (if not already present in the designer)
        private Label lblMerkModel;
        private Label lblTahunPlat;
        private Label lblHargaPerHari;
        private Label lblTotal;
        private DateTimePicker dtpTanggalMulai;
        private DateTimePicker dtpTanggalSelesai;

        public SewaKendaraan(Kendaraan kendaraan, Pelanggan pelanggan)
        {
            InitializeComponent();
            _kendaraan = kendaraan;
            _pelanggan = pelanggan;
            _transaksiController = new Controller.TransaksiController();

            // Initialize controls if not using designer
            InitializeCustomControls();

            InitializeForm();
        }

        private void InitializeCustomControls()
        {
            // Only add these if not using the designer
            if (lblMerkModel == null)
            {
                lblMerkModel = new Label { Name = "lblMerkModel", Location = new Point(10, 10), AutoSize = true };
                this.Controls.Add(lblMerkModel);
            }
            if (lblTahunPlat == null)
            {
                lblTahunPlat = new Label { Name = "lblTahunPlat", Location = new Point(10, 40), AutoSize = true };
                this.Controls.Add(lblTahunPlat);
            }
            if (lblHargaPerHari == null)
            {
                lblHargaPerHari = new Label { Name = "lblHargaPerHari", Location = new Point(10, 70), AutoSize = true };
                this.Controls.Add(lblHargaPerHari);
            }
            if (lblTotal == null)
            {
                lblTotal = new Label { Name = "lblTotal", Location = new Point(10, 100), AutoSize = true };
                this.Controls.Add(lblTotal);
            }
            if (dtpTanggalMulai == null)
            {
                dtpTanggalMulai = new DateTimePicker { Name = "dtpTanggalMulai", Location = new Point(10, 130) };
                dtpTanggalMulai.ValueChanged += dtpTanggalMulai_ValueChanged;
                this.Controls.Add(dtpTanggalMulai);
            }
            if (dtpTanggalSelesai == null)
            {
                dtpTanggalSelesai = new DateTimePicker { Name = "dtpTanggalSelesai", Location = new Point(10, 160) };
                this.Controls.Add(dtpTanggalSelesai);
            }
        }

        private void InitializeForm()
        {
            // Populate vehicle information
            lblMerkModel.Text = $"{_kendaraan.merk} {_kendaraan.model}";
            lblTahunPlat.Text = $"{_kendaraan.tahun} • {_kendaraan.nomor_plat}";
            lblHargaPerHari.Text = _kendaraan.harga_sewa_perhari.ToString("C") + " /hari";

            // Set default rental dates
            dtpTanggalMulai.Value = DateTime.Today;
            dtpTanggalSelesai.Value = DateTime.Today.AddDays(1);

            // Calculate initial total
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            DateTime mulai = dtpTanggalMulai.Value;
            DateTime selesai = dtpTanggalSelesai.Value;
            if (selesai <= mulai)
            {
                lblTotal.Text = "Total: 0";
                return;
            }
            int hari = (int)(selesai - mulai).TotalDays;
            decimal totalHarga = hari * _kendaraan.harga_sewa_perhari;
            lblTotal.Text = $"Total: {totalHarga:C} untuk {hari} hari";
        }

        private void dtpTanggalMulai_ValueChanged(object sender, EventArgs e)
        {
            // Ensure end date is after start date
            if (dtpTanggalSelesai.Value <= dtpTanggalMulai.Value)
            {
                dtpTanggalSelesai.Value = dtpTanggalMulai.Value.AddDays(1);
            }
            CalculateTotal();
        }

        private void btnSewa_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime mulai = dtpTanggalMulai.Value.Date;
                DateTime selesai = dtpTanggalSelesai.Value.Date;

                if (selesai <= mulai)
                {
                    MessageBox.Show("Tanggal selesai harus setelah tanggal mulai!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Create transaction
                bool success = _transaksiController.CreateTransaksi(
                    _pelanggan.id_pelanggan,
                    _kendaraan.id_kendaraan,
                    mulai,
                    selesai,
                    _kendaraan.harga_sewa_perhari
                );

                if (success)
                {
                    MessageBox.Show("Penyewaan berhasil dibuat!\nSilakan lakukan pembayaran.", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Gagal membuat penyewaan. Silakan coba lagi.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
