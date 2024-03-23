using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Dossiers.Models
{
    public class Mysps
    {
        public static string GetConnection
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Contxt"].ConnectionString;
            }
        }

        public static DataTable Getstudents()
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cd = new SqlCommand("getstudents", con);
            cd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                sd.SelectCommand = cd;
                sd.Fill(dt);
            }
            catch (Exception) { }
            return dt;
        }
        public static DataTable Getusers()
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cd = new SqlCommand("getusers", con);
            cd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                sd.SelectCommand = cd;
                sd.Fill(dt);
            }
            catch (Exception) { }
            return dt;
        }

        public static DataTable Getcourses()
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cd = new SqlCommand("getcourses", con);
            cd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                sd.SelectCommand = cd;
                sd.Fill(dt);
            }
            catch (Exception) { }
            return dt;

        }


        public static DataTable GetStudentCourses(int? Sid)
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cd = new SqlCommand("SELECT * FROM StudentCourses WHERE SId="+Sid, con);
            cd.CommandType = CommandType.Text;
            SqlDataAdapter sd = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                sd.SelectCommand = cd;
                sd.Fill(dt);
            }
            catch (Exception) { }
            return dt;
        }

        public static string DeleteOldCourses(int? Sid)
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cmd = new SqlCommand("DELETE FROM StudentCourses WHERE SId=" + Sid, con);
            cmd.CommandType = CommandType.Text;
            string res = "0";
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                res = "1";
            }
            catch (Exception) { }
            finally { con.Close(); }
            return res;
        }
        
        public static string DeleteMultipleCourses(string Ids)
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cmd = new SqlCommand("DeleteMultipleCourses", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Ids", Ids);
            string res = "0";
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                res = "1";
            }
            catch (Exception) { }
            finally { con.Close(); }
            return res;
        }

        public static string InsertOTP(int? Sid, int? OTP)
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO OTP(StID,OTP) VALUES(" + Sid + "," + OTP + ")", con);
            cmd.CommandType = CommandType.Text;
            string res = "0";
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                res = "1";
            }
            catch (Exception) { }
            finally { con.Close(); }
            return res;
        }

        public static string GetOtp(int? Sid)
        {
            SqlConnection con = new SqlConnection(GetConnection);
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 OTP FROM Otp WHERE StID=" + Sid + " AND IsUsed=0 ORDER BY Id DESC", con);
            cmd.CommandType = CommandType.Text;
            string res = "0";
            try
            {
                con.Open();
                var r = cmd.ExecuteScalar();
                if (r != null)
                    res = r.ToString();
            }
            catch (Exception) { }
            finally { con.Close(); }
            return res;
        }
    }
}