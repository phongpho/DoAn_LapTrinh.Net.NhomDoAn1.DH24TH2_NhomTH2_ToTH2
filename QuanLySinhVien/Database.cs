using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms; 

public class Database
{
    private string connectionString = @"Data Source=DESKTOP-TKVQ97S;Initial Catalog=QuanLySinhVien;Integrated Security=True";

    private SqlConnection connection;

    public Database()
    {
        connection = new SqlConnection(connectionString);
    }

    public void OpenConnection()
    {
        try
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message);
        }
    }

    public void CloseConnection()
    {
        if (connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }

    public DataTable GetData(string sqlQuery)
    {
        DataTable dataTable = new DataTable();
        OpenConnection();
        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection))
        {
            adapter.Fill(dataTable);
        }
        CloseConnection();
        return dataTable;
    }

    public int Execute(string sqlQuery)
    {
        int result = 0;
        OpenConnection(); 
        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        {
            result = command.ExecuteNonQuery(); 
        }
        CloseConnection();
        return result;
    }
}