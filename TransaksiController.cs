using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalMobil.Models;
using Npgsql;
using System;
using System.Windows.Forms;

namespace RentalMobil.Controller
{
    public class TransaksiController
    {
        public bool CreateTransaksi(int idPelanggan, int idKendaraan,
                                  DateTime tanggalSewa, DateTime tanggalKembali,
                                  decimal hargaPerHari)
        {
            using (var connection = Database.GetConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Hitung total hari dan harga
                        int hari = (tanggalKembali - tanggalSewa).Days;
                        decimal totalHarga = hari * hargaPerHari;

                        // 1. Buat transaksi
                        string queryTransaksi = @"
                            INSERT INTO transaksi 
                            (id_pelanggan, id_kendaraan, tanggal_sewa, tanggal_kembali, total_harga, status_pembayaran)
                            VALUES (@id_pelanggan, @id_kendaraan, @tanggal_sewa, @tanggal_kembali, @total_harga, 'belum_lunas')";

                        using (var cmd = new NpgsqlCommand(queryTransaksi, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id_pelanggan", idPelanggan);
                            cmd.Parameters.AddWithValue("@id_kendaraan", idKendaraan);
                            cmd.Parameters.AddWithValue("@tanggal_sewa", tanggalSewa);
                            cmd.Parameters.AddWithValue("@tanggal_kembali", tanggalKembali);
                            cmd.Parameters.AddWithValue("@total_harga", totalHarga);
                            cmd.ExecuteNonQuery();
                        }

                        // 2. Update status kendaraan
                        string queryUpdateKendaraan = @"
                            UPDATE kendaraan 
                            SET status = 'disewa' 
                            WHERE id_kendaraan = @id_kendaraan";

                        using (var cmd = new NpgsqlCommand(queryUpdateKendaraan, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@id_kendaraan", idKendaraan);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Error saat membuat transaksi: {ex.Message}",
                                      "Database Error",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
        }
    }
}


